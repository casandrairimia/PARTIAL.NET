using RoomHub.Api.Endpoints;
using RoomHub.Api.Repositories;
using RoomHub.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Starter organized in layers: Endpoints -> Services -> Repositories -> Models.
builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<BookingService>();

var app = builder.Build();

app.MapGet("/", () => "RoomHub API is running.");
app.MapRoomEndpoints();
app.MapBookingEndpoints();

app.Run();
