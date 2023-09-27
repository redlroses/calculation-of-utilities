using System;

namespace СalculationOfUtilities.Interpreters.Data
{
    public interface IReadingData
    {
        public int Id { get; }
        public DateTime Date { get; }
    }
}