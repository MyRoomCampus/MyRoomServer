using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Entities
{
    public class MyRoomDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserClaim> UsersClaims { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Widget> Widgets { get; set; } = null!;
    }
}
