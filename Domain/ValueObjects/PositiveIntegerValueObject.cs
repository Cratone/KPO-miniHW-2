using System;

namespace Domain.ValueObjects
{
    public record PositiveIntegerValueObject
    {
        public int Value { get; }

        public PositiveIntegerValueObject(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Value must be positive", nameof(value));

            Value = value;
        }

        public override string ToString() => Value.ToString();

        public static implicit operator int(PositiveIntegerValueObject value) => value.Value;
        public static explicit operator PositiveIntegerValueObject(int value) => new PositiveIntegerValueObject(value);
    }
} 