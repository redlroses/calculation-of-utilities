using System.Collections.Generic;
using СalculationOfUtilities.Data;

namespace СalculationOfUtilities.Services.Interfaces
{
    public interface IDoubleRateService : IService
    {
        ICollection<ResourceCost> CalculateCost(Reading dayReading, Reading nightReading);
    }
}