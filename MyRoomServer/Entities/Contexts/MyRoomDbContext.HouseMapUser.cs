using Microsoft.EntityFrameworkCore;

namespace MyRoomServer.Entities.Contexts
{
    public partial class MyRoomDbContext
    {
        private static void BuildHouseMapUserModel(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<HouseMapUser>(entity =>
            {
                entity.HasIndex(e => e.HouseId).IsUnique(true);
            });
        }
    }
}
