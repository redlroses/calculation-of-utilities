using System;

namespace СalculationOfUtilities.Interpreters.Data
{
    public readonly struct DoubleReadingData : IReadingData
    {
        public DoubleReadingData(int id, DateTime date, decimal firstValue = 0, decimal secondValue = 0)
        {
            Id = id;
            Date = date;
            FirstValue = firstValue;
            SecondValue = secondValue;
        }

        public int Id { get; }
        public DateTime Date { get; }
        public decimal FirstValue { get; }
        public decimal SecondValue { get; }

        public override string ToString() =>
            $"{Id,-2} {Date,-25} {FirstValue,-7} {SecondValue,-13}";
    }
}