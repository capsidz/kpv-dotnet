using UserManagementApi.Data;
using UserManagementApi.Repository;
using UserManagementApi.Services;
using UserManagementApi.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RegistrationValidator>();
builder.Services.AddScoped<RegistrationService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();