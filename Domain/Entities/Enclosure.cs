using System;
using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Enclosure
    {
        private List<Guid> _animalIds = new List<Guid>();
        public IReadOnlyList<Guid> AnimalIds => _animalIds; 
        public bool IsCleaned { get; private set; } = true;
        
        public Guid Id { get; private set; } = Guid.NewGuid();
        public AnimalTypeValueObject AnimalType { get; }
        public PositiveIntegerValueObject Size { get; }
        public int AnimalCount { get => _animalIds.Count; }
        public PositiveIntegerValueObject MaxCapacity { get; }

        public Enclosure(AnimalTypeValueObject animalType, PositiveIntegerValueObject size, PositiveIntegerValueObject maxCapacity)
        {
            AnimalType = animalType;
            Size = size;
            MaxCapacity = maxCapacity;
        }

        public bool IsCompatibleWith(Animal animal) => AnimalType == animal.Species.AnimalType;

        public bool CanTransfer(Animal animal) => IsCompatibleWith(animal) && AnimalCount < MaxCapacity;

        public void AddAnimal(Guid animalId)
        {
            if (AnimalCount >= MaxCapacity)
                throw new InvalidOperationException("Enclosure is at maximum capacity");
            
            _animalIds.Add(animalId);
        }

        public void RemoveAnimal(Guid animalId)
        {
            _animalIds.Remove(animalId);
        }

        public void Clean()
        {
            IsCleaned = true;
        }
    }
} 