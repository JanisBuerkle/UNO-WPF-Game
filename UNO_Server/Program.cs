using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR; // Füge diese Zeile hinzu, um auf SignalR zuzugreifen
using UNO_Server.Hubs;
using UNO_Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<RoomContext>(opt => opt.UseInMemoryDatabase("Rooms"));
builder.Services.AddSignalR(); // Füge diese Zeile hinzu, um SignalR zu konfigurieren

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/myhub"); // Füge diese Zeile hinzu, um den Hub zu mappen

app.Run();