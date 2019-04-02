﻿using FileManagerDBLogic.Interfaces;
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
        public async Task CreateTest(TestDB testDB)
        {
            await context.TestDB.InsertOneAsync(testDB);
        }
        public IEnumerable<TestDB> GetAll()
        {
            var result =  context.TestDB.Find(c => true);
            return  result.ToList();
        }
    }
}
