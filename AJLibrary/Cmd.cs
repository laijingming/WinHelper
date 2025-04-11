using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AJLibrary
{
    public class Cmd
    {
        public const int STATUS_ERROR = -1;
        public const int STATUS_START = 1;
        public const int STATUS_RUNNING = 2;
        public const int STATUS_DONE = 3;
        /// <summary>
        /// 存入命令列表。并记录执行结果
        /// </summary>
        public static List<TaskCompletionSource<object>> TaskCompletionSourceList = new List<TaskCompletionSource<object>>();
        public static ProcessStartInfo psi = null;

        //执行结果
        public delegate void DisplayPartialResult(int state, string partialResult="");
        public static DisplayPartialResult displayPartialResult;

        public static int id = 1;
        public static string git_bash_path = "D:\\Git\\git-bash.exe";
        //public static string default_path = @"%USERPROFILE%\\Desktop";
        public static string work_path = @"C:\";


        public static string getCommandId(int id, string command) 
        {
            return id + " " + command;
        }

        public static void makeProcessStartInfo()
        {
            // 创建一个进程启动信息对象
            if (psi == null)
            {
                psi = new ProcessStartInfo
                {
                    //StandardOutputEncoding = Encoding.GetEncoding("GB2312"),// 设置标准输出的编码为UTF-8
                    StandardOutputEncoding = Encoding.UTF8,// 设置标准输出的编码为UTF-8
                    FileName = "cmd.exe", // 使用命令行解释器
                    //FileName = "powershell.exe", // 使用命令行解释器
                    UseShellExecute = false, // 不使用系统外壳程序执行
                    RedirectStandardOutput = true, // 重定向标准输出流
                    RedirectStandardError = true,  // 重定向错误
                    CreateNoWindow = true, // 不创建新窗口
                    //WorkingDirectory = work_path      // 设置工作目录，确保dir命令有效
                };

            }
        }

        /// <summary>
        /// 阻塞执行命令
        /// </summary>
        /// <param name="command"></param>
        public static string ExecCommandSync(string command) 
        {
            string output;
            try
            {
                makeProcessStartInfo();
                psi.WorkingDirectory = work_path;
                psi.Arguments = $"/C {command}"; // 执行命令
                                                 // 启动进程
                Process process = new Process
                {
                    StartInfo = psi
                };
                process.Start();
                // 获取命令的输出
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

            }
            catch (Exception ex)
            {
                output = "Exception:"+ex.Message;
            }

            return output;
        }

        /// <summary>
        /// 命令行输入执行
        /// </summary>
        /// <param name="command"></param>
        public static async void ExecCommandCom(string command)
        {
            try
            {
                string cd_msg = IsDriveSwitchCommand(command);
                if (!string.IsNullOrEmpty(cd_msg))//如果不为空，为切盘操作
                {
                    displayPartialResult?.Invoke(STATUS_RUNNING, cd_msg);
                }
                else
                {


                    makeProcessStartInfo();
                    psi.Arguments = $"/C {command}"; // 执行命令
                    psi.WorkingDirectory = work_path;

                    using (Process process = new Process { StartInfo = psi })
                    {
                        process.Start();

                        // 异步读取并显示命令的输出
                        string outputChunk = await process.StandardOutput.ReadToEndAsync(); // 读取所有输出
                        string errorChunk = await process.StandardError.ReadToEndAsync();   // 读取所有错误

                        // 更新UI，显示结果
                        if (!string.IsNullOrEmpty(outputChunk))
                        {
                            displayPartialResult?.Invoke(STATUS_RUNNING, outputChunk);
                        }

                        if (!string.IsNullOrEmpty(errorChunk))
                        {
                            displayPartialResult?.Invoke(STATUS_ERROR, errorChunk);
                        }

                        process.WaitForExit();
                    }
                }

            }
            catch (Exception ex)
            {
                // 错误处理
                displayPartialResult?.Invoke(STATUS_ERROR, Environment.NewLine + "Exception: " + ex.Message);
            }

            displayPartialResult?.Invoke(STATUS_DONE);
        }
        /// <summary>
        /// 等待任务完成，继续执行下个任务
        /// </summary>
        public static Task<object[]> WaitTaskDone()
        {
            return Task.WhenAll(TaskCompletionSourceList.Select(tcs => tcs.Task));
        }

        /// <summary>
        /// 异步命令
        /// </summary>
        /// <param name="command"></param>
        public static async void ExecAsync(string command) 
        {
            command=command.Trim();
            displayPartialResult?.Invoke(STATUS_START, command);

            //command = command.Replace("&&","\n");
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            try
            {
                TaskCompletionSourceList.Add(tcs);

                makeProcessStartInfo();
                psi.Arguments = $"/C {command}"; // 执行命令
                //psi.WorkingDirectory = work_path;

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();

                    // 异步读取并显示命令的输出
                    StringBuilder outputBuilder = new StringBuilder();
                    char[] buffer = new char[4096];
                    int bytesRead;
                    while ((bytesRead = await process.StandardOutput.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string outputChunk = new string(buffer, 0, bytesRead);
                        outputBuilder.Append(outputChunk);
                        displayPartialResult?.Invoke(STATUS_RUNNING, outputChunk);
                    }
                    while ((bytesRead = await process.StandardError.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string outputChunk = new string(buffer, 0, bytesRead);
                        outputBuilder.Append(outputChunk);
                        displayPartialResult?.Invoke(STATUS_ERROR, outputChunk);
                    }
                    process.WaitForExit();
                    tcs.SetResult(null);

                }

            }
            catch (Exception ex)
            {
                // 错误处理
                displayPartialResult?.Invoke(STATUS_ERROR,Environment.NewLine + "Exception: " + ex.Message);
            }

        }


        /// <summary>
        ///  处理盘符切换，例如 d:, e:
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string IsDriveSwitchCommand(string command)
        {
            string msg = "";
            if (command.StartsWith("cd "))
            {
                //提取命令后面路径
                string path = command.Substring(3).Trim();
                if (path == "..")
                {
                    //返回上级目录
                    work_path = Directory.GetParent(work_path)?.FullName ?? work_path;
                }
                else if (Directory.Exists(path))// 绝对路径
                {
                    work_path = Path.GetFullPath(path);
                }
                else
                {
                    //处理相对路径
                    string combine_path = Path.Combine(work_path, path);
                    if (Directory.Exists(combine_path))
                    {
                        work_path = combine_path;
                    }
                    else
                    {
                        msg = "The system cannot find the path specified.";
                    }
                }
                if (string.IsNullOrEmpty(msg))
                {
                    // 显示目录已改变信息
                    msg = $"Directory changed to {work_path}";
                }

            }
            return msg;
        }


    }
}
