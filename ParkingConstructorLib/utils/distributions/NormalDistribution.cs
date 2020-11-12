using System;

namespace GasStationMs.App.DistributionLaws
{
    public class NormalDistribution : IDistributionLaw
    {
        private readonly double expectedValue;
        private readonly double variance;
        private readonly Random random = new Random();

        public NormalDistribution(double expectedValue, double variance)
        {
            this.expectedValue = expectedValue;
            this.variance = variance;
        }

        public double GetRandNumber()
        {
            double sum = 0;
            for (var i = 0; i <= 12; i++)
                sum += random.NextDouble();

            return Math.Abs(Math.Round((expectedValue + variance * (sum - 6)), 2));
        }
    }
}
