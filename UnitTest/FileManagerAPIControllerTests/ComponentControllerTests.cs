using FileManagerAPI.Controllers;
using FileManagerBussinessLogic.Infrastructure;
using FileManagerBussinessLogic.Interfaces;
using FileManagerDBLogic.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTest.FileManagerAPIControllerTests
{
    public class ComponentControllerTests
    {

        [Fact]
        public async void ComponentView()
        {
            //arrange
            var mock = new Mock<IFileManager>();
            var components = new List<StoredFile> { new StoredFile { FileName = "newFile", Size = 1234 } };
            var componentService = new ComponentController(mock.Object);
            //act
            var result = await componentService.Get();
            //assert
            mock.Verify(c => c.GetAllFile());
            Assert.NotEmpty(components);
        }
        [Fact]
        public async void ComponentViewIsNotFound()
        {
            //arrange
            var mock = new Mock<IFileManager>();
            var component = new ComponentController(mock.Object);
            //act
            var result = await component.GetId(null);
            //assert
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async void Task_GetPostById_Return_OkResult()
        {
            string fileId = "5cb45392fedbf916603e0fd5";        
            var mock = new Mock<IFileManager>();
            var componentService = new ComponentController(mock.Object);          
            var component = await componentService.GetId(fileId);
            Assert.IsType<OkObjectResult>(component);
        }
        [Fact]
        public async void Task_GetPostById_Return_NotFoundResult()
        {

            var mock = new Mock<IFileManager>();
            var componentService = new ComponentController(mock.Object);
            string fileId = "2";
            var component = await componentService.GetId(fileId);
            Assert.IsType<NotFoundResult>(component);
        }

        public List<StoredFile> GetTestComponent()
        {
            var tests = new List<StoredFile>
            {
                new StoredFile { FileId = "5cb45392fedbf916603e0fd5",FileName="Promise.txt",Size = 671 },
                new StoredFile { FileId = "5cb45392fedbf916603e0fd7",FileName="121212.txt",Size = 6306 },          
            };
            return tests;
        }
    }
}

   
