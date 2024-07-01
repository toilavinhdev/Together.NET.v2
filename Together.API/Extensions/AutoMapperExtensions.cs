using AutoMapper;
using Together.Application;
using Together.Shared.Extensions;

namespace Together.API.Extensions;

public static class AutoMapperExtensions
{
    public static void AddMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(
            cfg =>
            {
                cfg.AddMaps(ApplicationAssembly.Assembly);
            });

        services.AddSingleton(mapperConfig.CreateMapper().CreateAutoMapperInstance());
    }
}