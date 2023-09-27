using Microsoft.Data.Sqlite;
using СalculationOfUtilities.Constants;

namespace СalculationOfUtilities.Interpreters
{
    public class ReadingConfig : ISqliteDataInterpreter
    {
        private int _id;

        public bool HasColdWaterMeter { get; private set; }
        public bool HasHotWaterMeter { get; private set; }
        public bool HasElectricityMeter { get; private set; }

        bool ISqliteDataInterpreter.Interpreter(SqliteDataReader reader)
        {
            if (reader.HasRows == false)
            {
                return false;
            }

            reader.Read();

            _id = reader.GetInt32(MeterReadingsIndices.ConfigId);
            HasColdWaterMeter = reader.GetBoolean(MeterReadingsIndices.ColdWaterConfig);
            HasHotWaterMeter = reader.GetBoolean(MeterReadingsIndices.HotWaterConfig);
            HasElectricityMeter = reader.GetBoolean(MeterReadingsIndices.ElectricityConfig);

            return true;
        }

        public override string ToString() =>
            $"id: {_id}, HasColdWaterMeter: {HasColdWaterMeter}, HasHotWaterMeter: {HasHotWaterMeter}, HasElectricityWaterMeter: {HasElectricityMeter}";
    }
}