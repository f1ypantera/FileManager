using FileManagerDBLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using FileManagerDBLogic.Models;
using System.Threading.Tasks;
using MongoDB.Driver;


namespace FileManagerDBLogic.Services
{
    public class TestService :ITestService
    {
        private readonly IMongoContext context;
        public TestService(IMongoContext context)
        {
            this.context = context;
        }
        public void CreateTest(TestDB testDB)
        {
             context.TestDB.InsertOne(testDB);
        }
        public IEnumerable<TestDB> GetAll()
        {
            var result =  context.TestDB.Find(c => true);
            return  result.ToList();
        }
        public TestDB GetbyId(string id)
        {
            var result =  context.TestDB.Find(c => c.Id == id);        
            return result.FirstOrDefault();
        }
    }
}

