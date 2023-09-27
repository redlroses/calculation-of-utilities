using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using СalculationOfUtilities.Interpreters;
using СalculationOfUtilities.Services;

namespace СalculationOfUtilities.Applications
{
    public class MeterReadingsApplicationContext : ApplicationContext
    {
        private const string DatabaseName = "MetterReadings";
        private const string ColdWaterReadingsName = "ColdWater";
        private const string HotWaterReadingsName = "HotWater";
        private const string ElectricityReadingsName = "Electricity";
        private const string ReadingsConfigName = "Сonfig";
        private const string ConfigIdName = "id";
        private const string DateName = "date";
        private const string FirstValueName = "firstValue";
        private const string SecondValueName = "secondValue";

        private const int LastSymbolCount = 2;
        private const int ConfigId = 1;


        private readonly Dictionary<ServiceType, string> _readingsTableNames = new Dictionary<ServiceType, string>
        {
            [ServiceType.ColdWater] = ColdWaterReadingsName,
            [ServiceType.HotWater] = HotWaterReadingsName,
            [ServiceType.Electricity] = ElectricityReadingsName,
        };

        private readonly StringBuilder _stringBuilder;

        public MeterReadingsApplicationContext() : base(DatabaseName)
        {
            _stringBuilder = new StringBuilder();
        }

        public bool TryGetConfig(out ReadingConfig config) =>
            TryRead(ReadingsConfigName, out config);

        public bool TryGetReadings(ServiceType byType, out ReadingInterpreter readingInterpreter) =>
            TryRead(_readingsTableNames[byType], out readingInterpreter);

        public void AddReading(ServiceType byType, DateTime dateTime, decimal first, decimal second)
        {
            string names = CreateNamesString(DateName, FirstValueName, SecondValueName);
            string values = CreateValuesString(DateName, FirstValueName, SecondValueName);

            List<SqliteParameter> sqliteParameters = new List<SqliteParameter>
            {
                CreateInsertParameter(DateName, dateTime),
                CreateInsertParameter(FirstValueName, first),
                CreateInsertParameter(SecondValueName, second)
            };

            Add(_readingsTableNames[byType], names, values, sqliteParameters.ToArray());
        }

        public void AddReading(ServiceType byType, DateTime dateTime, decimal first)
        {
            string names = CreateNamesString(DateName, FirstValueName);
            string values = CreateValuesString(DateName, FirstValueName);

            List<SqliteParameter> sqliteParameters = new List<SqliteParameter>
            {
                CreateInsertParameter(DateName, dateTime),
                CreateInsertParameter(FirstValueName, first),
            };

            Add(_readingsTableNames[byType], names, values, sqliteParameters.ToArray());
        }

        public void ResetConfig()
        {
            SqliteParameter sqliteParameter = CreateInsertParameter(ConfigIdName, ConfigId);
            Delete(ReadingsConfigName, ConfigIdName, sqliteParameter);
        }

        public void CreateConfig(bool hasColdWaterMeter, bool hasHotWaterMeter, bool hasElectricityMeter)
        {
            string names = CreateNamesString(nameof(hasColdWaterMeter), nameof(hasHotWaterMeter),
                nameof(hasElectricityMeter));
            string values = CreateValuesString(nameof(hasColdWaterMeter), nameof(hasHotWaterMeter),
                nameof(hasElectricityMeter));

            List<SqliteParameter> sqliteParameters = new List<SqliteParameter>
            {
                CreateInsertParameter(ConfigIdName, ConfigId),
                CreateInsertParameter(nameof(hasColdWaterMeter), hasColdWaterMeter),
                CreateInsertParameter(nameof(hasHotWaterMeter), hasHotWaterMeter),
                CreateInsertParameter(nameof(hasElectricityMeter), hasElectricityMeter)
            };

            Add(ReadingsConfigName, names, values, sqliteParameters.ToArray());
        }

        private string CreateValuesString(params string[] values)
        {
            _stringBuilder.Clear();

            foreach (string word in values)
            {
                _stringBuilder.Append('@').Append(word).Append(", ");
            }

            _stringBuilder.Remove(_stringBuilder.Length - LastSymbolCount, LastSymbolCount);

            return _stringBuilder.ToString();
        }

        private string CreateNamesString(params string[] name)
        {
            _stringBuilder.Clear();
            return _stringBuilder.AppendJoin(", ", name).ToString();
        }

        private SqliteParameter CreateInsertParameter(string name, bool value) =>
            new SqliteParameter($@"{name}", value);

        private SqliteParameter CreateInsertParameter(string name, int value) =>
            new SqliteParameter($@"{name}", value);

        private SqliteParameter CreateInsertParameter(string name, DateTime value) =>
            new SqliteParameter($@"{name}", value);

        private SqliteParameter CreateInsertParameter(string name, decimal value) =>
            new SqliteParameter($@"{name}", value);
    }
}

