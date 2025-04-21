using System;

namespace Domain.ValueObjects
{
    public record TimeValueObject
    {
        public TimeOnly Value { get; }

        public TimeValueObject(TimeOnly value)
        {
            Value = value;
        }

        public static implicit operator TimeOnly(TimeValueObject time) => time.Value;
        public static explicit operator TimeValueObject(TimeOnly value) => new TimeValueObject(value);

        public override string ToString() => Value.ToString();
    }
} 