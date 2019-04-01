using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileManagerDBLogic.Models;


namespace FileManagerDBLogic.Interfaces
{
    public interface ITestService
    {
        Task CreateTest(TestDB testDB);
        Task<List<TestDB>> GetTest();
    }
}
