using System;
using System.IO;
using System.Windows.Forms;

namespace AJLibrary
{
    public class FileTool
    {
        public const string TextFileFilter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
        public const string ExcelFileFilter = "Excel 文件 (*.xls;*.xlsx)|*.xls;*.xlsx|所有文件 (*.*)|*.*";
        public const string JsonFileFilter = "JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*";
        public const string DbFileFilter = "db 文件 (*.db)|*.db|所有文件 (*.*)|*.*";
        public static string OpenFile(string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = filter;
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // 用户选择了文件，将文件路径显示在文本框中
                    return openFileDialog.FileName;
                }
                return null; // 或者可以返回空字符串 ""
            }
        }

        public static string OpenFolder()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "选择文件夹";
                folderBrowserDialog.ShowNewFolderButton = false;

                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // 用户选择了文件夹，返回文件夹路径
                    return folderBrowserDialog.SelectedPath;
                }
                return null; // 或者可以返回空字符串 ""
            }
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="contentToSave"></param>
        public static void SaveToFile(string filePath, string contentToSave)
        {
            //string dirPath = Common.ROOTPATH + "cache/";
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                try
                {
                    // 创建目录
                    Directory.CreateDirectory(dirPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建目录时发生错误：" + ex.Message);
                }
            }
            // 使用 StreamWriter 将内容写入文件
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(contentToSave);
            }
        }

        /// <summary>
        /// 加载文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string LoadFromFile(string filePath)
        {
            //filePath = Common.ROOTPATH + "cache/" + filePath;
            string dirPath = Path.GetDirectoryName(filePath);
            string content = "";
            // 检查文件是否存在
            if (File.Exists(filePath))
            {
                // 使用 StreamReader 读取文件内容
                using (StreamReader reader = new StreamReader(filePath))
                {
                    content = reader.ReadToEnd();
                }
            }
            return content;
        }

        /// <summary>
        /// 获取文件或者文件夹最后修改时间
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns></returns>
        public static DateTime GetResourceLastWriteTime(string resourcePath)
        {
            if (File.Exists(resourcePath))
            {
                return File.GetLastWriteTime(resourcePath);
            }
            return Directory.GetLastWriteTime(resourcePath);
        }
    }
}
