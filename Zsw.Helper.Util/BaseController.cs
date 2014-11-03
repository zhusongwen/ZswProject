using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.Security.Principal;

namespace Zsw.Common.Util
{
    public class BaseController : Controller
    {
        private IPrincipal user;

        public new IPrincipal User
        {
            get
            {
                if (user == null)
                {
                    return new HttpPrincipal();
                }
                else
                {
                    return user;
                }
            }
        }

        /// <summary>
        /// 授权验证
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                base.OnAuthorization(filterContext);
                return;
            }
            string area = "";
            if (filterContext.RouteData.DataTokens["area"] != null)
            {
                area = filterContext.RouteData.DataTokens["area"].ToString().ToLower();
            }

            string controller = filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            string action = filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower();

            var permitCode = string.Join("/", area, controller, action).ToLower();

            HttpContext.User = this.User;

            if (FormsAuthenticationService.LoginUrl.Trim('~').ToLower().StartsWith(permitCode))
            {
                base.OnAuthorization(filterContext);
            }
            else if (HttpContext.User.Identity.IsAuthenticated)
            {
                //if (UserSession.Current.CheckPagePermit(permitCode))
                //{
                base.OnAuthorization(filterContext);
                //}
            }
            else
            {
                var loginUrl = string.Format("{0}?redirectUrl={1}", FormsAuthenticationService.LoginUrl, HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
                base.Response.Redirect(loginUrl, true);
            }
        }
        /// <summary>
        /// 系统异常
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var requestUrl = "";
                if (filterContext.RequestContext.HttpContext.Request.Url != null)
                    requestUrl = filterContext.RequestContext.HttpContext.Request.Url.ToString();
                var exception =
                    new Exception(string.Format("请求URL:{0} | 异常说明: {1}\r\nStackTrace:{2}\r\n", requestUrl,
                                                filterContext.Exception.Message, filterContext.Exception.StackTrace));
            }
            catch (Exception)
            {

            }
            base.OnException(filterContext);
        }
        /// <summary>
        /// 在调用操作方法之前调用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}
