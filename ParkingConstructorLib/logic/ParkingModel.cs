using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.models;

namespace ParkingConstructorLib.logic
{
    public class ParkingModel
    {
        private ParkingModelElement[,] parkingLot;
        private int columnCount;
        private int rowColumn;

        public ParkingModel(int columnCount, int rowColumn)
        {
            this.columnCount = columnCount;
            this.rowColumn = rowColumn; 
            parkingLot = new ParkingModelElement[columnCount, rowColumn];
        }

        public void SetElement(int columnIndex, int rowIndex, ParkingModelElement element)
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

        public bool IsParkingModelCorrect()
        {
            return true;
        }

        public void Clear()
        {
            this.Clear(this.columnCount, this.rowColumn);
        }

        public void Clear(int columnCount, int rowColumn)
        {
            this.columnCount = columnCount;
            this.rowColumn = rowColumn;
            parkingLot = new ParkingModelElement[columnCount, rowColumn];
        }
    }
}