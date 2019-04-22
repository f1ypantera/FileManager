using FileManagerDBLogic.Models;
using System;
using System.Collections.Generic;

namespace FileManagerBussinessLogic.Models
{
    public class Add
    {
        public string action { get; } = "ADD";
        public List<StoredFile> storedFiles { get; set; }
        
       
    }
}
