﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMongoController : ControllerBase
    {
        private readonly IAccountMongoService accountMongoService;

        public AccountMongoController(IAccountMongoService accountMongoService)
        {
            this.accountMongoService = accountMongoService;

        }

        [HttpGet]
        [Route("Roles")]
        public async Task<List<ProvidedRole>> GetRoles()
        {
            return await accountMongoService.GetAllRole();
        }
        [HttpGet]
        [Route("Users")]
        public async Task<List<User>> GetUser()
        {
            return await accountMongoService.GetAllUser();
        }
        [HttpGet]
        [Route("AllUsers")]
        public IEnumerable<User> GetAllUser()
        {
            var result = accountMongoService.GetAllUserNotAsync().ToList();
            return result;
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(ProvidedRole providedRole)
        {
            await accountMongoService.CreateRole(providedRole);
            return Ok(providedRole);
        }

  
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {

            var isExist = await accountMongoService.RegisterUser(registerModel);
            if (isExist != null)
            {
                return Unauthorized();
            }
            return Ok("Has been registered");

        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var isExist  = await accountMongoService.Login(loginModel);
                if (isExist == null)
                {
                    return Unauthorized();
                }
                else
                {

                    await Authenticate(isExist);
                }
            }
            return Ok("Login sucess ");
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.ProvidedRole?.RoleName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logout");
        }
    }
}