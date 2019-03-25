
namespace FileManagerDBLogic.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        //public ICollection<Component> Components { get; set; }
        //public Owner()
        //{
        //    Components = new List<Component>();
        //}
    }
}
