namespace ParkingConstructorLib.utils.distributions
{
    public class DeterminedDistribution : IDistributionLaw
    {
        private readonly double constNumber;

        public DeterminedDistribution(double constNumber)
        {
            this.constNumber = constNumber;
        }

        public double GetRandNumber()
        {
            return constNumber;
        }
    }
}
