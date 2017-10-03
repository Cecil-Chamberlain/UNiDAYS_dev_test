using System;
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

        private AccountController _controller;
        private Mock<IAccountDbContext> _accountDbContext;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _accountDbContext = new Mock<IAccountDbContext>();
            _controller = new AccountController(_accountDbContext.Object);

        }

        [TestMethod]
        public void TestNewUserGet()
        {
            var result = _controller.NewUser() as ViewResult;
            Assert.AreEqual("NewUser", result.ViewName);
        }

        [TestMethod]
        public void TestNewUserPost()
        {
            var model = new NewUserViewModel();

            //_accountDbContext.Setup(m => m.CreateNewUser("test@domain.com", "password")).Returns("");
            
            var result = _controller.NewUser(model) as ViewResult;
            //Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual(model, result.ViewData.Model);
        }
    }
}
