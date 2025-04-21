using System;
using Domain.Events;

namespace Domain.Events.EventHandlers
{
    /// <summary>
    /// Обработчик событий перемещения животных
    /// </summary>
    public class AnimalMovedEventHandler : IEventHandler<AnimalMovedEvent>
    {
        /// <summary>
        /// Обрабатывает событие перемещения животного, выводя информацию в консоль
        /// </summary>
        /// <param name="event">Событие перемещения животного</param>
        public void Handle(AnimalMovedEvent @event)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===== ANIMAL MOVED EVENT =====");
            Console.WriteLine($"Time: {@event.OccurredOn}");
            Console.WriteLine($"Animal ID: {@event.AnimalId}");
            Console.WriteLine($"From Enclosure: {@event.PreviousEnclosureId?.ToString() ?? "None"}");
            Console.WriteLine($"To Enclosure: {@event.NewEnclosureId}");
            Console.WriteLine("===============================");
            Console.ResetColor();
        }
    }
} 