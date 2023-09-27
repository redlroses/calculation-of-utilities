using System;
using System.Collections.Generic;
using System.Linq;
using СalculationOfUtilities.Applications;
using СalculationOfUtilities.Data;
using СalculationOfUtilities.Interpreters;
using СalculationOfUtilities.Interpreters.Data;
using СalculationOfUtilities.Services;
using СalculationOfUtilities.Services.Interfaces;
using СalculationOfUtilities.View;

namespace СalculationOfUtilities
{
    public sealed class Calculation
    {
        private const char YesAnswer = 'Д';
        private const char NoAnswer = 'Н';

        private readonly ConsolePrinter _printer;
        private readonly ConsoleReader _reader;
        private readonly ServicesApplicationContext _servicesApplication;
        private readonly MeterReadingsApplicationContext _meterReadingsApplication;
        private readonly List<ResourceCost> _priceList;

        private ReadingConfig _config;

        private Func<ICollection<ResourceCost>> _coldWaterCalculation;
        private Func<ICollection<ResourceCost>> _hotWaterCalculation;
        private Func<ICollection<ResourceCost>> _electricityCalculation;

        private int _residentsCount;

        public Calculation()
        {
            _printer = new ConsolePrinter(YesAnswer, NoAnswer);
            _reader = new ConsoleReader(YesAnswer, NoAnswer);
            _meterReadingsApplication = new MeterReadingsApplicationContext();
            _servicesApplication = new ServicesApplicationContext();
            _priceList = new List<ResourceCost>(5);
        }

        private ISingleRateService ColdWater => _servicesApplication.ColdWaterService;
        private ISingleRateService HotWater => _servicesApplication.HotWaterService;
        private IDoubleRateService Electricity => _servicesApplication.ElectricityService;

        public void BeginCalculation()
        {
            ShowLookupTable();

            bool hasConfig = TryGetConfig();

            if (hasConfig == false)
            {
                BeginConfiguration();
            }
            else
            {
                OfferToResetSettings();
            }

            ApplyConfiguration();
            BeginCalculationCycle();
        }

        private void BeginCalculationCycle()
        {
            do
            {
                MainCalculation();
            } while (IsRepeat());
        }

        private bool IsRepeat()
        {
            _printer.PrintRepeatOffer();
            return GetYesNoAnswer();
        }

        private void MainCalculation()
        {
            _residentsCount = GetResidentsCount();

            _priceList.Clear();
            _priceList.AddRange(_coldWaterCalculation.Invoke());
            _priceList.AddRange(_hotWaterCalculation.Invoke());
            _priceList.AddRange(_electricityCalculation.Invoke());

            _printer.PrintResourcePriceHeader();
            _printer.PrintResourcePrice(_priceList);
        }

        private void OfferToResetSettings()
        {
            _printer.PrintResetOffer();
            _printer.PrintYesNoQuestion();

            if (GetYesNoAnswer())
            {
                _meterReadingsApplication.ResetConfig();
                _printer.PrintConfigurationWasReset();
                BeginConfiguration();
            }
        }

        private ICollection<ResourceCost> CalculateByStandard(IStandardService service) =>
            service.CalculateCost(_residentsCount);

        private ICollection<ResourceCost> CalculateByRate(ISingleRateService service)
        {
            SingleReadingData lastReadingsData = GetLastReadingsData<SingleReadingData>(service.Type);

            _printer.PrintMeasurementSubmissionForm(service.Type);
            Reading reading = CreateReading(service, lastReadingsData.FirstValue);

            AddSingleRateReading(service, reading, lastReadingsData.Date.AddMonths(1));
            return service.CalculateCost(reading);
        }

        private ICollection<ResourceCost> CalculateByDoubleRate(IDoubleRateService service)
        {
            DoubleReadingData lastReadingsData = GetLastReadingsData<DoubleReadingData>(service.Type);

            _printer.PrintFirstRateMeasurementSubmissionForm(service.Type);
            Reading dayReading = CreateReading(service, lastReadingsData.FirstValue);

            _printer.PrintSecondRateMeasurementSubmissionForm(service.Type);
            Reading nightReading = CreateReading(service, lastReadingsData.SecondValue);

            AddDoubleRateReading(service, dayReading, nightReading, lastReadingsData.Date.AddMonths(1));
            return service.CalculateCost(dayReading, nightReading);
        }

        private void AddSingleRateReading(IService service, Reading dayReading, DateTime date) =>
            _meterReadingsApplication.AddReading(service.Type, date, dayReading.Current);

        private void AddDoubleRateReading(IService service, Reading dayReading, Reading nightReading, DateTime date) =>
            _meterReadingsApplication.AddReading(service.Type, date, dayReading.Current,
                nightReading.Current);

        private Reading CreateReading(IService service, decimal pastReading)
        {
            decimal currentReading = GetValidatedReading(pastReading, service.Type);
            Reading reading = new Reading(pastReading, currentReading);
            return reading;
        }

        private T GetLastReadingsData<T>(ServiceType serviceType) where T : IReadingData, new()
        {
            _meterReadingsApplication.TryGetReadings(serviceType, out ReadingInterpreter readingInterpreter);
            return readingInterpreter.GetLast<T>();
        }

        private int GetResidentsCount()
        {
            _printer.PrintResidentsNumberForm();
            int number;

            while (_reader.TryReadPositive(out number) == false)
            {
                _printer.PrintResidentsNumberForm();
            }

            return number;
        }

        private decimal GetValidatedReading(decimal pastReading, ServiceType type)
        {
            decimal currentReading = GetCurrentReading(type);

            while (currentReading < pastReading)
            {
                _printer.PrintInvalidReadingWarning();
                currentReading = GetCurrentReading(type);
            }

            return currentReading;
        }

        private decimal GetCurrentReading(ServiceType type)
        {
            decimal currentReading;

            while (_reader.TryReadPositive(out currentReading) == false)
            {
                _printer.PrintMeasurementSubmissionForm(type);
            }

            return currentReading;
        }

        private void ApplyConfiguration()
        {
            ConfigurateCalculationForService(
                ref _coldWaterCalculation, _config.HasColdWaterMeter, ColdWater);
            ConfigurateCalculationForService(
                ref _hotWaterCalculation, _config.HasHotWaterMeter, HotWater);
            ConfigurateCalculationForService(
                ref _electricityCalculation, _config.HasElectricityMeter, Electricity);
        }

        private void ConfigurateCalculationForService(ref Func<ICollection<ResourceCost>> funk, bool hasMeter, IService service)
        {
            if (hasMeter)
            {
                funk = service switch
                {
                    ISingleRateService singleRate => () => CalculateByRate(singleRate),
                    IDoubleRateService doubleRate => () => CalculateByDoubleRate(doubleRate),
                    _ => funk
                };
            }
            else
            {
                funk = () => CalculateByStandard(service as IStandardService);
            }
        }

        private bool TryGetConfig() =>
            _meterReadingsApplication.TryGetConfig(out _config);

        private void BeginConfiguration()
        {
            _printer.PrintConfigurationHeader();

            _meterReadingsApplication.CreateConfig(
                GetServiceConfig(ServiceType.ColdWater),
                GetServiceConfig(ServiceType.HotWater),
                GetServiceConfig(ServiceType.Electricity));

            TryGetConfig();
        }

        private bool GetServiceConfig(ServiceType serviceType)
        {
            _printer.PrintConfigurationFor(serviceType);
            return GetYesNoAnswer();
        }

        private void ShowLookupTable() =>
            _printer.PrintLookupTable(_servicesApplication.Services.Select(service => service.Info));

        private bool GetYesNoAnswer()
        {
            bool answer;

            while (_reader.TryReadYesNoQuestion(out answer) == false)
            {
                _printer.PrintYesNoQuestion();
            }

            return answer;
        }
    }
}