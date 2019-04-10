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
        public async void Test ()
        {
            var mock = new Mock<IFileManager>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestComponents());
            var controller = new ComponentController(mock.Object);

            var result = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<List<StoredFile>>>(result);
            var model = Assert.IsAssignableFrom<List<StoredFile>>(actionResult);
            Assert.Equal(GetTestComponents().Count, model.Count());

        }

        private List<StoredFile> GetTestComponents()
        {

            var files = new List<StoredFile>
            {
                new StoredFile { FileId = "1", FileName = "example", Size = 123},
        
            };
            return files;
        }
    }

}
