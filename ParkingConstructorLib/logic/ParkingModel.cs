using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.models;
using ParkingConstructorLib.services;
using ParkingConstructorLib.utils.distributions;

namespace ParkingConstructorLib.logic
{
    //Расположение дороги
    public enum RoadDirections
    {
        Top = 0,
        Bottom = 1,
        Right = 2,
        Left = 3
    }

    [Serializable]
    public class ParkingModel<T> where T: class
    {
        private ParkingModelElement<T>[,] parkingLot;//Элементы парковки

        public int ColumnCount { get; private set; }//Количество столбцов модели

        public int RowCount { get; private set; }//Количество строк модели

        public RoadDirections RoadDirection;//Расположение дороги

        public ParkingModel(int columnCount, int rowCount)//Конструктор
        {
            this.ColumnCount = columnCount;
            this.RowCount = rowCount; 
            parkingLot = new ParkingModelElement<T>[columnCount, rowCount];
            RoadDirection = RoadDirections.Bottom;
        }

        //Установить элемент
        public void SetElement(int columnIndex, int rowIndex, ParkingModelElement<T> element)
        {
            try
            {
                parkingLot[columnIndex, rowIndex] = element;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //Получить элемент
        public ParkingModelElement<T> GetElement(int columnIndex, int rowIndex)
        {
            return parkingLot[columnIndex, rowIndex];
        }

        //Проверка на корректность модели
        public bool IsParkingModelCorrect()
        {
            var isEntryExists = false;
            var isOneEntry = false;

            var isExitExists = false;
            var isOneExit = false;

            var isCashierExists = false;
            var isOneCashier = false;
            
            var isParkingSpaceExists = false;

            for (var i = 0; i < ColumnCount; i++)
            {
                for (var j = 0; j < RowCount; j++)
                {
                    var element = parkingLot[i, j];
                    if(element == null) continue;
                    var elementType = element.GetElementType();
                    switch (elementType)
                    {
                        case ParkingModelElementType.Entry:
                            isOneEntry = !isEntryExists;
                            isEntryExists = true;
                            if (!IsCorrectBorderElement(i, j)) return false;
                            break;
                        case ParkingModelElementType.Exit:
                            isOneExit = !isExitExists;
                            isExitExists = true;
                            if (!IsCorrectBorderElement(i, j)) return false;
                            break;
                        case ParkingModelElementType.Cashier:
                            isOneCashier = !isCashierExists;
                            isCashierExists = true;
                            break;
                        case ParkingModelElementType.ParkingSpace:
                        case ParkingModelElementType.TruckParkingSpace:
                            isParkingSpaceExists = true;
                            break;
                        default: break;
                    }
                }
            }

            return isEntryExists && isOneEntry && 
                   isExitExists && isOneExit &&
                   isCashierExists && isOneCashier &&
                   isParkingSpaceExists; // TODO: можно возвращать обьект с указанием того, что именно не корректно (если вдруг понадобится)
        }

        //Очистить модель
        public void Clear()
        {
            Clear(ColumnCount, RowCount);
        }

        public void Clear(int columnCount, int rowCount)
        {
            ColumnCount = columnCount;
            RowCount = rowCount;
            parkingLot = new ParkingModelElement<T>[columnCount, rowCount];
        }

        //Проверка на граничный элемент
        private bool IsBorderElement(int column, int row)
        {
            return (column == 0 || row == 0) || (column == ColumnCount - 1 || row == RowCount - 1);
        }

        //Проверка на корректность граничного элемента
        private bool IsCorrectBorderElement(int column, int row)
        {
            bool correct;

            switch (RoadDirection)
            {
                case RoadDirections.Top:
                    correct = row == 0;
                    break;
                case RoadDirections.Bottom:
                    correct = row == RowCount - 1;
                    break;
                case RoadDirections.Right:
                    correct = column == ColumnCount - 1;
                    break;
                case RoadDirections.Left:
                    correct = column == 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return correct && IsBorderElement(column, row);
        }
    }
}