using Microsoft.EntityFrameworkCore;
using UNO_Server.Hubs;
using UNO_Server.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<RoomContext>(opt => opt.UseInMemoryDatabase("Rooms"));
builder.Services.AddSignalR();
builder.Services.AddSingleton<MyHub>();


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