using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace Corporate.Chat.API.Configurations
{
    public static class ResponseCompressionConfig
    {
        public static void AddResponseCompressionConfiguration(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
        }
    }
}