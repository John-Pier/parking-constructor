namespace ParkingConstructorLib.services
{
    public class SettingModelService
    {
        public const int MinDayTimeRate = 1;
        public const int MaxDayTimeRate = 500;

        public const int MinNightTimeRate = 1;
        public const int MaxNightTimeRate = 500;

        
        public const double MinGenerationDeterminedDistributionValue = 1d/6d; // 10 s
        public const double MaxGenerationDeterminedDistributionValue = 60d; // 3600 s
        
        public const double MinGenerationUniformDistributionValue = 1d/6d; // 10 s
        public const double MaxGenerationUniformDistributionValue = 60d; // 3600 s
        
        public const double MinParkingTimeDeterminedDistributionValue = 1d/6d;
        public const double MaxParkingTimeDeterminedDistributionValue = 60d;
        
        public const double MinParkingTimeUniformDistributionValue = 1d/6d; // 10 s
        public const double MaxParkingTimeUniformDistributionValue = 7200d; // 2 days
        
        public bool CheckDayTimeRate(int rate)
        {
            return rate >= MinDayTimeRate && rate <= MaxDayTimeRate;
        }
        
        public bool CheckNightTimeRate(int rate)
        {
            return rate >= MinNightTimeRate && rate <= MaxNightTimeRate;
        }

        public bool CheckGenerationDeterminedDistributionValue(double value)
        {
            return value >= MinGenerationDeterminedDistributionValue &&
                   value <= MaxGenerationDeterminedDistributionValue;
        }
        
        public bool CheckParkingTimeDistributionValue(double value)
        {
            return value >= MinParkingTimeDeterminedDistributionValue &&
                   value <= MaxParkingTimeDeterminedDistributionValue;
        }

        public bool CheckGenerationUniformDistributionValue(double value)
        {
            return value >= MinGenerationUniformDistributionValue &&
                   value <= MaxGenerationUniformDistributionValue;
        }
    }
}