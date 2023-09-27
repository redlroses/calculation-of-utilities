using System;

namespace СalculationOfUtilities.View
{
    public class ConsolePrinter : Printer
    {
        public ConsolePrinter(char yesSymbol, char noSymbol) : base(yesSymbol, noSymbol) { }

        protected override void PrintLine(string output) =>
            Console.WriteLine(output);

        protected override void Print(string output) =>
            Console.Write(output);

        protected override void PrintLine() =>
            Console.WriteLine();
    }
}