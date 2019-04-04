using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using MongoDB.Bson;
using MongoDB.Driver;
namespace FileManagerDBLogic.Services
{
    public class AccountMongoService : IAccountMongoService
    {
        private readonly IMongoContext context;
        public AccountMongoService(IMongoContext context)
        {
            this.context = context;
        }

        public async Task CreateRole(ProvidedRole providedRole)
        {
            await context.ProvidedRoles.InsertOneAsync(providedRole);
        }
        public async Task<List<ProvidedRole>> GetAllRole()
        {
            var result = await context.ProvidedRoles.FindAsync(c => true);
            return await result.ToListAsync();
        }


        public List<User> GetAllUserForUI()
        {

            
         
            //var result = context.Users.Aggregate().Unwind<User, User>(c => c.StoreFilesId).Lookup(
            //     foreignCollection: context.StoredFiles,
            //     localField: c => c.StoreFilesId,
            //     foreignField: e => e.FileId,
            //     @as: (User eu) => eu.StoreFilesId).
            //      Unwind(c => c.StoreFilesId).
            //      Group( );
            return result.ToList();
        }

        public async Task<List<User>> GetAllUser()
        {
            var result = await context.Users.FindAsync(c => true);
            return await result.ToListAsync();
        }

        public async Task<User> RegisterUser(RegisterModel registerModel)
        {
            var user = await context.Users.FindAsync(r => r.Email == registerModel.Email);
            var isExist = await user.FirstOrDefaultAsync();
            if (isExist == null)
            {
                string[] name = registerModel.Email.Split('@');
                User newUser = new User { Email = registerModel.Email, Password = registerModel.Password, Name = name[0] };
                var role = await context.ProvidedRoles.FindAsync(r => r.RoleName == "Admin");
                var isRole = role.FirstOrDefaultAsync();
                if (isRole != null)
                {
                    newUser.ProvidedRole = await isRole;
                    await context.Users.InsertOneAsync(newUser);
                }
            }
            return isExist;

        }
        public async Task<User> Login(LoginModel loginModel)
        {
            var asyncCursor = await context.Users.FindAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
            var user = await asyncCursor.FirstOrDefaultAsync();
            return user;
        }

    }
}
