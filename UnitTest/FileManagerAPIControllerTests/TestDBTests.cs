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
        [Fact]
        public void Test_View()
        {
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetAll()).Returns(GetTest());
            var controller = new TestDBController(mock.Object, null);

            var result = controller.GetAllTest();

            var view = Assert.IsType<List<TestDB>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TestDB>>(view);
            Assert.Equal(GetTest().Count, model.Count());

        }
        [Fact]
        public  void Test_Return_GetAll_View()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, null);

            var test =  controller.GetAllTest();

            Assert.IsType<List<TestDB>>(test);
        }
        [Fact]
        public void Test_Return_By_Id_OkResult()
        {
            // не работает ? не знаю почему
            string testId = "5cb489bfa2e6bd3c6497afc0";
            var mock = new Mock<ITestService>();
         
      
            var controller = new TestDBController(mock.Object, null);

            var test = controller.GetId(testId);          
            Assert.IsType<OkObjectResult>(test);
        }
        [Fact]
        public void Test_Redirect_Model_IsNotValid()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, null);
            controller.ModelState.AddModelError("Name", "Required");
            TestDB testDB = new TestDB();

            var result = controller.AddTestNotMap(testDB);

            var view = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testDB, view.Value);
        }
        [Fact]
        public void Test_Can_AddModel()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, null);
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
        public void Test_Return_NotFoundResult()
        {
            var mock = new Mock<ITestService>();
            var controller = new TestDBController(mock.Object, null); 
    
            var result = controller.GetId(null);
         
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Test_Return_NotFoundResult_When_NotFound()
        {
            string testId = "5caeff79e1d244a0eccce920";
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetbyId(testId)).Returns(null as TestDB);
            var controller = new TestDBController(mock.Object, null);

            var result = controller.GetId(testId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Test_Match_Result()
        {
            string testId = "5cb489bfa2e6bd3c6497afc0";
            var mock = new Mock<ITestService>();
            mock.Setup(repo => repo.GetbyId(testId))
                .Returns(GetTest().FirstOrDefault(p => p.Id == testId));
            var controller = new TestDBController(mock.Object, null);

            var result = controller.GetId(testId);          
            var viewResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<TestDB>(viewResult.Value);

            Assert.Equal("Vadym", model.Name);
            Assert.Equal("Tselikin", model.Surname);
            Assert.Equal(23, model.Age);       
            Assert.Equal(testId, model.Id);
        }

        public List<TestDB> GetTest()
        {
            var tests = new List<TestDB>
            {
                new TestDB { Id = "5cb489bfa2e6bd3c6497afc0",Name="Vadym",Surname = "Tselikin", Age = 23 },        
            };
            return tests;
        }
          
    }
}
