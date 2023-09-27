using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using СalculationOfUtilities.Constants;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Services;

namespace СalculationOfUtilities.Interpreters
{
    public class ServicesInterpreter : ISqliteDataInterpreter
    {
        private List<ServiceConfig> _serviceConfigs;

        public IReadOnlyCollection<ServiceConfig> Services => _serviceConfigs;

        bool ISqliteDataInterpreter.Interpreter(SqliteDataReader reader)
        {
            _serviceConfigs = new List<ServiceConfig>();

            if (reader.HasRows == false)
            {
                return false;
            }

            while (reader.Read())
            {
                int id = reader.GetInt32(ServiceIndices.IdIndex);
                string name = reader.GetString(ServiceIndices.NameIndex);
                decimal rate = reader.GetDecimal(ServiceIndices.RateIndex);
                decimal standard = reader.GetDecimal(ServiceIndices.StandardIndex);
                string unit = reader.GetString(ServiceIndices.UnitIndex);

                _serviceConfigs.Add(new ServiceConfig(id, name, rate, standard, unit));
            }

            return true;
        }

        public ColdWaterService CreateColdWaterService()
        {
            Resource coldWaterResource = CreateResource(_serviceConfigs[ServiceIndices.ColdWaterIndex]);
            return new ColdWaterService(coldWaterResource);
        }

        public HotWaterService CreateHotWaterService()
        {
            Resource heatCarrierResource = CreateResource(_serviceConfigs[ServiceIndices.HeatCarrierIndex]);
            Resource thermalEnergyResource = CreateResource(_serviceConfigs[ServiceIndices.ThermalEnergyIndex]);
            return new HotWaterService(heatCarrierResource, thermalEnergyResource);
        }

        public ElectricityService CreateElectricityService()
        {
            Resource defaultResource = CreateResource(_serviceConfigs[ServiceIndices.DefaultElectricityIndex]);
            Resource dayResource = CreateResource(_serviceConfigs[ServiceIndices.DayElectricityIndex]);
            Resource nightResource = CreateResource(_serviceConfigs[ServiceIndices.NightElectricityIndex]);
            return new ElectricityService(defaultResource, dayResource, nightResource);
        }

        private Resource CreateResource(ServiceConfig fromData) =>
            new Resource(fromData.Name, fromData.MeasureUnit, fromData.Rate, fromData.Standard);
    }
}
