using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Together.API.Extensions;
using Together.Application;
using Together.Application.Features.FeatureFile.Command;
using Together.Application.Sockets;
using Together.Persistence;
using Together.Shared.Cloudinary;
using Together.Shared.Redis;
using Together.Shared.WebSockets;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment(out var appSettings);
builder.SetupSerilog(appSettings);

var services = builder.Services;
services.AddSingleton(appSettings);
services.AddCoreCors();
services.AddLanguages();
services.AddHttpContextAccessor();
services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
services.AddCoreAuth(appSettings.JwtTokenConfig);
services.AddEndpoints();
services.AddSwaggerDocument();
services.AddMapper();
services.AddMediator();
services.AddDbContext<TogetherContext>(options => 
    options.UseNpgsql(appSettings.PostgresConfig.ConnectionStrings));
services.AddRedis(appSettings.RedisConfiguration);
services.AddWebSocketManager(ApplicationAssembly.Assembly);
services.AddCloudinary();

var app = builder.Build();
app.UseLanguages();
app.UseStaticFiles();
app.UseDefaultExceptionHandler();
app.UseSwaggerDocument();
app.UseCoreCors();
app.UseCoreAuth();
app.UseWebSockets();
app.MapEndpoints();
app.MapWebSocketHandler<TogetherWebSocketHandler>("/ws");

app.MapGet("/api/ping", () => "Pong");

app.MapGet("/api/sockets", (TogetherWebSocketHandler handler) => handler.ConnectionManager.GetAll());

app.MapPost("/upload", (ISender sender, IFormFile file, string? bucket) => 
    sender.Send(new UploadFileCommand(file, bucket))).DisableAntiforgery();

TogetherContextInitialization.SeedAsync(
    app.Services.CreateScope()
        .ServiceProvider
        .GetRequiredService<TogetherContext>()).Wait();

app.Run();