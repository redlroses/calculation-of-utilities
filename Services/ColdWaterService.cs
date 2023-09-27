using System.Collections.Generic;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Services.Interfaces;

namespace СalculationOfUtilities.Services
{
    public class ColdWaterService : Service, ISingleRateService, IStandardService
    {
        private const string ServiceName = "Холодное водоснабжение";

        private readonly Resource _coldWaterResource;

        public ColdWaterService(Resource coldWaterResource) : base(ServiceType.ColdWater)
        {
            _coldWaterResource = coldWaterResource;
        }

        public override string ToString() =>
            $"\nУслуга: {ServiceName} \n {_coldWaterResource}";

        public ICollection<ResourceCost> CalculateCost(int residentCount)
        {
            decimal consumption = GetConsumption(residentCount, _coldWaterResource);
            return CalculateOnConsumptionBase(consumption);
        }

        public ICollection<ResourceCost> CalculateCost(Reading reading)
        {
            decimal consumption = GetConsumption(reading);
            return CalculateOnConsumptionBase(consumption);
        }

        private ICollection<ResourceCost> CalculateOnConsumptionBase(decimal consumption)
        {
            decimal cost = GetCost(consumption, _coldWaterResource);
            return CreatePriceList(new ResourceCost(_coldWaterResource, consumption, cost));
        }
    }
}