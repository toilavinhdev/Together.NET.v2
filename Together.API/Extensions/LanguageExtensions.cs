using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Together.Shared.Localization;

namespace Together.API.Extensions;

public static class LanguageExtensions
{
    private static readonly List<CultureInfo> SupportedCultures =
    [
        new CultureInfo("en-US"),
        new CultureInfo("vi-VN")
    ];
    
    public static void AddLanguages(this IServiceCollection services)
    {
        services.AddLocalization();
        services.AddSingleton<ILocalizationService, LocalizationService>();
    }

    public static void UseLanguages(this WebApplication app)
    {
        app.UseRequestLocalization(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(SupportedCultures[0]);
            options.SupportedCultures = SupportedCultures;
            options.SupportedUICultures = SupportedCultures;
            options.RequestCultureProviders =
            [
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            ];
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
    }
}