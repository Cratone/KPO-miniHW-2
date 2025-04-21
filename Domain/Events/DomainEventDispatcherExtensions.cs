using Microsoft.Extensions.DependencyInjection;
using Domain.Events.EventHandlers;

namespace Domain.Events
{
    /// <summary>
    /// Методы расширения для настройки диспетчера доменных событий в DI контейнере
    /// </summary>
    public static class DomainEventDispatcherExtensions
    {
        /// <summary>
        /// Регистрирует обработчики доменных событий в DI контейнере
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <returns>Коллекция сервисов с добавленными обработчиками событий</returns>
        public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IEventHandler<AnimalMovedEvent>, AnimalMovedEventHandler>();
            services.AddSingleton<IEventHandler<FeedingTimeEvent>, FeedingTimeEventHandler>();
            
            services.AddSingleton(DomainEventDispatcher.Instance);
            
            return services;
        }

        /// <summary>
        /// Настраивает диспетчер доменных событий, регистрируя в нем обработчики
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        public static void ConfigureDomainEventHandlers(this IServiceProvider serviceProvider)
        {
            var dispatcher = serviceProvider.GetRequiredService<DomainEventDispatcher>();
            
            dispatcher.Register(serviceProvider.GetRequiredService<IEventHandler<AnimalMovedEvent>>());
            dispatcher.Register(serviceProvider.GetRequiredService<IEventHandler<FeedingTimeEvent>>());
        }
    }
} 