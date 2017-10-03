namespace UNiDAYS_dev_test.Services
{
    public interface IAccountDbContext
    {
        string CreateNewUser(string email, string password);
        int UserEmailExists(string email);
        int DeleteUser(string email);
        string CreateSalt(int numBytes);
        string HashPassPlusSalt(string pass, string salt);
    }
}
