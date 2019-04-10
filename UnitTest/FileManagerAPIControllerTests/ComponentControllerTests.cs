using FileManagerAPI.Controllers;
using FileManagerBussinessLogic.Infrastructure;
using FileManagerBussinessLogic.Interfaces;
using FileManagerDBLogic.Models;
using Moq;
using System;
using System.Collections.Generic;
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
            var components  = new List<StoredFile> { new StoredFile { FileName = "newFile", Size = 1234 } };
            var componentService = new ComponentController(mock.Object);
            //act
            var result = await componentService.Get();
            //assert
            mock.Verify(c => c.GetAllFile());
            Assert.NotEmpty(components);


        }
    }
}
