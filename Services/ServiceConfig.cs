namespace СalculationOfUtilities.Services
{
    public readonly struct ServiceConfig
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Rate { get; }
        public decimal Standard { get; }
        public string MeasureUnit { get; }

        public ServiceConfig(int id, string name, decimal rate, decimal standard, string measureUnit)
        {
            Id = id;
            Name = name;
            Rate = rate;
            Standard = standard;
            MeasureUnit = measureUnit;
        }

        public override string ToString() =>
            $"{Id,-2} {Name,-25} {Rate,-7} {Standard,-13} {MeasureUnit,-10}";
    }
}
