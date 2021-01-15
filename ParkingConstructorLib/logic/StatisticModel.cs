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
        
        /// <summary>
        /// Вызывается в момент, когда машина на паркомате
        /// Дата нужна чтобы потом рассичтывать средний доход за день, ночь (null допустимо)
        /// </summary>
        /// <param name="vehicleScope"></param>
        public void AddToFinalScope(double vehicleScope, DateTime currentDateTime)
        {
            FinalScope += vehicleScope;
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
            
        }
    }
}