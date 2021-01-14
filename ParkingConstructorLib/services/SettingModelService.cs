namespace ParkingConstructorLib.services
{
    public class SettingModelService
    {
        public static readonly int MinDayTimeRate = 1;
        public static readonly int MaxDayTimeRate = 500;

        public static readonly int MinNightTimeRate = 1;
        public static readonly int MaxNightTimeRate = 500;

        // Minutes
        
        public static readonly double MinGenerationDeterminedDistributionValue = 1d/2d; // 30 s
        public static readonly double MaxGenerationDeterminedDistributionValue = 60d; // 3600 s
        
        public static readonly double MinGenerationUniformDistributionValue = 1d/2d; // 30 s
        public static readonly double MaxGenerationUniformDistributionValue = 120d; // 2 h
        
        public static readonly double MinGenerationNormalDistributionMValue = 1d/2d; // 30 s
        public static readonly double MaxGenerationNormalDistributionMValue = 120d; // 2 h
        
        public static readonly double MinGenerationNormalDistributionDValue = 1d/60d; // 1 s
        public static readonly double MaxGenerationNormalDistributionDValue = 10d; // 600 s
        
        public static readonly double MinGenerationExponentialDistributionValue = 0.0002;
        public static readonly double MaxGenerationExponentialDistributionValue = 0.1;
        
        // Hours
        
        public static readonly double MinParkingTimeDeterminedDistributionValue = 1d/6d; // 10 min
        public static readonly double MaxParkingTimeDeterminedDistributionValue = 48d; // 2 days 
        
        public static readonly double MinParkingTimeUniformDistributionValue = 1/6d; // 10 min
        public static readonly double MaxParkingTimeUniformDistributionValue = 48d; // 2 days 

        public static readonly double MinParkingTimeNormalDistributionMValue = 1d/6d; // 10 min
        public static readonly double MaxParkingTimeNormalDistributionMValue = 48d; // 2 days
        
        public static readonly double MinParkingTimeNormalDistributionDValue = 1d/6d; // 10 min
        public static readonly double MaxParkingTimeNormalDistributionDValue = 24d; // 1 day
        
        // Unused
        public static readonly double MinParkingTimeExponentialDistributionValue = 0.0002;
        public static readonly double MaxParkingTimeExponentialDistributionValue = 0.1;
        
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
        
        public bool CheckGenerationUniformDistributionValue(double value)
        {
            return value >= MinGenerationUniformDistributionValue &&
                   value <= MaxGenerationUniformDistributionValue;
        }
        
        public bool CheckParkingTimeDistributionValue(double value)
        {
            return value >= MinParkingTimeDeterminedDistributionValue &&
                   value <= MaxParkingTimeDeterminedDistributionValue;
        }
    }
}