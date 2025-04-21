using System;

namespace Domain.ValueObjects
{
    public record NonEmptyStringValueObject
    {
        public string Value { get; }

        public NonEmptyStringValueObject(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;

        public static implicit operator string(NonEmptyStringValueObject name) => name.Value;
        public static explicit operator NonEmptyStringValueObject(string value) => new NonEmptyStringValueObject(value);
    }
} 