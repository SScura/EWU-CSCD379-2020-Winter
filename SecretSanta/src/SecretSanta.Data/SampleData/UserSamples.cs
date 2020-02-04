namespace SecretSanta.Data
{
    public static class UserSamples
    {
        static public User CreateInigoMontoya() => new User(Inigo, Montoya);
        public const string Inigo = "Inigo";
        public const string Montoya = "Montoya";
        public const string MontoyaUserName = "imontoya";

        static public User CreateLukeSkywalker() => new User(Luke, Skywalker);
        public const string Luke = "Luke";
        public const string Skywalker = "Skywalker";
        public const string SkywalkerUserName = "lskywalker";
    }
}