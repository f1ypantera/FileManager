﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using MongoDB.Driver;

namespace FileManagerDBLogic.Services
{
   public class AccountMongoService:IAccountMongoService
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
            var isExist = await user.FirstOrDefaultAsync();
            if (isExist == null)
            {
                User newUser = new User { Email = registerModel.Email, Password = registerModel.Password };
                var role = await context.ProvidedRoles.FindAsync(r => r.RoleName == "User");
                var isRole = role.FirstOrDefaultAsync();
                if(isRole!=null)
                {
                    newUser.ProvidedRole = await isRole;
                    await context.Users.InsertOneAsync(newUser);
                }                           
            }
        }
    
    }
}
