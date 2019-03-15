using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.Models
{
    public class UserListComponents
    {

        public int OwnerId { get; set; }
        public ICollection<Component> Components { get; set; }
        public UserListComponents()
        {
            Components = new List<Component>();
        }

    }
}
