using HotDesk;
using HotDesk.Entities;
using HotDesk.Middleware;
using HotDesk.Services;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);





// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<HotDeskDbContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<HotDeskSeeder>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddScoped<ILocationServices, LocationServices>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IDeskServices, DeskServices>();










var app = builder.Build();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<HotDeskSeeder>();
seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
