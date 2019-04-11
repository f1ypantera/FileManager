using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using FileManagerAPI.ModelsDTO;
using FileManagerDBLogic.Models;
using FileManagerDBLogic.Services;
using FileManagerDBLogic.Interfaces;
using FileManagerAPI.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace UnitTest.FileManagerAPIControllerTests
{
    public class TestDBTests
    {
        private IMapper mapper;
        
        [Fact]
        public void IndexReturnsAViewResultWithAListofPeople()
        {
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestDBDTOs());
            var controller = new TestDBController(mock.Object, mapper);

            var result = controller.GetAllTest();

            var view = Assert.IsType<List<TestDB>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TestDB>>(view);
            Assert.Equal(GetTestDBDTOs().Count, model.Count());

        }
        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTestDBDTOs());
            var controller = new TestDBController(mock.Object, mapper);
            
            var result = controller.GetResult();
      
            Assert.IsType<OkObjectResult>(result);

        }
        public List<TestDB> GetTestDBDTOs()
        {
            var tests = new List<TestDB>
            {
                new TestDB { Id = "1",Name="Vadym",Surname = "Tselikin", Age = 24 },
                new TestDB { Id = "2",Name="Ira",Surname = "Repnikova", Age = 22 },
                new TestDB { Id = "3",Name="Vitaliy",Surname = "Tselikin", Age = 44 },
            };
            return tests;
        }
          
    }
}
