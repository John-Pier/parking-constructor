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

        public ParkingModel(int columnCount, int rowColumn)
        {
            this.parkingLot = new ParkingModelElement[columnCount, rowColumn];
        }

        public void SetElement(int columnIndex, int rowIndex, ParkingModelElement element)
        {
            try
            {
                this.parkingLot[columnIndex, rowIndex] = element;
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
    }
}