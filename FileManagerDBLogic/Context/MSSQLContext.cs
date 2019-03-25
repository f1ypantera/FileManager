using Microsoft.EntityFrameworkCore;
using FileManagerDBLogic.Models;

namespace FileManagerDBLogic.Context
{
    public class MSSQLContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Role> Roles { get; set; }
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string clientRoleName = "user";

            string adminName = "Admin";
            string adminEmail = "admin@ukr.net";
            string adminPassword = "123456";


            Role adminRole = new Role { Id = 1, RoleName = adminRoleName };
            Role clientRole = new Role { Id = 2, RoleName = clientRoleName };
            Owner adminUser = new Owner { Id = 1, Name = adminName, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, clientRole });
            modelBuilder.Entity<Owner>().HasData(new Owner[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
