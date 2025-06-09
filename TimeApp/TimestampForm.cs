using System;
using System.Drawing;
using System.Windows.Forms;
using AJLibrary;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace TimeApp
{
    public partial class TimestampForm : DevExpress.XtraEditors.XtraForm
    {
        public TimestampForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 每秒一次获取当前时间戳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            labelControl_timestamp.Text = Time.Now().ToString();
        }

        private void Timestamp_Load(object sender, EventArgs e)
        {
            labelControl_timestamp.Text = Time.Now().ToString();
            textEdit_timestamp.Text = Time.Now().ToString();
            textEdit_time.Text = Time.NowDate().ToString();
            comboBoxEdit_timestapm.SelectedIndex = 0;
            comboBoxEdit_time.SelectedIndex = 0;

            // 设置标签样式
            labelControl_timestamp.AppearanceHovered.Font = new Font("宋体", 12F);
            labelControl_timestamp.AppearanceHovered.ForeColor = Color.DodgerBlue;
            //labelControl_timestamp.AppearanceHovered.TextOptions.HAlignment = HorzAlignment.Center;
            //labelControl_timestamp.AppearanceHovered.TextOptions.VAlignment = VertAlignment.Center;
        }

        private void simpleButton_timestamp_Click(object sender, EventArgs e)
        {
            try
            {
                textEdit_to_time.Text = Time.UnixTimeStampToDateTime(long.Parse(textEdit_timestamp.Text), comboBoxEdit_timestapm.SelectedIndex == 0).ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }

        }

        private void simpleButton_time_Click(object sender, EventArgs e)
        {
            try
            { 
                textEdit_to_timestamp.Text = Time.DateTimeToUnixTimeStamp(textEdit_time.Text, comboBoxEdit_time.SelectedIndex == 0);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
                throw;
            }
            
        }

        private void simpleButton_control_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            if (timer1.Enabled)
            {
                simpleButton_stop.Visible = true;
                simpleButton_start.Visible = false;
            }
            else
            {
                simpleButton_start.Visible = true;
                simpleButton_stop.Visible = false;
            }
        }

        private void labelControl_timestamp_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(labelControl_timestamp.Text);
        }
    }
}
