using System;

namespace СalculationOfUtilities.View
{
    public class ConsoleReader : Reader
    {
        public ConsoleReader(char yesAnswer, char noAnswer) : base(yesAnswer, noAnswer) { }

        protected override string ReadLine() =>
            Console.ReadLine();
    }
}