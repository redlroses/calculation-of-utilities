using System;

namespace СalculationOfUtilities.Interpreters.Data
{
    public readonly struct SingleReadingData : IReadingData
    {
        public SingleReadingData(int id, DateTime date, decimal firstValue = 0)
        {
            Id = id;
            Date = date;
            FirstValue = firstValue;
        }

        public int Id { get; }
        public DateTime Date { get; }
        public decimal FirstValue { get; }

        public override string ToString() =>
            $"{Id,-2} {Date,-25} {FirstValue,-7}";
    }
}