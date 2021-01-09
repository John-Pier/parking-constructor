namespace ParkingConstructorLib.services
{
    public class SettingModelService
    {
        public static readonly int MinDayTimeRate = 1;
        public static readonly int MaxDayTimeRate = 500;

        public static readonly int MinNightTimeRate = 1;
        public static readonly int MaxNightTimeRate = 500;

        // Minutes
        
        public static readonly double MinGenerationDeterminedDistributionValue = 1d/6d; // 10 s
        public static readonly double MaxGenerationDeterminedDistributionValue = 60d; // 3600 s
        
        public static readonly double MinGenerationUniformDistributionValue = 1d/6d; // 10 s
        public static readonly double MaxGenerationUniformDistributionValue = 120d; // 2 h
        
        public static readonly double MinGenerationNormalDistributionMValue = 1d/6d; // 10 s
        public static readonly double MaxGenerationNormalDistributionMValue = 120d; // 2 h
        
        public static readonly double MinGenerationNormalDistributionDValue = 1d/60d; // 1 s
        public static readonly double MaxGenerationNormalDistributionDValue = 10d; // 600 s
        
        // Hours
        
        public static readonly double MinParkingTimeDeterminedDistributionValue = 1d/6d;
        public static readonly double MaxParkingTimeDeterminedDistributionValue = 60d;
        
        public static readonly double MinParkingTimeUniformDistributionValue = 1d/6d; // 10 s
        public static readonly double MaxParkingTimeUniformDistributionValue = 7200d; // 2 days
        
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