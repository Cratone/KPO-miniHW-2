using System;

namespace Domain.ValueObjects
{
    public record DateValueObject
    {
        public DateTime Value { get; }

        public DateValueObject(DateTime value)
        {
            Value = value;
        }

        public static implicit operator DateTime(DateValueObject date) => date.Value;
        public static explicit operator DateValueObject(DateTime value) => new DateValueObject(value);

        public override string ToString() => Value.ToShortDateString();
    }
} 