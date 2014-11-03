using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zsw.Common.Util
{
    [Serializable]
    public class UserSession
    {
        const string USER_SESSION_KEY = "ZSWAUTH";

        const int ANONYMOUS_USER_ID = 0;

        const string ANONYMOUS_USER_CODE = "anonymous";

        const string ANONYMOUS_USER_NAME = "未登录用户";

        public UserSession()
        {
            this.UserID = ANONYMOUS_USER_ID;
            this.UserCode = ANONYMOUS_USER_CODE;
            this.UserName = ANONYMOUS_USER_NAME;
            this.Roles = new List<string>();
        }
        /// <summary>
        /// 返回当前会话信息
        /// </summary>
        public static UserSession Current
        {
            get
            {
                UserSession value = null;
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[USER_SESSION_KEY] == null)
                {
                    value = new UserSession();
                    HttpContext.Current.Session[USER_SESSION_KEY] = value;
                }
                else
                {
                    value = HttpContext.Current.Session[USER_SESSION_KEY] as UserSession;
                }
                return value;
            }
        }
        /// <summary>
        /// 验证当前用户是否已经登录
        /// </summary>
        public bool IsValid
        {
            get
            {
                return UserID != ANONYMOUS_USER_ID;
            }
        }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int UserID { get; private set; }

        /// <summary>
        /// 当前用户名（登陆名）
        /// </summary>
        public string UserCode { get; private set; }

        /// <summary>
        /// 当前用户显示名称（昵称）
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 当前用户拥有的角色
        /// </summary>
        public List<string> Roles { get; private set; }

        /// <summary>
        /// 检查用户是否具有页面访问权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckPagePermit(string path)
        {
            return true;
        }
        /// <summary>
        /// 同步用户信息
        /// </summary>
        /// <returns></returns>
        public bool SyncUserData(string userCode)
        {
            this.UserID = 1;
            this.UserCode = userCode;
            this.UserName = "测试用户";

            HttpContext.Current.Session[USER_SESSION_KEY] = UserSession.Current;
            return true;
        }
    }
}
