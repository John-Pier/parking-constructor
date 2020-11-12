using System;

namespace ParkingConstructorLib.utils.distributions
{
    public class ExponentialDistribution : IDistributionLaw
    {
        private readonly double lambda;
        private readonly Random random = new Random();

        public ExponentialDistribution(double lambda)
        {
            if (lambda <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.lambda = lambda;
        }

        public double GetRandNumber()
        {
            double y = random.NextDouble();
            return -(1 / lambda) * Math.Log(y);
        }
    }
}
