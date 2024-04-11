using Microsoft.EntityFrameworkCore;
using UNO_Server.ViewModel;
using UNO.Contract;

namespace UNO_Server.Models;

public class RoomContext : DbContext
{
    public RoomContext(DbContextOptions<RoomContext> options) : base(options)
    {
    }

    public DbSet<RoomDTO> RoomItems { get; set; } = null!;
    public DbSet<MultiplayerDTO> Players { get; set; } = null!;
}