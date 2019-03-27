using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using FileManagerDBLogic.Context;
using FileManagerDBLogic.Models;
using FileManagerDBLogic.Interfaces;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly MSSQLContext context;
        private readonly IMongoContext mongoContext;
        private readonly IRepositoryMSSQLService<Owner> repository;
        public AccountController(MSSQLContext context, IRepositoryMSSQLService<Owner> repository, IMongoContext mongoContext)
        {
            this.repository = repository;
            this.context = context;
            this.mongoContext = mongoContext;
        }

        [HttpGet]
        [Route("Owners")]
        public ActionResult GetAllOwners()
        {
            var owner = repository.GetAll().Include(n => n.Role);
            return Ok(owner);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            Owner owner = context.Owners.FirstOrDefault(u => u.Email == registerModel.Email);
            if (owner == null)
            {
                owner = new Owner { Email = registerModel.Email, Password = registerModel.Password };
                Role clinetRole = context.Roles.FirstOrDefault(r => r.RoleName == "user");
                if (clinetRole != null)
                    owner.Role = clinetRole;
                await context.Owners.AddAsync(owner);
                await mongoContext.Owners.InsertOneAsync(owner);
                await context.SaveChangesAsync();
            }
            return Ok("Has been registered");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginModel loginmodel)
        {
            if (ModelState.IsValid)
            {
                Owner owner = await context.Owners.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == loginmodel.Email && u.Password == loginmodel.Password);

                if (owner == null)
                {
                    return Unauthorized();
                }
                else
                {
                    await Authenticate(owner);
                }
            }
            return Ok("Ok");
        }
        private async Task Authenticate(Owner owner)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, owner.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, owner.Role?.RoleName)
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