using System.Collections.Generic;


namespace FileManagerDBLogic.Models
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
