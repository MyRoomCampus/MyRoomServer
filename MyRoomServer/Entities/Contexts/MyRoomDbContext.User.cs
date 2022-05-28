using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Entities.Contexts
{
    public partial class MyRoomDbContext : DbContext
    {
        private static void BuildUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique(true);
            });
        }
    }
}
