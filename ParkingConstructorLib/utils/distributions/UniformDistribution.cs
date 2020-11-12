using System;

namespace GasStationMs.App.DistributionLaws
{
    public class UniformDistribution : IDistributionLaw
    {
        private readonly double a;
        private readonly double b;
        private readonly Random random = new Random();

        public UniformDistribution(double a, double b)
        {
            if (a > b)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.a = a;
            this.b = b;
        }

        public double GetRandNumber()
        {
            return random.NextDouble() * (b - a) + a;
        }
    }
}
