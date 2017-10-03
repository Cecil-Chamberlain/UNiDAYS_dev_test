using System;
using System.Text;
using System.Web.Mvc;
using UNiDAYS_dev_test.Models;
using UNiDAYS_dev_test.Services;

namespace UNiDAYS_dev_test.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountDbContext _accountDbContext;
        /*
        public AccountController()
            :this(new AccountDbContext())
        {
        }*/

        public AccountController(IAccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Account/NewUser
        [AllowAnonymous]
        public ActionResult NewUser()
        {
            return View("NewUser");
        }

        //
        // POST: /Account/NewUser
        [HttpPost]
        [RequireHttps]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser([Bind(Include= "Email,Password,ConfirmPassword")]NewUserViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                string ret = _accountDbContext.CreateNewUser(model.Email, model.Password);
                TempData.Add("Notification", ret);
                return RedirectToAction("NewUser");
            }
            else
            {
                return View("NewUser");
            }

        }




        #region Helpers

        #endregion
    }
}