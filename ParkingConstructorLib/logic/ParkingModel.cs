using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.models;
using ParkingConstructorLib.services;

namespace ParkingConstructorLib.logic
{
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
        private ParkingModelElement<T>[,] parkingLot;

        public int ColumnCount { get; private set; }

        public int RowCount { get; private set; }

        public RoadDirections RoadDirection;

        public ParkingModel(int columnCount, int rowCount)
        {
            this.ColumnCount = columnCount;
            this.RowCount = rowCount; 
            parkingLot = new ParkingModelElement<T>[columnCount, rowCount];
            RoadDirection = RoadDirections.Bottom;
        }

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

        public ParkingModelElement<T> GetElement(int columnIndex, int rowIndex)
        {
            return parkingLot[columnIndex, rowIndex];
        }

        public bool IsParkingModelCorrect()
        {
            var isEntryExists = false;
            var isOneEntry = false;

            var isExitExists = false;
            var isOneExit = false;

            var isCashierExists = false;
            var isOneCashier = false;

            var isParkingSpaceExists = false;

            // TODO: Проверка на корректную сторону

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
                            if (!IsBorderElement(i, j)) return false;
                            break;
                        case ParkingModelElementType.Exit:
                            isOneExit = !isExitExists;
                            isExitExists = true;
                            if (!IsBorderElement(i, j)) return false;
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

        public void Clear()
        {
            Clear(ColumnCount, RowCount);
        }

        public void Clear(int columnCount, int rowCount)
        {
            this.ColumnCount = columnCount;
            this.RowCount = rowCount;
            parkingLot = new ParkingModelElement<T>[columnCount, rowCount];
        }

        private bool IsBorderElement(int column, int row)
        {
            return (column == 0 || row == 0) || (column == ColumnCount - 1 || row == RowCount - 1);
        }
        
        //private 
    }
}