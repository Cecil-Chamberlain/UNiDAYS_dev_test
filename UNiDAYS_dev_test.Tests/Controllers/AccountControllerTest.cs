using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using UNiDAYS_dev_test.Controllers;

namespace UNiDAYS_dev_test.Tests
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void TestNewUser()
        {
            var controller = new AccountController();
            var result = controller.NewUser() as ViewResult;
            Assert.AreEqual("NewUser", result.ViewName);
        }
    }
}
