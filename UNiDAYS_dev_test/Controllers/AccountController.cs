using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using UNiDAYS_dev_test.Models;

namespace UNiDAYS_dev_test.Controllers
{
    public class AccountController : Controller
    {

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
        public ActionResult NewUser(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewUser");
            }

            string connStr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (UserExists(sqlCon, model.Email) == 0)
                {

                    try
                    {
                        string query = "INSERT INTO Users (UserEmail, UserSalt, UserPassword) VALUES(@modelEmail, @salt, @password)";
                        string salt = CreateSalt();
                        string hashedPass = HashPassPlusSalt(model.Password, salt);

                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                        sqlCmd.Parameters.AddWithValue("@modelEmail", model.Email);
                        sqlCmd.Parameters.AddWithValue("@salt", salt);
                        sqlCmd.Parameters.AddWithValue("@password", hashedPass);

                        sqlCon.Open();

                        Int32 rowsAffected = sqlCmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("RowsAffected: {0}", rowsAffected);
                    }
                    catch (SqlException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error" + ex.Errors[0].ToString());
                    }
                    TempData.Add("Notification", "Account successfully created");
                    return RedirectToAction("NewUser");
                }
                else
                {
                    ModelState.AddModelError("", "An account already exists using that Email Address");
                    return RedirectToAction("NewUser");
                }
            }

        }




        #region Helpers
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[16];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string HashPassPlusSalt(string pass, string salt)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytepass = uEncode.GetBytes(pass);
            byte[] bytesalt = uEncode.GetBytes(salt);
            byte[] passandsalt = new byte[bytepass.Length + bytesalt.Length];
            Buffer.BlockCopy(bytepass, 0, passandsalt, 0, bytepass.Length);
            Buffer.BlockCopy(bytesalt, 0, passandsalt, bytepass.Length, bytesalt.Length);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(passandsalt);
            return Convert.ToBase64String(hash);
        }

        public static int UserExists(SqlConnection conn, string userEmail)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE UserEmail = @userEmail";

            SqlCommand sqlCmd = new SqlCommand(query, conn);

            sqlCmd.Parameters.AddWithValue("@userEmail", userEmail);

            conn.Open();

            int ret = (int)sqlCmd.ExecuteScalar();

            conn.Close();

            return ret;
        }
        #endregion
    }
}