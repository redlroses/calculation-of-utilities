using System.Collections.Generic;
using СalculationOfUtilities.Data;

namespace СalculationOfUtilities.Services.Interfaces
{
    public interface IStandardService : IService
    {
        ICollection<ResourceCost> CalculateCost(int residentCount);
    }
}