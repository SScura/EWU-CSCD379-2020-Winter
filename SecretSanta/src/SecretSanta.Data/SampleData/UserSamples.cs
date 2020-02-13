namespace SecretSanta.Data.SampleData
{
    class UserSamples
    {
        public const string Inigo = "Inigo";
        public const string Montoya = "Montoya";
        public const string MontoyaUserName = "imontoya";

        public const string Luke = "Luke";
        public const string Skywalker = "Skywalker";
        public const string SkywalkerUserName = "bfields";

        public static User CreateInigoMontoya() => new User(Inigo, Montoya);
        public static User CreateLukeSkywalker() => new User(Luke, Skywalker);
    }
}
