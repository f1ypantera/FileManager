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
            mock.Setup(repo => repo.GetAll()).Returns(GetTest());
            var controller = new TestDBController(mock.Object, mapper);

            var result = controller.GetAllTest();

            var view = Assert.IsType<List<TestDB>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TestDB>>(view);
            Assert.Equal(GetTest().Count, model.Count());

        }
        [Fact]
        public void AddPhoneReturnsViewResultWithPhoneModel()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, mapper);
            controller.ModelState.AddModelError("Name", "Required");
            TestDB testDB = new TestDB();

            var result = controller.AddTestNotMap(testDB);

            var view = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testDB, view.Value);
        }

        [Fact]
        public void AddTestReturnsARedirectAndAddsTest()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, mapper);


            var newTest = new TestDB()
            {
                Name = "Lena",
                Surname = "Tselikina",
                Age = 42
            };

            var result = controller.AddTestNotMap(newTest);   
            mock.Verify(r => r.CreateTest(It.Is<TestDB>(s=>s.Name == "Lena")),Times.Once());        
            Assert.Equal("Lena", newTest.Name);
        }
        [Fact]
        public void TrueNotFoundResult()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, mapper); 
    
            var result = controller.GetId(null);
         
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetTestReturnsNotFoundResultWhenTestNotFound()
        {
            string testId = "5caeff79e1d244a0eccce920";
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetbyId(testId)).Returns(null as TestDB);
            var controller = new TestDBController(mock.Object, mapper);

            var result = controller.GetId(testId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetTestReturnsViewResultWithTest()
        {
            string testId = "5caeff79e1d244a0eccce920";
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetbyId(testId))
                .Returns(GetTest().FirstOrDefault(p => p.Id == testId));
            var controller = new TestDBController(mock.Object, mapper);

            var result = controller.GetId(testId);          
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<TestDB>(viewResult.Value);

            Assert.Equal("Vadym", model.Name);
            Assert.Equal("Tselikin", model.Surname);
            Assert.Equal(24, model.Age);       
            Assert.Equal(testId, model.Id);
        }

            public List<TestDB> GetTest()
        {
            var tests = new List<TestDB>
            {
                new TestDB { Id = "5caeff79e1d244a0eccce920",Name="Vadym",Surname = "Tselikin", Age = 24 },
                new TestDB { Id = "5caf2397c300476870e7fb15",Name="Ira",Surname = "Repnikova", Age = 22 },
                new TestDB { Id = "5caf2397c300476870e7fb14",Name="Vitaliy",Surname = "Tselikin", Age = 44 },
            };
            return tests;
        }
          
    }
}
