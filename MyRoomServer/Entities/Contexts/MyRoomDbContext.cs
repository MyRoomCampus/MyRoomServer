using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Entities.Contexts
{
    public partial class MyRoomDbContext : DbContext
    {
        public MyRoomDbContext(DbContextOptions<MyRoomDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserClaim> UsersClaims { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Widget> Widgets { get; set; } = null!;
        public DbSet<AgentHouse> AgentHouses { get; set; } = null!;
        public DbSet<Media> Medias { get; set; } = null!;
        public DbSet<HouseMapUser> HouseMapUsers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            BuildAgentHouseModel(modelBuilder);
            BuildUserModel(modelBuilder);
        }
    }
}
