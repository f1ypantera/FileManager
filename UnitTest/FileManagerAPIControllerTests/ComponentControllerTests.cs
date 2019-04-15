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
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.FileManagerAPIControllerTests
{
    public class ComponentControllerTests
    {
        [Fact]
        public async void ComponentTest_View()
        {
            //arrange
            var mock = new Mock<IFileManager>();
            var componentService = new ComponentController(mock.Object);
            //act
            var result = await componentService.Get();
            //assert
            mock.Verify(c => c.GetAllFile());
            Assert.NotEmpty(GetTestComponent());
        }
        [Fact]
        public async void ComponentTest_Return_NotFoundResult()
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
        public async void ComponentTest_Return_OkResult()
        {
            // не работает ? не знаю почему
            string fileId = "5cb45392fedbf916603e0fd5";        
            var mock = new Mock<IFileManager>();
            var componentService = new ComponentController(mock.Object);          
            var component = await componentService.GetId(fileId);
            Assert.IsType<OkObjectResult>(component);
        }
   
        [Fact]
        public void ComponentTest_Check_Components_list()
        {
            string fileId = "5cb45392fedbf916603e0fd5";
            var mock = new Mock<IFileManager>();
           // mock.Setup(repo => repo.GetbyId(fileId)).Returns(GetTestComponent().FirstOrDefault(p => p.FileId == fileId));
            var componentService = new ComponentController(mock.Object);
            var result = componentService.GetId(fileId);

            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<StoredFile>(viewResult.Value);

            Assert.Equal("Promise.txt", model.FileName);
            Assert.Equal(671, model.Size);
            Assert.Equal(fileId, model.FileId);

        }

        [Fact]
        public void ComponentTest_Return_ViewResult_List_Of_Components()
        {
            // Arrange
            var mock = new Mock<IFileManager>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestComponent());
            var componentService = new ComponentController(mock.Object);
            // Act
            var result = componentService.GetAllComponent();
            // Assert
            var view = Assert.IsType<List<StoredFile>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<StoredFile>>(view);
            Assert.Equal(GetTestComponent().Count, model.Count());
        }

        public  List<StoredFile> GetTestComponent()
        {
            var tests = new List<StoredFile>
            {
                new StoredFile { FileId = "5cb45392fedbf916603e0fd5",FileName="Promise.txt",Size = 671 },
                new StoredFile { FileId = "5cb45392fedbf916603e0fd7",FileName="121212.txt",Size = 6306 },          
            };
            return  tests;
        }
    }
}

   
