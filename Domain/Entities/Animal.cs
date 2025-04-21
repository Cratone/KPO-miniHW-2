using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Animal
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public SpeciesValueObject Species { get; private set; }
        public NonEmptyStringValueObject Name { get; private set; }
        public DateValueObject BirthDate { get; private set; }
        public GenderValueObject Gender { get; private set; }
        public NonEmptyStringValueObject FavoriteFood { get; private set; }
        public HealthStatusValueObject HealthStatus { get; private set; }
        public Guid? EnclosureId { get; private set; }

        public List<IDomainEvent> DomainEvents { get; private set; } = new List<IDomainEvent>();

        public Animal(SpeciesValueObject species, NonEmptyStringValueObject name, DateValueObject dateOfBirth, GenderValueObject gender, NonEmptyStringValueObject favoriteFood, HealthStatusValueObject healthStatus)
        {
            Species = species;
            Name = name;
            BirthDate = dateOfBirth;
            Gender = gender;
            FavoriteFood = favoriteFood;
            HealthStatus = healthStatus;
        }

        public void Feed(NonEmptyStringValueObject food)
        {
            
        }

        public void Heal()
        {
            if (HealthStatus == HealthStatusValueObject.Sick)
            {
                HealthStatus = HealthStatusValueObject.Healthy;
            }
        }

        public void MoveToEnclosure(Guid enclosureId)
        {
            Guid? previousEnclosureId = EnclosureId;
            EnclosureId = enclosureId;
            
            DomainEvents.Add(new AnimalMovedEvent(Id, previousEnclosureId, enclosureId));
        }
    }
} 