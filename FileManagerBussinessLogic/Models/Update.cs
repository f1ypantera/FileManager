using FileManagerDBLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagerBussinessLogic.Models
{
    class Update
    {
        public string Action { get; } = "Update";
        public string id { get; set; }
        public List<StoredFile> storedFiles { get; set; }
    }
}
