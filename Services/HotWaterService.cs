using System.Collections.Generic;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Services.Interfaces;

namespace СalculationOfUtilities.Services
{
    public class HotWaterService : Service, ISingleRateService, IStandardService
    {
        private const string ServiceName = "Горячее водоснабжение";

        private readonly Resource _heatCarrierResource;
        private readonly Resource _thermalEnergyResource;

        public HotWaterService(Resource heatCarrierResource, Resource thermalEnergyResource) : base(ServiceType.HotWater)
        {
            _heatCarrierResource = heatCarrierResource;
            _thermalEnergyResource = thermalEnergyResource;
        }

        public override string ToString() =>
            $"\nУслуга: {ServiceName} \n {_heatCarrierResource} \n {_thermalEnergyResource}";

        public ICollection<ResourceCost> CalculateCost(int residentCount)
        {
            decimal heatCarrierConsumption = GetConsumption(residentCount, _heatCarrierResource);
            return CalculateOnHeatCarrierBase(heatCarrierConsumption);
        }

        public ICollection<ResourceCost> CalculateCost(Reading reading)
        {
            decimal heatCarrierConsumption = GetConsumption(reading);
            return CalculateOnHeatCarrierBase(heatCarrierConsumption);
        }

        private ICollection<ResourceCost> CalculateOnHeatCarrierBase(decimal heatCarrierConsumption)
        {
            decimal thermalEnergyConsumption = GetConsumption(heatCarrierConsumption, _thermalEnergyResource);

            decimal heatCarrierCost = GetCost(heatCarrierConsumption, _heatCarrierResource);
            decimal thermalEnergyCost = GetCost(thermalEnergyConsumption, _thermalEnergyResource);

            return CreatePriceList(
                new ResourceCost(_heatCarrierResource, heatCarrierConsumption, heatCarrierCost),
                new ResourceCost(_thermalEnergyResource, thermalEnergyConsumption, thermalEnergyCost));
        }
    }
}
