﻿using FileManagerDBLogic.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagerAPI.ModelsDTO
{
    public class StoredFileDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FileId { get; set; }
        public string FileName { get; set; }
        public double Size { get; set; }
        public DateTime dateTimeSave { get; set; }

    }
}
