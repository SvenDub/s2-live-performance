using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Live_Performance.Entity;
using Live_Performance.Models;
using Live_Performance.Persistence.Exception;
using Util;

namespace Live_Performance.Controllers
{
    public class LoginController : EntityController<User>
    {
        /// <summary>
        ///     Show login form.
        /// </summary>
        /// <param name="returnUrl">The url to direct to after logging in.</param>
        /// <param name="failed">Whether the last attempt failed.</param>
        public ActionResult Index(string returnUrl, bool failed = false)
        {
            if (Session[SessionVars.User] == null)
            {
                ViewBag.RedirectUrl = returnUrl;
                ViewBag.Failed = failed;
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
        }

        /// <summary>
        ///     Try to login. Redirect to login form if failed.
        /// </summary>
        /// <param name="model">The entered credentials.</param>
        /// <param name="returnUrl">The url to direct to after logging in.</param>
        [HttpPost]
        public ActionResult Index(User model, string returnUrl)
        {
            if (Session[SessionVars.User] == null)
            {
                List<User> users =
                    Repository.FindAllWhere(user => user.Email == model.Email && user.Password == model.Password);
                if (users.Count == 1)
                {
                    User loggedIn = users.First();

                    Session[SessionVars.User] = loggedIn;

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", new { returnUrl = returnUrl, failed = true });
                }
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        /// <summary>
        ///     Log out.
        /// </summary>
        public ActionResult Logout()
        {
            Session.Remove(SessionVars.User);

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Show signup form.
        /// </summary>
        /// <param name="returnUrl">The url to direct to after logging in.</param>
        /// <param name="failed">Whether the last attempt failed.</param>
        public ActionResult Signup(string returnUrl, bool failed = false)
        {
            if (Session[SessionVars.User] == null)
            {
                ViewBag.RedirectUrl = returnUrl;
                ViewBag.Failed = failed;
                return View();
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(returnUrl);
            }
        }

        /// <summary>
        ///     Signup.
        /// </summary>
        /// <param name="model">The entered credentials.</param>
        /// <param name="returnUrl">The url to direct to after logging in.</param>
        [HttpPost]
        public ActionResult Signup(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Make sure the user can not create an admin account or override an existing one.
                model.Admin = false;
                model.Id = 0;

                try
                {
                    Repository.Save(model);
                }
                catch (DataSourceException ex)
                {
                    Log.E("SIGNUP", ex.ToString());
                    return RedirectToAction("Signup", new { returnUrl = returnUrl, failed = true });
                }

                List<User> users =
                    Repository.FindAllWhere(user => user.Email == model.Email && user.Password == model.Password);
                if (users.Count == 1)
                {
                    User loggedIn = users.First();

                    Session[SessionVars.User] = loggedIn;

                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", new { returnUrl = returnUrl, failed = true });
                }
            }
            else
            {
                // Make sure the user can not create an admin account or override an existing one.
                model.Admin = false;
                model.Id = 0;
                ViewBag.Failed = true;

                return View(model);
            }
        }

        /// <summary>
        ///     Show a message that the user is unauthorized.
        /// </summary>
        /// <param name="returnUrl">The url that was not allowed.</param>
        public ActionResult Unauthorized(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
    }
}