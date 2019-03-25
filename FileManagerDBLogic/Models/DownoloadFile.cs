using System;
using System.Collections.Generic;


namespace FileManagerDBLogic.Models
{
    public class DownoloadFile
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public int TotalCount { get; set; }
        public List<ChunksOfFiles> chunks { get; set; }
        public DateTime LastDownoloadTime { get; set; }
    }
}
