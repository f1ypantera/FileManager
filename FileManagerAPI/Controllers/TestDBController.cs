﻿using System;
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
        public async Task<ActionResult> AddTest([FromBody] TestDBDTO testDTO)
        {      
            var test = mapper.Map<TestDB>(testDTO);
            await testService.CreateTest(test);          
            return Ok("Has been Added");
        }

    }
}