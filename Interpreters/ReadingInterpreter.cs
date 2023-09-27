using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using СalculationOfUtilities.Constants;
using СalculationOfUtilities.Interpreters.Data;

namespace СalculationOfUtilities.Interpreters
{
    public class ReadingInterpreter : ISqliteDataInterpreter
    {
        private const int DoubleRateColumCount = 4;

        private List<IReadingData> _readingsData;

        public bool Interpreter(SqliteDataReader reader)
        {
            _readingsData = new List<IReadingData>();

            if (reader.HasRows == false)
            {
                return false;
            }

            bool isDoubleRated = reader.FieldCount == DoubleRateColumCount;

            while (reader.Read())
            {
                int id = reader.GetInt32(MeterReadingsIndices.IdIndex);
                DateTime date = reader.GetDateTime(MeterReadingsIndices.DateIndex);
                decimal firstValue = reader.GetDecimal(MeterReadingsIndices.FirstValue);

                if (isDoubleRated)
                {
                    decimal secondValue = reader.GetDecimal(MeterReadingsIndices.SecondValue);
                    _readingsData.Add(new DoubleReadingData(id, date, firstValue, secondValue));
                }
                else
                {
                    _readingsData.Add(new SingleReadingData(id, date, firstValue));
                }
            }

            return true;
        }

        public T GetLast<T>() where T : IReadingData, new()
        {
            if (_readingsData.Count > 0)
            {
                return (T) _readingsData.OrderBy(readingData => readingData.Date).Last();
            }

            if (typeof(SingleReadingData).IsAssignableFrom(typeof(T)))
            {
                IReadingData data = new SingleReadingData(0, DateTime.Today);
                return (T) data;
            }

            if (typeof(DoubleReadingData).IsAssignableFrom(typeof(T)))
            {
                IReadingData data = new DoubleReadingData(0, DateTime.Today);
                return (T) data;
            }

            throw new InvalidOperationException();
        }
    }
}