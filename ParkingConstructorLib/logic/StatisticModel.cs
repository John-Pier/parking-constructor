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
        /// Содержит общее чисто доступных парковочных мест на карте
        /// </summary>
        public int ParkingPlaces;


        private double dayDateTimeScope = 0;
        private double nightDateTimeScope = 0;

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
        public void TakeParkingPlace()
        {
            
        }
        
        /// <summary>
        /// Вызывается когда машина покидает парковочное место
        /// (не важно в какой момент, например после выезда с парковки или ее оплаты)
        /// </summary>
        public void FreeParkingPlace()
        {
            
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
        }

        public void CalculateStatistic()
        {
            CalculateAverageIncome();
        }
        
        private void CalculateAverageIncome()
        {
            if (StartDateTime != EndDateTime)
            {
                var days = (EndDateTime - StartDateTime).Days;
                AverageIncomePerDay = dayDateTimeScope / days;
                AverageIncomePerNight = nightDateTimeScope / days;
            }
        }
    }
}