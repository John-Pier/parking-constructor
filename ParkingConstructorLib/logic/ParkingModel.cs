using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.models;
using ParkingConstructorLib.services;

namespace ParkingConstructorLib.logic
{
    [Serializable]
    public class ParkingModel<T> where T: class
    {
        private ParkingModelElement<T>[,] parkingLot;

        public int ColumnCount { get; private set; }

        public int RowColumn { get; private set; }
        
        // [NonSerialized]
        // private ParkingModelService modelService = new ParkingModelService(); 

        public ParkingModel(int columnCount, int rowColumn)
        {
            this.ColumnCount = columnCount;
            this.RowColumn = rowColumn; 
            parkingLot = new ParkingModelElement<T>[columnCount, rowColumn];
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

            for (var i = 0; i < ColumnCount; i++)
            {
                for (var j = 0; j < RowColumn; j++)
                {
                    var elementType = parkingLot[i, j].GetElementType();
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
                   isParkingSpaceExists;
        }

        public void Clear()
        {
            Clear(ColumnCount, RowColumn);
        }

        public void Clear(int columnCount, int rowColumn)
        {
            this.ColumnCount = columnCount;
            this.RowColumn = rowColumn;
            parkingLot = new ParkingModelElement<T>[columnCount, rowColumn];
        }

        private bool IsBorderElement(int column, int row)
        {
            return (column == 0 || row == 0) || (column == ColumnCount - 1 || row == RowColumn - 1);
        }
    }
}