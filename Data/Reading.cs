namespace СalculationOfUtilities.Data
{
    public struct Reading
    {
        private const string InvalidReadingMessage = "Current reading must be grather then past reading and both grather then zero";

        public Reading(decimal past, decimal current)
        {
            if (past < 0 || current < 0 || past > current)
            {
                throw new System.ArgumentOutOfRangeException(nameof(current), InvalidReadingMessage);
            }

            Past = past;
            Current = current;
        }

        public decimal Past { get; }
        public decimal Current { get; }
    }
}
