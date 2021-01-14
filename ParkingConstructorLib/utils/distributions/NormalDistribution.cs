using System;

namespace ParkingConstructorLib.utils.distributions
{
    public class NormalDistribution : IDistributionLaw
    {
        private readonly double expectedValue;
        private readonly double variance;
        private readonly Random random = new Random();

        private const int tryingCount = 100;
        private const int precisionCount = 12;

        public NormalDistribution(double expectedValue, double variance)
        {
            this.expectedValue = expectedValue;
            this.variance = variance;
        }

        public double GetRandNumber()
        {
            var i = 0;
            while (i < tryingCount)
            {
                double sum = 0;
                for (var j = 0; j <= precisionCount; j++)
                {
                    sum += random.NextDouble();
                }
                var value = Math.Abs(Math.Round(expectedValue + variance * (sum - 6), 2));
                if (value > 0) return value;
                i++;
            }

            return expectedValue;
        }
    }
}
