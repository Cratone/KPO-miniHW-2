using System;
using Domain.ValueObjects;

namespace Domain.Events
{
    /// <summary>
    /// Событие, представляющее процесс кормления животного
    /// </summary>
    public class FeedingTimeEvent : IDomainEvent
    {
        /// <summary>
        /// Идентификатор животного, которое кормят
        /// </summary>
        public Guid AnimalId { get; }
        
        /// <summary>
        /// Идентификатор расписания кормления
        /// </summary>
        public Guid ScheduleId { get; }
        
        /// <summary>
        /// Время, когда произошло событие
        /// </summary>
        public DateTime OccurredOn { get; }
        
        /// <summary>
        /// Тип пищи, использованной для кормления
        /// </summary>
        public NonEmptyStringValueObject Food { get; }
        
        /// <summary>
        /// Создает новое событие кормления животного
        /// </summary>
        /// <param name="animalId">Идентификатор животного</param>
        /// <param name="scheduleId">Идентификатор расписания кормления</param>
        /// <param name="food">Тип пищи</param>
        public FeedingTimeEvent(Guid animalId, Guid scheduleId, NonEmptyStringValueObject food)
        {
            AnimalId = animalId;
            ScheduleId = scheduleId;
            Food = food;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 