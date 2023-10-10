using Microsoft.Extensions.DependencyInjection;
using TGProVN.Extension.Serialization.Abstractions.Options;
using TGProVN.Extension.Serialization.Abstractions.Serializers;
using TGProVN.Extension.Serialization.JsonConverters;

namespace TGProVN.Extension.Serialization;

public static class ServiceCollectionExtensions
{
    public static void AddJsonSerialization(this IServiceCollection services,
                                            Action<SystemTextJsonOptions>? configure = null)
    {
        services
           .AddScoped<IJsonSerializerOptions, SystemTextJsonOptions>()
           .Configure<SystemTextJsonOptions>(configureOptions => {
                configure?.Invoke(configureOptions);

                if (configureOptions.JsonSerializerOptions.Converters.All(
                        c => c.GetType() != typeof(TimespanJsonConverter)))
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
            });
        
        services.AddScoped<IJsonSerializer, SystemTextJsonSerializer>();
    }
}