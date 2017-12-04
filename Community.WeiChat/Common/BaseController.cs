using Community.Common;
using System.Web.Mvc;

namespace Community.WeiChat.Common
{
    public class BaseController : Controller
    {

        protected override void OnException(ExceptionContext filterContext)
        {
            //写日志
            LogHelper.Output(Request.Url.LocalPath, ErrorLevel.ERROR, filterContext.Exception);

            //filterContext.ExceptionHandled = true;
            ////重定向跳转页面
            //filterContext.Result = new System.Web.Mvc.RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Shared" }));
        }

        #region 取得登录用户信息

        /// <summary>
        /// 取得登录用户信息
        /// </summary>
        /// <returns></returns>
        protected string LoginUserInfo()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        #endregion 取得登录用户信息

    }
}