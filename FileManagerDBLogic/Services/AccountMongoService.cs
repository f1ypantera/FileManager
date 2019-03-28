using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
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
        public async Task<List<User>> GetAllUser()
        {
            var result = await context.Users.FindAsync(c => true);

            
            return await result.ToListAsync();
        }
        public async Task RegisterUser(RegisterModel registerModel)
        {
            var user = await context.Users.FindAsync(r => r.Email == registerModel.Email);
            string[] name = registerModel.Email.Split('@');
            var isExist = await user.FirstOrDefaultAsync();
            if (isExist == null)
            {
                User newUser = new User { Email = registerModel.Email, Password = registerModel.Password ,Name = name[0]};
                var role = await context.ProvidedRoles.FindAsync(r => r.RoleName == "User");
                var isRole = role.FirstOrDefaultAsync();
                if (isRole != null)
                {
                    newUser.ProvidedRole = await isRole;
                    await context.Users.InsertOneAsync(newUser);
                }
            }
        }
        public async Task<User> Login(LoginModel loginModel)
        {
            var asyncCursor = await context.Users.FindAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
            var user = await asyncCursor.FirstOrDefaultAsync();
            return user;           
         }
   
    }
}
