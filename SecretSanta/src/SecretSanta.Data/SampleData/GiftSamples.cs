namespace SecretSanta.Data.SampleData
{
    public static class GiftSamples
    {
        public const string Car = "Car";
        public const string CarUrl = "www.ferrari.com";
        public const string CarDescription = "Red ferrari is fast.";

        public const string Motorcycle = "Arduino";
        public const string MotorcycleUrl = "www.hd.com";
        public const string MotorcycleDescription = "Every good geek needs an IOT device";

        public static Gift CreateCar() => new Gift(Car, CarUrl, CarDescription, UserSamples.CreateInigoMontoya());
        public static Gift CreateMotorcycle() => new Gift(Motorcycle, MotorcycleUrl, MotorcycleDescription, UserSamples.CreateLukeSkywalker());
    }
}
