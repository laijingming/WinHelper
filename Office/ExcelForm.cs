using AJLibrary;
using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Office
{
    public partial class ExcelForm : DevExpress.XtraEditors.XtraForm
    {

        public ExcelForm()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        /// <summary>
        /// 加载excel
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadDocument(string filePath="")
        {
            if (File.Exists(filePath))
            {
                this.Text = Path.GetFileName(filePath);
                IWorkbook workbook = spreadsheetControl1.Document;
                workbook.LoadDocument(filePath);
            }
        }
    }
}