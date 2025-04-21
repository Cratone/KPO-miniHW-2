using System;

namespace Domain.Events
{
    /// <summary>
    /// Базовый интерфейс для всех доменных событий
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Время, когда произошло событие
        /// </summary>
        DateTime OccurredOn { get; }
    }
} 