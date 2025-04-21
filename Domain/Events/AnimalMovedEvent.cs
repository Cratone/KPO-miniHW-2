using System;
using Domain.ValueObjects;

namespace Domain.Events
{
    /// <summary>
    /// Событие, представляющее перемещение животного между вольерами
    /// </summary>
    public class AnimalMovedEvent : IDomainEvent
    {
        /// <summary>
        /// Идентификатор перемещаемого животного
        /// </summary>
        public Guid AnimalId { get; }
        
        /// <summary>
        /// Идентификатор исходного вольера (null, если это первое размещение)
        /// </summary>
        public Guid? PreviousEnclosureId { get; }
        
        /// <summary>
        /// Идентификатор целевого вольера
        /// </summary>
        public Guid NewEnclosureId { get; }
        
        /// <summary>
        /// Время, когда произошло событие
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Создает новое событие перемещения животного
        /// </summary>
        /// <param name="animalId">Идентификатор животного</param>
        /// <param name="previousEnclosureId">Идентификатор исходного вольера (может быть null)</param>
        /// <param name="newEnclosureId">Идентификатор целевого вольера</param>
        public AnimalMovedEvent(Guid animalId, Guid? previousEnclosureId, Guid newEnclosureId)
        {
            AnimalId = animalId;
            PreviousEnclosureId = previousEnclosureId;
            NewEnclosureId = newEnclosureId;
            OccurredOn = DateTime.UtcNow;
        }
    }
} 