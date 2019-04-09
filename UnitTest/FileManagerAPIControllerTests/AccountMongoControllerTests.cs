using System;
using Xunit;
using FileManagerAPI.Controllers;
using FileManagerDBLogic.Interfaces;
using FileManagerDBLogic.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FileManagerDBLogic.Models;

namespace UnitTest.FileManagerAPIControllerTests
{
    public class AccountMongoControllerTests
    {
        [Fact]
        public async Task Can_User_Login()
        {
            //arrange
            var mock = new Mock<IAccountMongoService>();
            var userService = new AccountMongoController(mock.Object);
            var user = new LoginModel { Email = "Admin@ukr.net", Password = "123456" };
            //act 
            await userService.Login(user);
            //assert 
            mock.Verify(c => c.Login(It.Is<LoginModel>(s => s.Email == "Admin@ukr.net")), Times.Once());
            Assert.Equal("Admin@ukr.net", user.Email);
        }

        [Fact]
        public async Task Can_Register_User()
        {
            //arrange
            var mock = new Mock<IAccountMongoService>();
            var userService = new AccountMongoController(mock.Object);
            var user = new RegisterModel { Email = "example@ukr.net", Password = "123456", ConfirmPassword = "123456" };
            //act 
            await userService.Register(user);
            //assert 
            mock.Verify(c => c.RegisterUser(It.Is<RegisterModel>(s => s.Email == "example@ukr.net")), Times.Once());
            Assert.Equal("example@ukr.net", user.Email);
        }

        [Fact]
        public async void UserViewTest()
        {
            //arrange
            var mock = new Mock<IAccountMongoService>();
            var users = new List<User> { new User { Email = "example@ukr.net", Name = "example" } };
            var userService = new AccountMongoController(mock.Object);
            //act
            await userService.GetUser();
            //assert
            mock.Verify(c => c.GetAllUser());
            Assert.NotEmpty(users);
        }
    }
}
