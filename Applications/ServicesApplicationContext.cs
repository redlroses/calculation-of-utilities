using System.Collections.Generic;
using СalculationOfUtilities.Interpreters;
using СalculationOfUtilities.Services;

namespace СalculationOfUtilities.Applications
{
    public class ServicesApplicationContext : ApplicationContext
    {
        private const string DatabaseName = "Services";
        private const string TableName = "Services";

        private readonly Dictionary<ServiceType, Service> _services;

        public ServicesApplicationContext() : base(DatabaseName)
        {
            bool hasRows = TryRead(TableName, out ServicesInterpreter servicesInterpreter);

            if (hasRows == false)
            {
                throw new System.NullReferenceException(nameof(servicesInterpreter.Services));
            }

            _services = new Dictionary<ServiceType, Service>
            {
                [ServiceType.ColdWater] = servicesInterpreter.CreateColdWaterService(),
                [ServiceType.HotWater] = servicesInterpreter.CreateHotWaterService(),
                [ServiceType.Electricity] = servicesInterpreter.CreateElectricityService(),
            };
        }

        public ICollection<Service> Services => _services.Values;
        public ColdWaterService ColdWaterService => (ColdWaterService) GetService(ServiceType.ColdWater);
        public HotWaterService HotWaterService => (HotWaterService) GetService(ServiceType.HotWater);
        public ElectricityService ElectricityService => (ElectricityService) GetService(ServiceType.Electricity);

        private Service GetService(ServiceType byType)
        {
            if (_services.TryGetValue(byType, out Service service))
            {
                return service;
            }

            throw new KeyNotFoundException(nameof(byType));
        }
    }
}
