using AJLibrary;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
namespace Post
{
    public partial class PostForm : DevExpress.XtraEditors.XtraForm
    {
        public PostForm()
        {
            InitializeComponent();
            LoadUrl(txtPath);
            loadLastRunInfo();
            LoadProtpcolFile(txtProtocolFilePath.Text);
            InitTreeList(treeList1);
        }

        #region init

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
        private void loadLastRunInfo()
        {
            ConfigCache cache1Model = ConfigCache.GetIns;
            //提前加载数据
            string txtpath = cache1Model.Get(txtPath.Name);
            if (txtpath.Length > 0)
            {
                txtPath.Text = txtpath;
            }
            string _txtProtocolFilePath = cache1Model.Get(txtProtocolFilePath.Name);
            if (_txtProtocolFilePath.Length > 0)
            {
                txtProtocolFilePath.Text = _txtProtocolFilePath;
            }
            if (string.IsNullOrEmpty(txtProtocolFilePath.Text))
            {
                txtProtocolFilePath.Text = ConfigCache.GetIns.GetProtpcolFilePath();
            }

            string _txtParam = cache1Model.Get(txtParam.Name);
            if (txtpath.Length > 0)
            {
                txtParam.Text = _txtParam;
            }

            string _txtCheckEdit1 = cache1Model.Get(checkEdit1.Name);
            if (txtpath.Length > 0)
            {
                checkEdit1.Checked = bool.Parse(_txtCheckEdit1);
            }
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
                                            txtParam.AppendText("\r\n"+tmp);
                                        }
                                    }
                                }
                            }

                        }
                    }
                },
            });
            tree.ExpandAll();
        }
        private void TreeList1_DoubleClick(object sender, EventArgs e)
        {
            string tmp = treeList1.FocusedNode.GetValue("Value").ToString();
            if (tmp != null)
            {
                txtParam.AppendText("\r\n" + tmp);
            }
        }
        #endregion
    }
}
