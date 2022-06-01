using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Entities.Contexts
{
    public partial class MyRoomDbContext
    {
        private static void BuildUserOwnModel(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<UserOwn>(entity =>
            {
                entity.HasIndex(e => e.HouseId).IsUnique(true);

                entity.HasIndex(e => e.ProjectId).IsUnique(true);
            });
        }
    }
}
