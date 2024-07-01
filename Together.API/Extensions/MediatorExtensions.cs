using FluentValidation;
using Together.Application;
using Together.Application.Behaviors;

namespace Together.API.Extensions;

public static class MediatorExtensions
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly);
        
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(ApplicationAssembly.Assembly);
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));
            c.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });
    }
}