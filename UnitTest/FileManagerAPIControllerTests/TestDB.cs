using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using FileManagerAPI.ModelsDTO;

namespace UnitTest.FileManagerAPIControllerTests
{
    public class TestDB
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListofPeople()
        {

        }

            public List<TestDBDTO> GetTestDBDTOs()
        {
            var test = new List<TestDBDTO>
            {
                new TestDBDTO { Id = "1",Name="Vadym",Surname = "Tselikin", Age = 24 },
                new TestDBDTO { Id = "2",Name="Ira",Surname = "Repnikova", Age = 22 },
                new TestDBDTO { Id = "3",Name="Vitaliy",Surname = "Tselikin", Age = 44 },
            };
            return test;
        }
          
    }
}
