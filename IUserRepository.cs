using NPWebAPI_Project_1.Models;

namespace NPWebAPI_Project_1.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        User Authenticate(string username,string password);
        User Register(string userName,string password);
    }
}
