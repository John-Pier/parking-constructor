using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.services;
using ParkingConstructorLib.utils.distributions;

namespace ParkingConstructorLib.logic
{
    public class SettingsModel
    {
        public IDistributionLaw GenerationStreamDistribution  {get; private set;}
        public IDistributionLaw ParkingTimeDistribution {get; private set;}

        // TODO: Добавить ограничения на параметры
        public int DayTimeRate { get; private set;}
        public int NightTimeRate { get; private set;}
        public int PercentOfTrack { get; private set;}
        public int PercentOfCar { get; private set;}
        public double EnteringProbability { get; private set;}

        public readonly SettingModelService SettingService = new SettingModelService();
        
        public SettingsModel()
        {
            DayTimeRate = 80;
            NightTimeRate = 120;
            EnteringProbability = 0.5;

            SetPercentOfTrack(50);
        }

        public bool IsModelValid()
        {
            return GenerationStreamDistribution != null
                && ParkingTimeDistribution != null;
        }

        public void SetGenerationStreamDistribution(IDistributionLaw distributionLaw)
        {
            GenerationStreamDistribution = distributionLaw;
        }

        public void SetParkingTimeDistribution(IDistributionLaw distributionLaw)
        {
            ParkingTimeDistribution = distributionLaw;
        }

        public void SetDayTimeRate(int rate)
        {
            if (SettingService.CheckDayTimeRate(rate))
            {
                DayTimeRate = rate;
            }
        }

        public void SetNightTimeRate(int rate)
        {
            if (SettingService.CheckNightTimeRate(rate))
            {
                NightTimeRate = rate;
            }
        }

        public void SetProbabilityOfEnteringToParking(double probability)
        {
            EnteringProbability = probability;
        }

        public void SetPercentOfTrack(int percents)
        {
            if (percents <= 100 && percents >= 0)
            {
                PercentOfTrack = percents;
                PercentOfCar = 100 - PercentOfTrack;
            }
        }

        public int getDayTimeRate()
        {
            return DayTimeRate;
        }

        public int getNightTime()
        {
            return NightTimeRate;
        }
    }
}
