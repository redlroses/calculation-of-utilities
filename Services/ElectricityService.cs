using System.Collections.Generic;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Services.Interfaces;

namespace СalculationOfUtilities.Services
{
    public class ElectricityService : Service, IDoubleRateService, IStandardService
    {
        private const string ServiceName = "Электричество";

        private readonly Resource _defaultResource;
        private readonly Resource _dayResource;
        private readonly Resource _nightResource;

        public ElectricityService(Resource defaultResource, Resource dayResource, Resource nightResource) : base(ServiceType.Electricity)
        {
            _defaultResource = defaultResource;
            _dayResource = dayResource;
            _nightResource = nightResource;
        }

        public override string ToString() =>
            $"\nУслуга: {ServiceName} \n {_defaultResource} \n {_dayResource} \n {_nightResource}";

        public ICollection<ResourceCost> CalculateCost(int residentCount)
        {
            decimal consumption = GetConsumption(residentCount, _defaultResource);
            decimal cost = GetCost(consumption, _defaultResource);
            return CreatePriceList(new ResourceCost(_defaultResource, consumption, cost));
        }

        public ICollection<ResourceCost> CalculateCost(Reading dayReading, Reading nightReading)
        {
            decimal dayConsumption = GetConsumption(dayReading);
            decimal nightConsumption = GetConsumption(nightReading);

            decimal dayCost = GetCost(dayConsumption, _dayResource);
            decimal nightCost = GetCost(nightConsumption, _nightResource);

            return CreatePriceList(
                new ResourceCost(_dayResource, dayConsumption, dayCost),
                new ResourceCost(_nightResource, nightConsumption, nightCost));
        }
    }
}