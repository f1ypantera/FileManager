using FileManagerDBLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerDBLogic.Interfaces
{
    public interface IAccountMongoService
    {
        Task CreateRole(ProvidedRole providedRole);
        Task<List<ProvidedRole>> GetAllRole();
        Task RegisterUser(RegisterModel registerModel);
        Task Login(LoginModel loginModel);
        Task<List<User>> GetAllUser();
    }
}
