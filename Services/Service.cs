using System.Collections.Generic;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Services.Interfaces;

namespace СalculationOfUtilities.Services
{
    public abstract class Service : IService
    {
        private const int RoundDigits = 2;

        protected Service(ServiceType type)
        {
            Type = type;
        }

        public ServiceType Type { get; }

        public string Info => ToString();

        protected decimal GetConsumption(int personCount, Resource resource) =>
            resource.Standart * personCount;

        protected decimal GetConsumption(decimal byOtherStandard, Resource resource) =>
            byOtherStandard * resource.Standart;

        protected decimal GetConsumption(Reading reading) =>
            reading.Current - reading.Past;

        protected decimal GetCost(decimal consumption, Resource resource) =>
            decimal.Round(consumption * resource.Rate, RoundDigits);

        protected ICollection<ResourceCost> CreatePriceList(params ResourceCost[] resourcePrices) =>
            resourcePrices;
    }
}