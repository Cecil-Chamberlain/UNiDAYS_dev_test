using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace UNiDAYS_dev_test.Services
{
    public class AccountDbContext : IAccountDbContext
    {

        private readonly string connStr = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

        public string CreateNewUser(string email, string password)
        {
            if (UserEmailExists(email) == 0)
            {
                

                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                        try
                        {
                            string query = "INSERT INTO Users (UserEmail, UserSalt, UserPassword) VALUES(@modelEmail, @salt, @password)";
                            string salt = CreateSalt(16);
                            string hashedPass = HashPassPlusSalt(password, salt);

                            using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                            {
                                sqlCmd.Parameters.AddWithValue("@modelEmail", email);
                                sqlCmd.Parameters.AddWithValue("@salt", salt);
                                sqlCmd.Parameters.AddWithValue("@password", hashedPass);

                                sqlCon.Open();

                                Int32 rowsAffected = sqlCmd.ExecuteNonQuery();
                                System.Diagnostics.Debug.WriteLine("RowsAffected: {0}", rowsAffected);
                            }
                        }
                        catch (SqlException ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error" + ex.Errors[0].ToString());
                        }
                        return "Account successfully created";

                    }
            }
            return "The email address provided already exists on the system.";
        }
        
        public int UserEmailExists(string email)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE UserEmail = @userEmail";
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@userEmail", email);
                        sqlCon.Open();
                        int ret = (int)sqlCmd.ExecuteScalar(); // only works because ExecuteScalar returns only the first column, which is the ID attribute (int)

                        return ret;

                    }


                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error" + ex.Errors[0].ToString());
                return -1;
            }
        }

        public int DeleteUser(string email)
        {
            try
            {
                string query = "DELETE FROM Users WHERE UserEmail = @userEmail";
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(query, sqlCon))
                    {
                        sqlCmd.Parameters.AddWithValue("@userEmail", email);
                        sqlCon.Open();
                        int ret = sqlCmd.ExecuteNonQuery();

                        return ret;
                    }
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("Error" + ex.Errors[0].ToString());
                return -1;
            }
        }

        public string CreateSalt(int numBytes)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[numBytes];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string HashPassPlusSalt(string pass, string salt)
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
    }
}