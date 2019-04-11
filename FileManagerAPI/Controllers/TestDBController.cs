using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Reflection;
using FileManagerDBLogic.Interfaces;
using FileManagerBussinessLogic.Interfaces;
using FileManagerDBLogic.Models;
using FileManagerAPI.ModelsDTO;

namespace FileManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDBController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITestService testService;
        public TestDBController(ITestService testService,IMapper mapper)
        {
            this.testService = testService;
            this.mapper = mapper;
        }
        [HttpPost]
        [Route("AddTest")]
        public  IActionResult AddTest([FromBody] TestDBDTO testDTO)
        {      
            var test = mapper.Map<TestDB>(testDTO);
            testService.CreateTest(test);          
            return Ok("Has been Added");
        }
        [HttpGet]
        [Route("Test")]
        public ActionResult GetTest()
        {        
            var result =  mapper.Map<IEnumerable<TestDB>, List<TestDBDTO>>(testService.GetAll());
            return Ok(result);

        }

        [HttpGet]
        [Route("AllTest")]
        public IEnumerable<TestDB> GetAllTest()
        {
            var result = testService.GetAll().ToList();
            return result;

        }
        [HttpGet]
        public ActionResult GetResult()
        {
            var result = testService.GetAll();
            return Ok(result);

        }
        [HttpPost]
        [Route("AddTestNotMap")]
        public IActionResult AddTestNotMap([FromBody] TestDB testDB)
        {
      
            testService.CreateTest(testDB);
            return Ok("Has been Added");
        }

    }
}