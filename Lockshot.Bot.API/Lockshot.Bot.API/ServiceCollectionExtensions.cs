using Lockshot.Bot.API.Core.Interfaces;
using Lockshot.Bot.API.Settings;
using System.Net.Http.Headers;

namespace Lockshot.Bot.API
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection RegiserApiServices(this IServiceCollection services, IConfiguration configuration)
        {

            var microserviceSetting = new MicroservicesSettings();

            configuration.GetSection("MicroservicesSettings").Bind(microserviceSetting);

            services.AddRefitClient<IHuggingFaceRefit>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(microserviceSetting.HuggingFaceUrl);
                    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", microserviceSetting.HuggingFaceToken);
                });

            return services;

        }

    }
}
