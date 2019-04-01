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
            CreateMap<User, UserDTO>().ReverseMap().ForMember(c=>c.StoreFilesId,opt =>opt.MapFrom(x=>x.StoreFiles));
            CreateMap<StoredFile, StoredFileDTO>().ReverseMap().ForMember(c => c.User, opt => opt.MapFrom(x => x.FileName));

          CreateMap<TestDB,TestDBDTO>().ReverseMap();
        }
    }
}
