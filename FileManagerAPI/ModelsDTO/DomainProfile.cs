using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FileManagerDBLogic.Models;

namespace FileManagerAPI.ModelsDTO
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<User,UserDTO>().ReverseMap();
            CreateMap<StoredFile, StoredFileDTO>().ReverseMap();
            CreateMap<TestDB,TestDBDTO>().ReverseMap();
        }
    }
}
