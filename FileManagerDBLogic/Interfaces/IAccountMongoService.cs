using FileManagerDBLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManagerDBLogic.Interfaces
{
    public interface IAccountMongoService
    {
        Task CreateRole(ProvidedRole providedRole);
        Task<List<ProvidedRole>> GetAllRole();
        Task RegisterUser(RegisterModel registerModel);
        Task<User> Login(LoginModel loginModel);
        IEnumerable<User> GetAll();


    }
}
