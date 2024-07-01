﻿using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Together.API.Extensions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

public static class EndpointExtensions
{
    public static void AddEndpoints(this IServiceCollection services)
    {
        var serviceDescriptors = Metadata.Assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToList();

        services.TryAddEnumerable(serviceDescriptors);
    }
    
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
    }
}