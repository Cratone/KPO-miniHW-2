using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register services
            services.AddScoped<AnimalTransferService>();
            services.AddScoped<FeedingOrganizationService>();
            services.AddScoped<ZooStatisticsService>();
            
            return services;
        }
    }
} 