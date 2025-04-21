using System;

namespace Domain.ValueObjects
{
    public record SpeciesValueObject
    {
        public NonEmptyStringValueObject Name { get; }
        public AnimalTypeValueObject AnimalType { get; }

        public SpeciesValueObject(NonEmptyStringValueObject name, AnimalTypeValueObject animalType)
        {
            Name = name;
            AnimalType = animalType;
        }
    }
} 