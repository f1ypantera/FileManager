namespace FileManagerAPI.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        //dasdsad
        //SqlServer + MongoDb??
        //public IMongoCollection<Component> Components { get; set; }
        ////public Owner()
        ////{
        ////    Components = new Collection<Component>();
        ////}
    }
}
