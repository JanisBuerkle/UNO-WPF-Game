using Microsoft.EntityFrameworkCore;

namespace UNO_Server.Models;

public class RoomContext : DbContext
{
    public DbSet<Room> RoomItems { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;

    public RoomContext(DbContextOptions<RoomContext> options) : base(options)
    {
    }
}