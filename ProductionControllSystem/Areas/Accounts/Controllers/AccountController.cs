
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FrontEnd.Areas.Accounts.Models;
using System.Configuration;
using ClassLibrary.Core;
using ClassLibrary.Models;

namespace FrontEnd.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        public Database db{ get; set; }
        public AccountController()
        {
            db = new Database();
            ViewBag.isAuthenticated = isAuthenticated();
        }
        public ActionResult Index()
        {
            if (isAuthenticated())
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "Accounts" });
            }
        }
        public bool isAuthenticated()
        {
            return (Session != null) && (Session["UserId"]!=null);
        }
        public ActionResult ManageMyAccount()
        {
            //string userId = User.Identity.GetUserId();
            //ApplicationUser user = db.Users.Find(userId);
            //userId = user.Id;
            //Account account = db.Accounts.Find(userId);
            //ManageAccountViewModel model = new ManageAccountViewModel { Id = account.Id, Email = account.Email, FirstName = account.FirstName, LastName = account.LastName, PhoneNumber2 = account.PhoneNumber2, Designation = account.Designation, Address = account.Address, Status = account.Status };

            //ViewBag.id = userId;
            return View();
        }
        /*
         * @purpose - A logged in user can change his own password
         */
        public ActionResult ChangePassword()
        {

            return View();
        }
        /*
         * @purpose - SHow view to Create a new account with staff priviledges
         */
        public ActionResult AddAccount() {
            //AddAccountViewModel model = new AddAccountViewModel();
            //model.Id = db.Roles;
            return View();
        }
        /*
         * @purpose -  View all account information
         */
        public ActionResult AllAccounts() {
            return View();
        }
        // GET : a view to enter reg information
        [AllowAnonymous]
        public ActionResult Register()
        {
            //AddAccountViewModel model = new AddAccountViewModel();
            
            //// all roles except administrator
            //IEnumerable<IdentityRole> roles = db.Roles.Where(r => r.Name != "Administrator");

            //model.Id = roles;
            return View();
        }
        /*
         *@purpose - User can view all other staff accounts and edit them 
         */
        public ActionResult ManageAccounts() {
            return View();
        }
        // show the login view
        [HttpGet]
        public ActionResult LogIn(string incorrectLogin)
        {
            if (Session["UserId"]!=null)
            {
                return RedirectToAction("Dashboard");
            }
            if(incorrectLogin != null)
            {
                ViewBag.incorrectLogin = incorrectLogin;
            }
            LoginViewModel model = new LoginViewModel();
            model.Email = "";
            model.Password = "";
            model.LoginDate = DateTime.Today.ToString("yyyy-MM-dd");
            return View(model);
        }
        public ActionResult Dashboard()
        {
            DashboardViewModel model = new DashboardViewModel();
            model.employees = db.GetAll<Employee>();
            if (!isAuthenticated())
            {
                return RedirectToAction("Login");
            }
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            Session["UserId"] = "";
            Session["UserLevel"] = "";
            Session["LocationId"] = "";
            Session["UserFullName"] = "";
            Session["LoginDate"] = DateTime.Today.ToString("yyyy-M-dd");

            return RedirectToAction("Dashboard");
        }
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account", new { area = "Accounts" });
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        
        public ActionResult AutoLogin(string userId)
        {
            Session["UserId"] = "";
                Session["UserLevel"] = "";
            Session["LocationId"] = "";
            Session["UserFullName"] = "";
            Session["LoginDate"] = DateTime.Today.ToString("yyyy-M-dd");
                return RedirectToAction("Dashboard");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Login", "Account", new { area = "Accounts" });
        }
        #endregion
    }
}