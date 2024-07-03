using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Together.API.Extensions;
using Together.Persistence;
using Together.Shared.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.SetupEnvironment(out var appSettings);
builder.SetupSerilog();

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

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseLanguages();
app.UseDefaultExceptionHandler();
app.UseSwaggerDocument();
app.UseCoreCors();
app.UseCoreAuth();
app.MapEndpoints();

app.MapGet("/ping", () => "Pong");
//
// TogetherContextInitialization.SeedAsync(
//     app.Services.CreateScope()
//         .ServiceProvider
//         .GetRequiredService<TogetherContext>()).Wait();

app.Run();