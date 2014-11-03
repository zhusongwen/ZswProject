using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zsw.Common.Util
{
    public class UserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>        
        public int ID { get; set; }

        /// <summary>
        /// 用户Code（登录名）
        /// </summary>        
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>        
        public string UserName { get; set; }

        /// <summary>
        /// 用户所属组织ID
        /// </summary>        
        public int OrgId { get; set; }

        /// <summary>
        /// 用户所属组织名称
        /// </summary>        
        public string OrgName { get; set; }

        /// <summary>
        /// 用户所有的角色编码列表
        /// </summary>
        public List<string> RoleCodes { get; set; }
    }
}
