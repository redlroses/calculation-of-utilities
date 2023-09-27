namespace СalculationOfUtilities.Data
{
    public class Resource
    {
        public string Name { get; }
        public decimal Rate { get; }
        public decimal Standart { get; }
        public string MeasureUnit { get; }

        public Resource(string name, string measureUnit, decimal rate, decimal standart)
        {
            Name = name;
            MeasureUnit = measureUnit;
            Rate = rate;
            Standart = standart;
        }

        public override string ToString() =>
            $"Название: {Name,-20} Тариф: {Rate,-7} Норматив: {Standart,-8} Еденица измерения: {MeasureUnit,-7}";
    }
}
