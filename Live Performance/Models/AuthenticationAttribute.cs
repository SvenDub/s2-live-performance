using System.Web.Mvc;
using System.Web.Routing;
using Inject;
using Live_Performance.Entity;
using Live_Performance.Persistence;

namespace Live_Performance.Models
{
    /// <summary>
    ///     Require authorization on the attributed method or controller.
    /// </summary>
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        private readonly IRepository<User> _userRepository = Injector.Resolve<IRepository<User>>();

        /// <summary>
        ///     Check if the user is logged in and redirect to the appropriate page.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session?[SessionVars.User] == null)
            {
                RedirectToLogin(filterContext);
            }
            else
            {
                User user = filterContext.HttpContext.Session[SessionVars.User] as User;
                if (user != null)
                {
                    // Get latest user info from db
                    User dbUser = _userRepository.FindOne(user.Id);
                    filterContext.HttpContext.Session[SessionVars.User] = dbUser;

                    if (Admin && !dbUser.Admin)
                    {
                        RedirectToUnauthorized(filterContext);
                    }
                }
                else
                {
                    filterContext.HttpContext.Session.Remove(SessionVars.User);
                }
            }
        }

        /// <summary>
        ///     Redirect to the login page.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private static void RedirectToLogin(ActionExecutingContext filterContext)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Login",
                            action = "Index",
                            returnUrl = filterContext.HttpContext.Request.RawUrl
                        }));
        }

        /// <summary>
        ///     Redirect to the unauthorized page.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private static void RedirectToUnauthorized(ActionExecutingContext filterContext)
        {
            filterContext.Result =
                new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Login",
                            action = "Unauthorized",
                            returnUrl = filterContext.HttpContext.Request.RawUrl
                        }));
        }

        /// <summary>
        ///     Whether to require admin credentials.
        /// </summary>
        public bool Admin { get; set; } = false;
    }
}