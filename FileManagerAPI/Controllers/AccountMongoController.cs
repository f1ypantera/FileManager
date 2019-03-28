using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMongoController : ControllerBase
    {
        private readonly IAccountMongoService accountMongoService;
        private readonly IMongoContext mongoContext;
        public AccountMongoController(IAccountMongoService accountMongoService)
        {
            this.accountMongoService = accountMongoService;
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<ActionResult<List<ProvidedRole>>> GetRoles()
        {
            return await accountMongoService.GetAllRole();
        }
        [HttpGet]
        [Route("Users")]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            return await accountMongoService.GetAllUser();
        }
        [HttpPost]
        [Route("CreateRole")]
        public async Task<ActionResult> CreateRole(ProvidedRole providedRole)
        {
            await accountMongoService.CreateRole(providedRole);
            return Ok(providedRole);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            await accountMongoService.RegisterUser(registerModel);
            return Ok("Has been registered");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await mongoContext.Users.FindAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);
                var isExist = await user.FirstOrDefaultAsync();

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
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logout");
        }
    }
}