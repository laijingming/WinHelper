using AJLibrary;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Customization;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Office
{
    public partial class OfficeForm : DevExpress.XtraEditors.XtraForm
    {
        public OfficeForm()
        {
            InitializeComponent();
            // 隐藏标题栏和边框
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            Init();
        }

        private void Init()
        {
            LoadAccordionControlElement(OfficeCache.getIns.data);

        }

        /// <summary>
        /// 递归加载AccordionControl元素
        /// </summary>
        /// <param name="data">需要加载的节点列表</param>
        /// <param name="parentElement">父级AccordionControl元素</param>
        private void LoadAccordionControlElement(List<OfficCacheModel> data, AccordionControlElement parentElement = null)
        {
            foreach (var item in data)
            {
                if (Directory.Exists(item.path)) // 存在子节点
                {
                    // 创建组元素
                    AccordionControlElement groupElement = accordionControl1.AddGroup();
                    groupElement.Text = Path.GetFileName(item.name);
                    string[] files = Directory.GetFiles(item.path);
                    List<OfficCacheModel> _data = new List<OfficCacheModel>();
                    foreach (string _item in files)
                    {
                        _data.Add(new OfficCacheModel() { path= _item });
                    }
                    // 递归加载子节点
                    LoadAccordionControlElement(_data, groupElement);
                }
                else
                {
                    // 创建条目元素
                    AccordionControlElement itemElement = new AccordionControlElement(ElementStyle.Item);
                    itemElement.Text = Path.GetFileName(item.path);
                    // 添加到父级或主控件
                    if (parentElement != null)
                    {
                        parentElement.Elements.Add(itemElement);
                    }
                    else
                    {
                        accordionControl1.Elements.Add(itemElement);
                    }

                    // 为元素添加点击事件
                    itemElement.Click += (s, e) =>
                    {
                        ShowFormInContainer(item);
                    };
                }
            }
        }
        XtraForm frm = null;
        private void ShowFormInContainer(OfficCacheModel data)
        {
            try
            {   
                Common.LoadForm();
                switch (Path.GetExtension(data.path))
                {
                    case ".pdf":
                        break;
                    case ".xls":
                    case ".xlsx":
                    case ".cvs":
                        frm = Common.GetActivedForm<ExcelForm>() as ExcelForm;
                        ((ExcelForm)frm).LoadDocument(data.path);
                        break;
                    default:
                        break;
                }
                // 设置窗体属性
                if (frm != null)
                {
                    frm.TopLevel = false;
                    frm.FormBorderStyle = FormBorderStyle.None;
                    fluentDesignFormContainer1.Controls.Add(frm);
                    frm.Dock = DockStyle.Fill;
                    frm.Visible = true;
                    frm.BringToFront(); // 将窗体移动到 fluentDesignFormContainer1 中的最顶层
                }
                // 显示窗体
                if (frm != null)
                {
                    frm.Show();
                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show("加载窗体时发生错误: " + ex.Message);
            }
            finally
            {
                Common.CloseLoadForm();

            }
        }
    }


}