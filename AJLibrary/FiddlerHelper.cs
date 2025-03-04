using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AJLibrary
{
    public class FiddlerHelper
    {
        public string key;
        public string cert;
        public BeforeResponseHandle BeforeRequestFun;
        public delegate void BeforeResponseHandle(RequestInfo info);

        public static FiddlerHelper getIns => Master.getModel<FiddlerHelper>();

        public FiddlerHelper() 
        {   
            key = ConfigCache.getIns.Get("fiddler.certmaker.bc.key");
            cert = ConfigCache.getIns.Get("fiddler.certmaker.bc.cert");
            Install();
        }

        public void Install()
        {   
            //设置证书配置（确保HTTPS解密正常）
            if (!string.IsNullOrEmpty(key))
            {
                FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.key", key);
                FiddlerApplication.Prefs.SetStringPref("fiddler.certmaker.bc.cert", cert);
            }
            // 检查是否已存在根证书
            if (!CertMaker.rootCertExists())
            {
                // 创建根证书
                CertMaker.createRootCert();
                // 信任根证书
                CertMaker.trustRootCert();

                // 再次检测根证书是否成功安装
                if (!CertMaker.rootCertExists())
                {
                    // 如果依然不存在，提示用户手动安装
                    MessageBox.Show("自动安装根证书失败，请手动导入 FiddlerRoot.cer 到“受信任的根证书颁发机构”。",
                        "证书安装失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // 保存证书配置到缓存中，便于下次使用
                    ConfigCache.getIns.Set("fiddler.certmaker.bc.cert",
                        FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.cert", null));
                    ConfigCache.getIns.Set("fiddler.certmaker.bc.key",
                        FiddlerApplication.Prefs.GetStringPref("fiddler.certmaker.bc.key", null));
                    ConfigCache.getIns.SaveCacheToFile();
                }
            }
        }

        public bool Rremove()
        {
            // 检查是否存在根证书
            if (CertMaker.rootCertExists())
            {
                // 移除由 Fiddler 生成的证书
                if (CertMaker.removeFiddlerGeneratedCerts())
                {
                    ConfigCache.getIns.Set("fiddler.certmaker.bc.cert", null);
                    ConfigCache.getIns.Set("fiddler.certmaker.bc.key", null);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 启动 Fiddler
        /// </summary>
        /// <param name="port">端口</param>
        public void StartFiddler(int port=8888)
        {
            // 检查是否 Fiddler 代理已经启动
            if (!FiddlerApplication.IsStarted())
            {
                // 检查证书是否有效
                if (!CertMaker.rootCertExists() || !CertMaker.rootCertIsTrusted())
                {
                    // 重新安装证书
                    Install();
                }

                // 添加一个事件处理程序，以在发出请求前触发
                FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
                // 启动 Fiddler 代理，监听端口 8888
                FiddlerApplication.Startup(port, true, true, true);
                // 注释：参数分别表示端口号、允许远程连接、允许缓存、允许代理
            }
        }

        public void StopFiddler()
        {
            if (FiddlerApplication.IsStarted())
            {
                FiddlerApplication.Shutdown();
            }
        }
        
        public void CloseFiddler() 
        {
            StopFiddler();
            if (FiddlerApplication.oProxy != null && FiddlerApplication.oProxy.IsAttached)
            {
                FiddlerApplication.oProxy.Detach();
            }
        }

        private void FiddlerApplication_BeforeRequest(Session oSession)
        {
            //if (oSession.RequestMethod == "POST" || oSession.RequestMethod == "GET")
            if (true)
            {
                //Console.WriteLine(oSession.fullUrl);
                // 调用回调函数
                BeforeRequestFun?.Invoke(new RequestInfo()
                {
                    url = oSession.fullUrl,
                    header = oSession.RequestHeaders.ToString(),
                    body = oSession.GetRequestBodyAsString()
                });
            }
        }

        public void Save() 
        {
            CloseFiddler();
        }
    }
}

public class RequestInfo 
{
    public string url { get; set; }
    public string header { get; set; }
    public string body { get; set; }
}
