using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Zsw.Common.Util
{
    public class HttpIdentity : IIdentity
    {

        public string AuthenticationType
        {
            get { return "ZSWAUTH"; }
        }

        public bool? _IsAuthenticated = null;

        public bool IsAuthenticated
        {
            get
            {
                if (_IsAuthenticated.HasValue)
                {
                    return _IsAuthenticated.Value;
                }
                else
                {
                    if (UserSession.Current.IsValid)
                    {
                        _IsAuthenticated = true;
                    }
                    else
                    {
                        string userCode = null;
                        var tokenValid = FormsAuthenticationService.VerifyAuthToken(out userCode);
                        if (tokenValid && !string.IsNullOrEmpty(userCode))
                        {
                            _IsAuthenticated = UserSession.Current.SyncUserData(userCode);
                        }
                        else
                        {
                            _IsAuthenticated = false;
                        }
                    }
                    return _IsAuthenticated.Value;
                }
            }
        }

        /// <summary>
        /// 当请登录用户名
        /// </summary>
        public string Name
        {
            get { return UserSession.Current.UserCode; }
        }
    }
}
