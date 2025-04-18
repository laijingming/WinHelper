﻿using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AJLibrary
{
    public class Common
    {
        /// <summary>
        /// 根目录地址
        /// </summary>
        public static string ROOTPATH = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 加载浮窗
        /// </summary>
        /// <param name="start"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="millisecond"></param>
        public static void LoadForm(string title = "正在加载", string description = "请等待...", int millisecond = 0)
        {
            SplashScreenManager.ShowDefaultWaitForm(title, description);
            if (millisecond > 0)
            {
                System.Threading.Thread.Sleep(millisecond);
            }
        }

        /// <summary>
        /// 加载浮窗
        /// </summary>
        /// <param name="start"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="millisecond"></param>
        public static void CloseLoadForm()
        {
            SplashScreenManager.CloseForm();
        }

        /// <summary>
        /// 获得已激活form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static XtraForm GetActivedForm<T>() where T : XtraForm
        {
            XtraForm frm= Application.OpenForms.OfType<T>().FirstOrDefault();
            if (frm == null)
            {
                frm = Activator.CreateInstance<T>();// 实例化窗体
            }
            return frm;
        }

        public static JObject getJsonObject(string input)
        {
            try
            {
                return JObject.Parse(input);
            }
            catch
            {
                return null;
            }
        }
    }
}
