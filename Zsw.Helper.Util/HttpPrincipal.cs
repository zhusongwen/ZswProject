using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;


namespace Zsw.Common.Util
{
    public class HttpPrincipal : IPrincipal
    {
        private IIdentity identity = null;

        public IIdentity Identity
        {
            get
            {
                if (identity == null)
                {
                    identity = new HttpIdentity();
                }
                return identity;
            }
        }
        /// <summary>
        /// 确认当前用户是否属于当前脚色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return false;
        }
    }
}
