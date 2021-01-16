using System;

namespace ParkingConstructorLib.logic
{
    public class StatisticModel
    {
        public DateTime StartDateTime;
        public DateTime EndDateTime;
        public double AverageNumberOfOccupiedPlaces;
        public double AveragePercentageOfOccupiedPlaces;
        public double FinalScope;
        public double AverageIncomePerDay;
        public double AverageIncomePerNight;

        /// <summary>
        /// Содержит общее чисто парковочных мест на карте
        /// </summary>
        public int ParkingPlaces;
        
        private int countOfSumBusyParkingPlaces = 0;
        private int countOfSetParkingPlaces = 0;
        private double dayDateTimeScope = 0;
        private double nightDateTimeScope = 0;
        
        private const int SkipSetParkingPlacesCount = 10;

        public StatisticModel()
        {
            ClearStatistic();
        }
        
        /// <summary>
        /// Вызывается в момент, когда машина на паркомате
        /// Дата нужна чтобы потом рассичтывать средний доход за день, ночь (null допустимо)
        /// </summary>
        /// <param name="vehicleScope"></param>
        public void AddToFinalScope(double vehicleScope, DateTime currentDateTime)
        {
            FinalScope += vehicleScope;
            if (currentDateTime.Hour < 21 && currentDateTime.Hour >= 6)
            {
                dayDateTimeScope += vehicleScope;
            }
            else
            {
                nightDateTimeScope += vehicleScope;
            }
        }

        /// <summary>
        /// Вызывается когда машина заняла парковочное место (когда заехала или непосредственно встала)
        /// </summary>
        public void SetBusyParkingPlaces(int busyCount)
        {
            countOfSetParkingPlaces++;
            if (countOfSetParkingPlaces > SkipSetParkingPlacesCount)
            {
                countOfSumBusyParkingPlaces += busyCount;
            }
        }

        public void ClearStatistic()
        {
            StartDateTime = EndDateTime = DateTime.Now;
            AverageNumberOfOccupiedPlaces = 0;
            AveragePercentageOfOccupiedPlaces = 0;
            AverageIncomePerDay = 0;
            AverageIncomePerNight = 0;
            FinalScope = 0;
            ParkingPlaces = 0;
            dayDateTimeScope = 0;
            nightDateTimeScope = 0;
            countOfSetParkingPlaces = 0;
            countOfSumBusyParkingPlaces = 0;
        }

        public void CalculateStatistic()
        {
            CalculateAverageIncome();
            CalculateAverageValueOfOccupiedPlaces();
        }
        
        private void CalculateAverageIncome()
        {
            if (StartDateTime == EndDateTime) return;
            
            var hours = (EndDateTime - StartDateTime).TotalHours;
            var dayHours = 0d;
            var nigthHours = 0d;
            var dateTime = StartDateTime.AddHours(0);
            
            while (hours > 0)
            {
                if (dateTime.Hour < 21 && dateTime.Hour >= 6)
                {
                    if (hours > 0 && hours < 1)
                    {
                        dayHours += hours;
                    }
                    else
                    {
                        dayHours++;
                    }
                }
                else
                {
                    if (hours > 0 && hours < 1)
                    {
                        nigthHours += hours;
                    }
                    else
                    {
                        nigthHours++;
                    }
                   
                }
                dateTime = dateTime.AddHours(1);
                hours--;
            }

            if (dayDateTimeScope != 0 && dayHours > 0.1)
            {
                AverageIncomePerDay = dayDateTimeScope / (dayHours / 15d);
            }

            if (nightDateTimeScope != 0 && nigthHours > 0.1)
            {
                AverageIncomePerNight = nightDateTimeScope / (nigthHours / 9d);
            }
        }

        private void CalculateAverageValueOfOccupiedPlaces()
        {
            if (ParkingPlaces != 0 && countOfSetParkingPlaces != 0)
            {
                AverageNumberOfOccupiedPlaces = countOfSumBusyParkingPlaces / (double)(countOfSetParkingPlaces - SkipSetParkingPlacesCount);
                AveragePercentageOfOccupiedPlaces = AverageNumberOfOccupiedPlaces / ParkingPlaces * 100;
            }
        }
    }
}