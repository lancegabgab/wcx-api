using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using wcx_api.Data;
using wcx_api.Models;
using wcx_api.Seed;
using wcx_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WcxDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
	));

builder.Services
	.AddIdentityCore<User>()
	.AddRoles<Role>()
	.AddEntityFrameworkStores<WcxDbContext>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider
		.GetRequiredService<RoleManager<Role>>();

	var userManager = scope.ServiceProvider
		.GetRequiredService<UserManager<User>>();

	await IdentitySeeder.SeedAsync(
		roleManager,
		userManager
	);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
