namespace Domain.ValueObjects
{
    public record GenderValueObject 
    {
        public static readonly GenderValueObject Male = new GenderValueObject("Male");
        public static readonly GenderValueObject Female = new GenderValueObject("Female");

        public string Value { get; }

        private GenderValueObject(string value) => Value = value;

        public static GenderValueObject FromString(string value)
        {
            return value switch
            {
                "Male" => Male,
                "Female" => Female,
                _ => throw new ArgumentException("Invalid gender")
            };
        }
        
        public override string ToString() => Value;
    }
} 