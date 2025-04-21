using System;
using Domain.Events;

namespace Domain.Events.EventHandlers
{
    /// <summary>
    /// Обработчик событий кормления животных
    /// </summary>
    public class FeedingTimeEventHandler : IEventHandler<FeedingTimeEvent>
    {
        /// <summary>
        /// Обрабатывает событие кормления животного, выводя информацию в консоль
        /// </summary>
        /// <param name="event">Событие кормления животного</param>
        public void Handle(FeedingTimeEvent @event)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("===== FEEDING TIME EVENT =====");
            Console.WriteLine($"Time: {@event.OccurredOn}");
            Console.WriteLine($"Animal ID: {@event.AnimalId}");
            Console.WriteLine($"Schedule ID: {@event.ScheduleId}");
            Console.WriteLine($"Food: {@event.Food}");
            Console.WriteLine("===============================");
            Console.ResetColor();
        }
    }
} 