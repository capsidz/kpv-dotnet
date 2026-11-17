using ApprovalService.Data;
using Microsoft.EntityFrameworkCore;
using ApprovalService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestService>();

builder.Services.AddHttpClient("UserManagementApi", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["UserManagementApi:BaseUrl"]!);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "ApprovalService_";
});

builder.Services.AddHttpClient<IAiAnalysisService, OllamaAiAnalysisService>(
    client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Ollama:BaseUrl"]!);
    });

builder.Services.AddScoped<IUserManagementClient, UserManagementClient>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();