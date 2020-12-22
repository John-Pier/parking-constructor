using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.utils.distributions;

namespace ParkingConstructorLib.logic
{
    public class SettingsModel
    {
        public IDistributionLaw GenerationStreamDistribution  {get; private set;}
        public IDistributionLaw ParkingTimeDistribution {get; private set;}
        public int DayTimeRate { get; private set;}
        public int NightTimeRate { get; private set;}
        public double EnteringProbability { get; private set;}

        public SettingsModel()
        {

        }

        public void SetGenerationStreamDistribution(IDistributionLaw distributionLaw)
        {
            GenerationStreamDistribution = distributionLaw;
        }

        public void GetParkingTimeDistribution(IDistributionLaw distributionLaw)
        {
            ParkingTimeDistribution = distributionLaw;
        }

        public void SetDayTimeRate(int rate)
        {
            DayTimeRate = rate;
        }

        public void SeNightTimeRate(int rate)
        {
            NightTimeRate = rate;
        }

        public void SetProbabilityOfEnteringToParking(double probability)
        {
            EnteringProbability = probability;
        }
    }
}
