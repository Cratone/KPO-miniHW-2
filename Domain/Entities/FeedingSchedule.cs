using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class FeedingSchedule
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid AnimalId { get; private set; }
        public TimeValueObject Time { get; private set; }
        public NonEmptyStringValueObject Food { get; private set; }
        public bool Performed { get; private set; } = true;

        public List<IDomainEvent> DomainEvents { get; private set; } = new List<IDomainEvent>();

        public FeedingSchedule(Guid animalId, TimeValueObject time, NonEmptyStringValueObject food)
        {
            AnimalId = animalId;
            Time = time;
            Food = food;
        }

        public void ChangeSchedule(TimeValueObject newTime)
        {
            Time = newTime;
        }

        public void CancelExecution()
        {
            Performed = false;
        }

        public void RestoreExecution() 
        {
            Performed = true;
        }

        public override string ToString() => $"Feeding schedule for animal {AnimalId}: {Time}, Food: {Food}";
    }
} 