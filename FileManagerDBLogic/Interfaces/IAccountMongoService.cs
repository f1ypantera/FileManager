using FileManagerDBLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManagerDBLogic.Interfaces
{
    public interface IAccountMongoService
    {
        Task CreateRole(ProvidedRole providedRole);
        Task<List<ProvidedRole>> GetAllRole();
        Task<User> RegisterUser(RegisterModel registerModel);
        Task<User> Login(LoginModel loginModel);
        Task<List<User>> GetAllUser();
        List<User> GetAllUserForUI();
    }
}
