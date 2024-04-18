using Microsoft.EntityFrameworkCore;
using UNO.Contract;

namespace UNO_Server.Models;

public class RoomContext : DbContext
{
    public RoomContext(DbContextOptions<RoomContext> options) : base(options)
    {
    }

    public DbSet<Room> RoomItems { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
}