namespace SecretSanta.Data.SampleData
{
    public static class GroupSamples
    {
        public const string Employees = "Employess";
        public static Group CreateEmployeeGroup() => new Group(Employees);

        public const string NonEmployees = "Non-Employees";
        public static Group CreateNonEmployeeGroup() => new Group(NonEmployees);
    }
}
