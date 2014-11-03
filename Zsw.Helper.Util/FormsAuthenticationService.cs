using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Zsw.Common.Util
{
    /// <summary>
    /// 表单认证帮助类
    /// </summary>
    public class FormsAuthenticationService
    {
        /// <summary>
        /// 登陆页地址
        /// </summary>
        public static string LoginUrl { get; private set; }

        /// <summary>
        /// 登出页地址
        /// </summary>
        public static string LogoutUrl = "~/Account/Logout";

        /// <summary>
        /// Cookie生效的域名
        /// </summary>
        public static string CookieDomain { get; private set; }

        /// <summary>
        /// 认证的Cookie的名称
        /// </summary>
        public static string CookieName { get; private set; }

        /// <summary>
        /// CookiePath
        /// </summary>
        public static string CookiePath { get; private set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public static bool IsEnabled { get; private set; }

        /// <summary>
        /// Cookie过期时长
        /// </summary>
        public static TimeSpan Timeout { get; private set; }

        /// <summary>
        /// Cookie加密的后缀
        /// </summary>
        private static byte[] appKey;

        /// <summary>
        /// 网站的默认页
        /// </summary>
        public static string DefaultUrl { get; private set; }

        static FormsAuthenticationService()
        {
            var _Config = WebConfigurationManager.OpenWebConfiguration("~");
            SystemWebSectionGroup systemWeb = (SystemWebSectionGroup)_Config.GetSectionGroup("system.web");
            IsEnabled = (systemWeb.Authentication.Mode == AuthenticationMode.Forms);
            if (!IsEnabled)
            {
                return;
            }
            if (!_Config.AppSettings.Settings.AllKeys.Contains("AuthKey"))
            {
                appKey = Guid.NewGuid().ToByteArray();
                _Config.AppSettings.Settings.Add("AuthKey", Convert.ToBase64String(appKey));
            }
            else
            {
                appKey = Convert.FromBase64String(_Config.AppSettings.Settings["AuthKey"].Value);
            }
            CookieDomain = systemWeb.Authentication.Forms.Domain;
            CookieName = systemWeb.Authentication.Forms.Name + "Token";
            CookiePath = systemWeb.Authentication.Forms.Path;
            LoginUrl = systemWeb.Authentication.Forms.LoginUrl;
            DefaultUrl = systemWeb.Authentication.Forms.DefaultUrl;
            Timeout = systemWeb.Authentication.Forms.Timeout;
        }
        /// <summary>
        /// 用户登录创建cookie认证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="createPersistentCookie"></param>
        public static void LoginIn(string userName, bool createPersistentCookie)
        {
            HttpCookie cookie = new HttpCookie(CookieName);
            cookie.Value = CreateAuthToken(userName, Timeout);
            cookie.Domain = CookieDomain;
            cookie.Expires = DateTime.Now.Add(Timeout);
            cookie.Path = "/";
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        /// <summary>
        /// 推出登录将cookie设为失效
        /// </summary>
        public static void LoginOut()
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(CookieName))
            {
                var cookie = HttpContext.Current.Request.Cookies[CookieName];
                cookie.Expires = new DateTime(1900, 01, 01);
                cookie.Path = "/";
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        /// <summary>
        /// 验证cookie有效性
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool VerifyAuthToken(out string userName)
        {
            userName = null;
            byte[] data = null;
            if (!HttpContext.Current.Request.Cookies.AllKeys.Contains(CookieName))
            {
                return false;
            }
            try
            {
                CookieToken token = null;
                var cookieValue = HttpContext.Current.Request.Cookies[CookieName].Value;
                data = HttpServerUtility.UrlTokenDecode(cookieValue);
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream(data))
                {
                    token = (CookieToken)formatter.Deserialize(stream);
                }
                if (token.Signature.Length != 20)
                {
                    return false;
                }
                if (token.ExpiredDate < DateTime.Now)
                {
                    return false;
                }
                if (token.UserName == null)
                {
                    return false;
                }
                data = Encoding.UTF8.GetBytes(token.UserName).Concat(BitConverter.GetBytes(token.ExpiredDate.ToBinary())).Concat(appKey).ToArray();
                using (SHA1 sha1 = SHA1.Create())
                {
                    data = sha1.ComputeHash(data);
                }
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != token.Signature[i])
                    {
                        return false;
                    }
                }
                userName = token.UserName;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 创建cookie认证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="timeSpan"></param>
        public static string CreateAuthToken(string userName, TimeSpan timeSpan)
        {
            CookieToken token = new CookieToken();
            token.UserName = userName;
            token.ExpiredDate = DateTime.Now.Add(timeSpan);
            byte[] data = Encoding.UTF8.GetBytes(token.UserName).Concat(BitConverter.GetBytes(token.ExpiredDate.ToBinary())).Concat(appKey).ToArray();
            using (SHA1 sha1 = SHA1.Create())
            {
                token.Signature = sha1.ComputeHash(data);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, token);
                data = stream.ToArray();
            }
            return HttpServerUtility.UrlTokenEncode(data);
        }
        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return MD5Handler.EnMD5(text);
        }
    }
    [Serializable]
    class CookieToken
    {
        public string UserName { get; set; }

        public DateTime ExpiredDate { get; set; }

        public byte[] Signature { get; set; }
    }
}
