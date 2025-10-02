using UdlaActivos.Web.Abstractions;
using UdlaActivos.Web.Data;
using UdlaActivos.Web.Services;

namespace UdlaActivos.Web.Composition;

public static class ServiceRegistration
{
    public static IServiceCollection AddAssetsModule(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddScoped<AssetsService>();
        services.AddHttpClient("Api", c =>
            c.BaseAddress = new Uri(cfg["ApiBaseUrl"] ?? "https://localhost:7241"));

        var source = cfg["DataSource"] ?? "Mock";
        if (source.Equals("Api", StringComparison.OrdinalIgnoreCase))
        {
            //services.AddScoped<IAssetsData, HttpAssetsData>(sp =>
            //    new HttpAssetsData(sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api")));
        }
        else
        {
            services.AddSingleton<IAssetsData, MockAssetsData>();
        }

        return services;
    }
}
