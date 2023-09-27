using System.Collections.Generic;
using СalculationOfUtilities.Data;

namespace СalculationOfUtilities.Services.Interfaces
{
    public interface ISingleRateService : IService
    {
        ICollection<ResourceCost> CalculateCost(Reading reading);
    }
}