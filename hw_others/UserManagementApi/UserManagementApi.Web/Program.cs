using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UserManagementApi.Data;
using UserManagementApi.Repository;
using UserManagementApi.Services;
using UserManagementApi.Validation;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

Serilog.Debugging.SelfLog.Enable(Console.Error);

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.OpenSearch(
        new Serilog.Sinks.OpenSearch.OpenSearchSinkOptions(
            new Uri("http://localhost:9200")))
    .CreateLogger();

builder.Host.UseSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(
        configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PasswordHasher<UserManagementApi.Entities.User>>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RegistrationValidator>();
builder.Services.AddScoped<DateFilteredRequestValidator>();

builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();