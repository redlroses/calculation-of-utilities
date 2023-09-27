using System.Collections.Generic;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Services;

namespace СalculationOfUtilities.View
{
    public abstract class Printer
    {
        private const string ServiceName = "Усулуга";
        private const string Consumption = "Потребление";
        private const string MeasureUnit = "Ед. изм.";
        private const string Price = "Стоймость, руб";
        private const string LookupTableHeader = "Справочная таблица";
        private const string ConfigurationDescription = "Установлены ли счётчики {0}?";
        private const string ConfigurationHeader = "Процесс первичной настройки.\n Пожалуйста, ответьте на вопросы ниже:";
        private const string YesNoQuestion = "Введите \"{0}\", если хотите ответить - \"Да\"; введите \"{1}\", если хотите ответить - \"Нет\".";
        private const string CurrentReadingsDescription = "Введите текущие показания прибора учёта {0}: ";
        private const string CurrentReadingsFirstRateDescription = "Введите текущие показания дневного тарифа прибора учёта {0}: ";
        private const string CurrentReadingsSecondRateDescription = "Введите текущие показания ночного тарифа прибора учёта {0}: ";
        private const string ResidentNumberDescription = "Введите количество проживающих в помещении: ";
        private const string InvalidReadingWarning = "Предупреждение! Текущие показания не могу быть меньше прошлых. Повторите попытку.";
        private const string ResetOffer = "Сбросить настройки?";
        private const string ConfigurationWasReset = "Настройки были успешно сброшены";
        private const string Total = "Итого";
        private const string RepeatOffer = "Продолжить вычисления на основе показаний слудующего месяца?";

        private const string TableFormat = "{0, -20}{1, 15}{2, 10}{3, 15}";

        private readonly Dictionary<ServiceType, string> _configurationServiceNames = new Dictionary<ServiceType, string>
        {
            [ServiceType.ColdWater] = "Системы холодного вобоснабжения",
            [ServiceType.HotWater] = "Системы горячего вобоснабжения",
            [ServiceType.Electricity] = "Электроэнергии",
        };

        private readonly char _yesSymbol;
        private readonly char _noSymbol;

        protected Printer(char yesSymbol, char noSymbol)
        {
            _yesSymbol = yesSymbol;
            _noSymbol = noSymbol;
        }

        protected abstract void PrintLine(string output);

        protected abstract void Print(string output);

        protected abstract void PrintLine();

        public void PrintResourcePriceHeader()
        {
            string output = string.Format(TableFormat, ServiceName, Consumption, MeasureUnit, Price);

            PrintLine();
            PrintLine(output);
        }

        public void PrintResidentsNumberForm() =>
            Print(ResidentNumberDescription);

        public void PrintMeasurementSubmissionForm(ServiceType type) =>
            Print(string.Format(CurrentReadingsDescription, _configurationServiceNames[type]));

        public void PrintFirstRateMeasurementSubmissionForm(ServiceType type) =>
            Print(string.Format(CurrentReadingsFirstRateDescription, _configurationServiceNames[type]));

        public void PrintSecondRateMeasurementSubmissionForm(ServiceType type) =>
            Print(string.Format(CurrentReadingsSecondRateDescription, _configurationServiceNames[type]));

        public void PrintResourcePrice(ICollection<ResourceCost> priceList)
        {
            decimal total = default;

            foreach (var resourceCost in priceList)
            {
                PrintResourcePrice(resourceCost);
                total += resourceCost.Cost;
            }

            PrintTotal(total);
            PrintLine();
        }

        private void PrintTotal(decimal total) =>
            Print(string.Format(TableFormat, Total, "", "", total));

        public void PrintLookupTable(IEnumerable<string> serviceInfo)
        {
            PrintLine(LookupTableHeader);

            foreach (string info in serviceInfo)
            {
                PrintLine(info);
            }

            PrintLine();
        }

        public void PrintInvalidReadingWarning() =>
            PrintLine(InvalidReadingWarning);

        public void PrintConfigurationFor(ServiceType serviceType)
        {
            PrintLine(string.Format(ConfigurationDescription, _configurationServiceNames[serviceType]));
            PrintYesNoQuestion();
        }

        public void PrintConfigurationHeader() =>
            PrintLine(ConfigurationHeader);

        public void PrintYesNoQuestion() =>
            PrintLine(string.Format(YesNoQuestion, _yesSymbol, _noSymbol));

        public void PrintResetOffer() =>
            PrintLine(ResetOffer);

        public void PrintConfigurationWasReset() =>
            PrintLine(ConfigurationWasReset);

        public void PrintRepeatOffer()
        {
            PrintLine(RepeatOffer);
            PrintYesNoQuestion();
        }

        private void PrintResourcePrice(ResourceCost resourcePrice)
        {
            string output = string.Format(TableFormat, resourcePrice.Resource.Name,
                resourcePrice.Consumption, resourcePrice.Resource.MeasureUnit, resourcePrice.Cost);

            PrintLine(output);
        }
    }
}
