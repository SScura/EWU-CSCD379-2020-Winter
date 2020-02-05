namespace SecretSanta.Data
{
    public static class GiftSamples
    {
        public static Gift CreateCar() => new Gift(
            "Ferrari",
            "This gift cost way too much.",
            ".redferrari."
        );
        public static Gift CreateMotorcycle() => new Gift(
            "Harley Davidson",
            "That's more like it.",
            ".hd."
        );
    }
}