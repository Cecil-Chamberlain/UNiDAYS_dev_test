using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using UNiDAYS_dev_test.Services;

namespace UNiDAYS_dev_test.Tests.Services
{
    [TestClass]
    public class AccountDbContextTest
    {
        AccountDbContext _accountDbContext;
        
        public AccountDbContextTest()
        {
            _accountDbContext = new AccountDbContext();
        }
        
        [TestMethod]
        public void CrudTest()
        {
            string email = Create();
            GetByEmail(email);
            //Update(email);
            Delete(email);
        }

        [TestMethod]
        public void SaltMethodReturnsString()
        {
            int numBytes = 16;
            string result = _accountDbContext.CreateSalt(numBytes);
            Assert.AreEqual(typeof(string), result.GetType());
        }

        [TestMethod]
        public void HashMethodReturnsString()
        {
            int numBytes = 16;
            string password = "password";
            string salt = _accountDbContext.CreateSalt(numBytes);
            string result = _accountDbContext.HashPassPlusSalt(password, salt);
            Assert.AreEqual(typeof(string), result.GetType());
        }

        private string Create()
        {
            string email = "test@domain.com";
            string password = "password";
            string result = _accountDbContext.CreateNewUser(email, password);
            Assert.AreEqual("Account successfully created", result);
            return email;
        }

        private void GetByEmail(string email)
        {
            int result = _accountDbContext.UserEmailExists(email);
            Assert.AreEqual(1, result);
        }

        private void Update()
        {

        }

        private void Delete(string email)
        {
            int result = _accountDbContext.DeleteUser(email);
            Assert.AreEqual(1, result);
        }
    }
}
