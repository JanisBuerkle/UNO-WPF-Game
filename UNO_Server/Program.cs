using Microsoft.EntityFrameworkCore;
using UNO_Server.Hubs;
using UNO_Server.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddDbContext<RoomContext>(opt => opt.UseInMemoryDatabase("Rooms"));
builder.Services.AddSignalR();
builder.Services.AddSingleton<MyHub>();
// builder.WebHost.UseUrls("http://localhost:5000");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File((Path.Combine(AppContext.BaseDirectory, "Logs", "log.txt")), rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/myhub");

app.Run();