namespace Domain.ValueObjects
{
    public record HealthStatusValueObject
    {
        public static readonly HealthStatusValueObject Healthy = new HealthStatusValueObject("Healthy");
        public static readonly HealthStatusValueObject Sick = new HealthStatusValueObject("Sick");

        public string Value { get; }

        private HealthStatusValueObject(string value) => Value = value;

        public static HealthStatusValueObject FromString(string value)
        {
            return value switch
            {
                "Healthy" => Healthy,
                "Sick" => Sick,
                _ => throw new ArgumentException("Invalid health status")
            };
        }
        
        public override string ToString() => Value;
    }
} 