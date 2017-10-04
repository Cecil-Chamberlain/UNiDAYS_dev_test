using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using UNiDAYS_dev_test.Models;
using UNiDAYS_dev_test.Controllers;
using UNiDAYS_dev_test.Services;
using Moq;

namespace UNiDAYS_dev_test.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private Mock<IAccountDbContext> _accountDbContext;
        private AccountController _controller;
        
        
        [TestInitialize]
        public void TestInitialize()
        {
            _accountDbContext = new Mock<IAccountDbContext>();
            _controller = new AccountController(_accountDbContext.Object);
            

        }

        [TestMethod]
        public void TestNewUserGetReturnsCorrectViewResult()
        {
            var result = _controller.NewUser() as ViewResult;
            Assert.AreEqual("NewUser", result.ViewName);
        }

        [TestMethod]
        public void TestNewUserPostReturnsCorrectRedirectToRouteResult()
        {
            var model = new NewUserViewModel();
            var result = _controller.NewUser(model) as RedirectToRouteResult;
            Assert.AreEqual("NewUser", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestNewUserPostInvalidModelStateReturnsCorrectViewResult()
        {
            var model = new NewUserViewModel();
            _controller.ModelState.AddModelError("key", "error");
            var result = _controller.NewUser(model) as ViewResult;
            Assert.AreEqual("NewUser", result.ViewName);
        }

        [TestMethod]
        public void TestNewUserGetReturnsViewResultType()
        {
            var result = _controller.NewUser() as ViewResult;
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestNewUserPostReturnsRedirectToRouteResultType()
        {
            var model = new NewUserViewModel();
            var result = _controller.NewUser(model) as RedirectToRouteResult;
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestNewUserPostInvalidModelStateReturnsViewResultType()
        {
            var model = new NewUserViewModel();
            _controller.ModelState.AddModelError("key", "error");
            var result = _controller.NewUser(model) as ViewResult;
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }
    }
}
