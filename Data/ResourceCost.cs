namespace СalculationOfUtilities.Data
{
    public struct ResourceCost
    {
        public ResourceCost(Resource resource, decimal consumption, decimal cost)
        {
            Resource = resource;
            Consumption = consumption;
            Cost = cost;
        }

        public Resource Resource { get; }
        public decimal Consumption { get; }
        public decimal Cost { get; }
    }
}
