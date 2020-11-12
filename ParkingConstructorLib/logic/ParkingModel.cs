using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.models;
using ParkingConstructorLib.services;

namespace ParkingConstructorLib.logic
{
    public class ParkingModel
    {
        private ParkingModelElement[,] parkingLot;
        private int columnCount;
        private int rowColumn;

        private ParkingModelService modelService = new ParkingModelService(); 

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

        public ParkingModelElement GetElement(int columnIndex, int rowIndex)
        {
            return parkingLot[columnIndex, rowIndex];
        }

        public bool IsParkingModelCorrect()
        {
            return modelService.CheckCorrectParkingModelElementsArray();
        }

        public void Clear()
        {
            Clear(columnCount, rowColumn);
        }

        public void Clear(int columnCount, int rowColumn)
        {
            this.columnCount = columnCount;
            this.rowColumn = rowColumn;
            parkingLot = new ParkingModelElement[columnCount, rowColumn];
        }
    }
}