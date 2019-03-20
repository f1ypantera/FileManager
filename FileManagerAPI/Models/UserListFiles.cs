using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Models
{
    public class UserListFiles
    {

        public int OwnerId { get; set; }
        public ICollection<StoredFile> StoredFiles { get; set; }
        public UserListFiles()
        {
            StoredFiles = new List<StoredFile>();
        }

    }
}
