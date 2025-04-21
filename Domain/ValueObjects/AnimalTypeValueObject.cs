using System;

namespace Domain.ValueObjects
{
    public record AnimalTypeValueObject
    {
        public static readonly AnimalTypeValueObject Predator = new AnimalTypeValueObject("Predator");
        public static readonly AnimalTypeValueObject Herbivore = new AnimalTypeValueObject("Herbivore");
        public static readonly AnimalTypeValueObject Bird = new AnimalTypeValueObject("Bird");
        public static readonly AnimalTypeValueObject Aquatic = new AnimalTypeValueObject("Aquatic");

        public string Value { get; }

        private AnimalTypeValueObject(string value) => Value = value;

        public static AnimalTypeValueObject FromString(string value)
        {
            return value switch
            {
                "Predator" => Predator,
                "Herbivore" => Herbivore,
                "Bird" => Bird,
                "Aquatic" => Aquatic,
                _ => throw new ArgumentException("Invalid animal type")
            };
        }

        public override string ToString() => Value;
    }
} 