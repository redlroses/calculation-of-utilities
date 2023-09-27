namespace СalculationOfUtilities.View
{
    public abstract class Reader
    {
        private readonly char _yesAnswer;
        private readonly char _noAnswer;

        protected Reader(char yesAnswer, char noAnswer)
        {
            _yesAnswer = yesAnswer;
            _noAnswer = noAnswer;
        }

        protected abstract string ReadLine();

        public bool TryReadYesNoQuestion(out bool answer)
        {
            answer = false;

            if (char.TryParse(ReadLine(), out char symbol) == false)
            {
                return false;
            }

            if (symbol.Equals(_yesAnswer))
            {
                answer = true;
                return true;
            }

            if (symbol.Equals(_noAnswer))
            {
                answer = false;
                return true;
            }

            return false;
        }

        public bool TryReadPositive(out int number)
        {
            if (int.TryParse(ReadLine(), out number) == false)
            {
                return false;
            }

            if (number < 0)
            {
                return false;
            }

            return true;
        }

        public bool TryReadPositive(out decimal number)
        {
            if (decimal.TryParse(ReadLine(), out number) == false)
            {
                return false;
            }

            if (number < 0)
            {
                return false;
            }

            return true;
        }
    }
}