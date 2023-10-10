using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using TGProVN.Extension.Browser.Abstractions;
using TGProVN.Extension.Serialization;
using TGProVN.Extension.Serialization.Abstractions.Serializers;

namespace TGProVN.Extension.Browser;

public static class ServiceCollectionExtensions
{
    public static void AddLocalStorage(this IServiceCollection services)
    {
        services.AddSerialization();
            
        services.AddScoped<IStorageProvider, BrowserStorageProvider>()
                .AddScoped<ILocalStorageService, LocalStorageService>();
    }
    
    public static void AddLocalStorageAsSingleton(this IServiceCollection services)
    {
        services.AddSerialization();
        
        services.AddSingleton<IStorageProvider, BrowserStorageProvider>()
                .AddSingleton<ILocalStorageService, LocalStorageService>();
    }

    private static void AddSerialization(this IServiceCollection services)
    {
        if (services.All(x => x.ServiceType != typeof(IJsonSerializer))) {
            services.AddJsonSerialization(options => {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }
    }
}