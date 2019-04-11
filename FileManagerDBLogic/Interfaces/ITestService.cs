using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileManagerDBLogic.Models;


namespace FileManagerDBLogic.Interfaces
{
    public interface ITestService
    {
        void CreateTest(TestDB testDB);
        IEnumerable<TestDB> GetAll();
        TestDB GetbyId(string id);
    }
        
}
