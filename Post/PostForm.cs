using AJLibrary;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Post
{
    public partial class PostForm : DevExpress.XtraEditors.XtraForm
    {   
        PostConfigCache postCfgCache = PostConfigCache.getIns;
        HttpClient client = new HttpClient();
        DevTreeListInit devTree;

        public PostForm()
        {
            InitializeComponent();
            LoadUrl(txtPath);
            LoadLastRunInfo();
            LoadProtpcolFile(txtProtocolFilePath.Text);
            InitTreeList(treeList1);
            MemoEditTreelistColumn(treeList2, 0);
            InittextEditSearch();
        }

        #region init、非事件

        /// <summary>
        /// 1.添加协议源文件
        /// 2.用php脚本解析协议获得所有CS协议，并保存
        /// 3.将CS协议显示到ComboBoxEdit控件
        /// </summary>
        /// <param name="path"></param>
        private void LoadProtpcolFile(string path)
        {
            try
            {   
                if (!File.Exists(path))
                {
                    return;
                }
                //文件载入
                List<PostDataModel> result = JsonHelper.DeserializeJsonFileToType<List<PostDataModel>>(path);
                if (result != null)
                {
                    treeList1.DataSource = result;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 加载上次执行信息
        /// </summary>
        private void LoadLastRunInfo()
        {
            //提前加载数据
            string txtpath = postCfgCache.Get(txtPath.Name);
            if (txtpath.Length > 0)
            {
                txtPath.Text = txtpath;

            }
            string _txtProtocolFilePath = postCfgCache.Get(txtProtocolFilePath.Name);
            if (_txtProtocolFilePath.Length > 0)
            {
                txtProtocolFilePath.Text = _txtProtocolFilePath;
            }
            if (string.IsNullOrEmpty(txtProtocolFilePath.Text))
            {
                txtProtocolFilePath.Text = ConfigCache.GetIns.GetProtpcolFilePath();
            }

            string _txtParam = postCfgCache.Get(txtParam.Name);
            if (txtpath.Length > 0)
            {
                TxtParamAppend(_txtParam);
            }

            string _txtCheckButoonClear = postCfgCache.Get(checkButtonClear.Name);
            if (_txtCheckButoonClear.Length > 0)
            {
                checkButtonClear.Checked = bool.Parse(_txtCheckButoonClear);
            }


            string tmp = postCfgCache.Get(txtUids.Name);
            if (tmp.Length > 0) txtUids.Text = tmp;

            tmp = postCfgCache.Get(txtLoopCount.Name);
            if (tmp.Length > 0) txtLoopCount.Text = tmp;

        }

        /// <summary>
        /// 加载接口地址
        /// </summary>
        private void LoadUrl(ComboBoxEdit combo) 
        {
            combo.Properties.TextEditStyle = TextEditStyles.Standard;//用户可以在编辑框中直接输入文本
            combo.Properties.AutoComplete = true;//自动提供可能的匹配项
            combo.Properties.CaseSensitiveSearch = false;//搜索时不区分大小
            foreach (Testing item in PostUrlsCache.getIns.data)
            {
                if (combo.Text.Length <= 0)
                {
                    combo.Text = item.Url;
                }
                combo.Properties.Items.Add(item);
            }
            combo.TextChanged += (sender, e) =>
            {
                Testing ts = combo.SelectedItem as Testing;
                if (ts != null)
                {
                    combo.Text = ts.Url;
                }
            };
        }

        private void InitTreeList(TreeList tree)
        {
            devTree = new DevTreeListInit(tree);
            tree.ParentFieldName = "pid";
            tree.KeyFieldName = "id";
            tree.DoubleClick += TreeList1_DoubleClick;
            tree.OptionsFind.AlwaysVisible = true;//显示搜索
            tree.OptionsBehavior.ReadOnly = true;
            PopupMenuShowDev.TreeListDev(tree, new Dictionary<string, PopupMenuShowingEventHandler>()
            {
                {
                    "全部加入",(sender ,e)=>{
                        foreach (TreeListNode item2 in tree.Nodes)
                        {
                            if (item2.Visible&&item2.HasChildren)
                            {
                                foreach (TreeListNode item in item2.Nodes)
                                {
                                    if (item.Visible){
                                        string tmp = item.GetValue("Value").ToString();
                                        if (tmp != null)
                                        {
                                            TxtParamAppend(tmp);
                                        }
                                    }
                                }
                            }

                        }

                    }
                },
                {
                    "选择加入",(s,e)=>
                    {
                        foreach (TreeListNode item in devTree.treeListNodes)
                        {
                            if (item.Visible){
                                string tmp = item.GetValue("Value").ToString();
                                if (tmp != null)
                                {
                                    TxtParamAppend(tmp);
                                }
                            }
                        }
                    }
                },
                {
                    "取消所有选中", (se, e2) =>
                    {
                        treeList1.UncheckAll();
                        devTree.treeListNodes.Clear();
                    }
                },
                {
                    "清空协议", (se, e2) =>
                    {
                        txtParam.Text="";
                    }
                },

            });
            tree.ExpandAll();
        }

        /// <summary>
        /// TreeList 换行
        /// </summary>
        /// <param name="t"></param>
        /// <param name="index"></param>
        public static void MemoEditTreelistColumn(TreeList t, int index)
        {
            t.RowHeight = 10;
            //RepositoryItemRichTextEdit repositoryItemMemoEdit = new RepositoryItemRichTextEdit();
            RepositoryItemMemoEdit repositoryItemMemoEdit = new RepositoryItemMemoEdit();
            repositoryItemMemoEdit.AutoHeight = true;
            repositoryItemMemoEdit.WordWrap = true;
            t.Columns[index].ColumnEdit = repositoryItemMemoEdit;
        }
       
        /// <summary>
        /// todo:
        ///     1.获取请求地址和参数
        ///     2.设置并发任务
        ///     3.处理返回值
        /// </summary>
        private async void Run() 
        {
            if (!int.TryParse(txtLoopCount.Text.Trim(), out int concurrentRequests) || concurrentRequests <= 0 ||
         !int.TryParse(txtTimeSpace.Text.Trim(), out int timeSpace))
            {
                AppendToBoxResults("请输入有效的请求数量和时间间隔。");
                return;
            }

            string url = txtPath.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                AppendToBoxResults("请输入有效的请求 URL。");
                return;
            }
            //if (timeOut>0)//只能设置一次不然会报错
            //{
            //    client.Timeout = TimeSpan.FromSeconds(timeOut);
            //}

            string[] jsonData = Regex.Split(txtParam.Text.Trim(), "\r\n", RegexOptions.None);
            string[] param = Regex.Split(txtUids.Text.Trim(), "\r\n", RegexOptions.None);


            if (checkButtonClear.Checked)
            {
                memoEdit1.Text = "";
            }

            try
            {
                await RunConcurrentRequests(url, jsonData.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
                                             param.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
                                             concurrentRequests, timeSpace);
            }
            catch (Exception ex)
            {
                AppendToBoxResults($"运行出错: {ex}");
            }
        }

        private async Task RunConcurrentRequests(string url, string[] jsonData, string[] param, int concurrentRequests = 1, int timeSpace = 0)
        {
            List<Task> tasks = new List<Task>();
            int requestCounter = 0;
            jsonData = jsonData.Length > 0 ? jsonData : new[] { string.Empty };

            for (int i = 0; i < concurrentRequests; i++)
            {
                foreach (string uidLine in param.Length > 0 ? param : new[] { string.Empty })
                {
                    string[] paramArr = string.IsNullOrWhiteSpace(uidLine) ? Array.Empty<string>() : uidLine.Split(',');
                    string execUrl = paramArr.Length > 0 ? string.Format(url, paramArr) : url;
                    
                    foreach (string json in jsonData)
                    {
                        tasks.Add(MakeApiRequest(++requestCounter, execUrl, json));
                    }
                }

                if (timeSpace > 0)
                {
                    await Task.Delay(timeSpace);
                }
            }

            await Task.WhenAll(tasks);
        }

        private async Task MakeApiRequest(int requestNumber, string url, string jsonData)
        {
            try
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                // 设置请求头
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(url, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                //Json格式化
                object jsonObject = Common.getJsonObject(responseBody);
                if (jsonObject != null)
                {
                    responseBody = jsonObject.ToString();
                }

                string msg = $"Request {requestNumber} - Url: {url} - Param: {jsonData} - Status: {response.StatusCode}";
                //msg = msg.Replace("\r\n", Environment.NewLine);
                responseBody = responseBody.Replace("\n", Environment.NewLine);
                AppendToBoxResults($"{msg}\r\nResponse:\r\n{responseBody}\r\n");
                AddNode(msg, responseBody);
            }
            catch (Exception ex)
            {
                AppendToBoxResults($"Request {requestNumber} - Error: {ex}");
            }
        }

        private void AddNode(string parrent, string son)
        {
            TreeListNode node = treeList2.Nodes.Add(new object[] { parrent });
            node.Nodes.Add(new object[] { son });
        }


        private void AppendToBoxResults(string message)
        {
            message += "\r\n\r\n==========end==========\r\n\r\n";
            // 在文本框中显示结果
            if (memoEdit1.InvokeRequired)
            {
                memoEdit1.Invoke(new Action(() => AppendToBoxResults(message)));
            }
            else
            {
                memoEdit1.AppendText($"[{System.DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }

        private void TxtParamAppend(string message) 
        {
            txtParam.AppendText(message+Environment.NewLine);
        }
        #endregion


        private void TreeList1_DoubleClick(object sender, EventArgs e)
        {
            string tmp = treeList1.FocusedNode.GetValue("Value").ToString();
            if (tmp != null)
            {
                TxtParamAppend(tmp);
            }
        }
        private void btnExec_Click(object sender, EventArgs e)
        {
            treeList2.Nodes.Clear();
            Run();
            postCfgCache.Set(txtTimeSpace.Name, txtTimeSpace.Text.Trim());
            postCfgCache.Set(txtLoopCount.Name, txtLoopCount.Text.Trim());
            postCfgCache.Set(txtPath.Name, txtPath.Text.Trim());
            postCfgCache.Set(txtUids.Name, txtUids.Text.Trim());
            postCfgCache.Set(txtProtocolFilePath.Name, txtProtocolFilePath.Text.Trim());
            postCfgCache.Set(txtParam.Name, txtParam.Text.Trim());
            postCfgCache.Set(checkButtonClear.Name, checkButtonClear.Checked.ToString());
            postCfgCache.Save();
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            int index = navigationFrame1.SelectedPageIndex - 1;
            index = index >= 0 ? index : 1;
            navigationFrame1.SelectedPageIndex = index;
        }

        private void btnProtpcolFileOpen_Click(object sender, EventArgs e)
        {
            LoadProtpcolFile(txtProtocolFilePath.Text);
        }

        #region 搜索内容
        private void InittextEditSearch() 
        {
            textEditSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    FindLineByText();
                }
            };
            simpleButtonFindDown.Click += (s, e) => 
            {
                FindLineByText();
            };
            
        }
        private int _lastFoundIndex = -1;
        private void FindLineByText() 
        {
            string searchText = textEditSearch.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                XtraMessageBox.Show("请输入搜索内容");
                return;
            }
            // 从上次找到的位置后开始搜索
            int startIndex = _lastFoundIndex + 1;
            int foundIndex = memoEdit1.Text.IndexOf(searchText, startIndex, StringComparison.OrdinalIgnoreCase);

            if (foundIndex >= 0)
            {
                memoEdit1.Select(foundIndex,searchText.Length);
                memoEdit1.ScrollToCaret();
                memoEdit1.Focus();

                // 保存最后找到的位置
                _lastFoundIndex = foundIndex;
            }
            else {
                _lastFoundIndex = -1;
            }
        }
        #endregion

        private void PostForm_Load(object sender, EventArgs e)
        {

        }
    }
}
