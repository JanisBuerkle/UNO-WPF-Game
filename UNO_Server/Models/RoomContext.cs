using Microsoft.EntityFrameworkCore;
using UNO_Server.ViewModel;

namespace UNO_Server.Models;
public class RoomContext : DbContext
{
    public RoomContext(DbContextOptions<RoomContext> options) : base(options)
    {
    }

    public DbSet<RoomItem> RoomItems { get; set; } = null!;
    public DbSet<MultiplayerPlayer> Players { get; set; } = null!;
}