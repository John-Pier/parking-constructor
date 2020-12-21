using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public class CarVehicleModel: VehicleModel
    {
        private static int spawnRow;
        private static int spawnCol;
        protected int rowIndex;
        protected int columnIndex;
        public List<ParkingModelElementType> GetAvailableElementTypesForMovement()
        {
            return new List<ParkingModelElementType>
            {
                ParkingModelElementType.Road, 
                ParkingModelElementType.ParkingSpace, 
                ParkingModelElementType.Entry
            };
        }

        public enum CarType{
            Car,
            Truck
        };

        public static CarVehicleModel spawnCar(CarType carType)
        {
            if (carType == CarType.Car) return new Car(spawnRow, spawnCol);
            else return new Truck(spawnRow, spawnCol);
        }

        public static void setSpawnPoint(int col, int row)
        {
            spawnCol = col;
            spawnRow = row;
        }

        public int GetRequiredSize()
        {
            return 1;
        }

        public int getRowIndex()
        {
            return rowIndex;
        }

        public int getColumnIndex()
        {
            return columnIndex;
        }

        public void setRowIndex(int rowIndex)
        {
            this.rowIndex = rowIndex;
        }

        public void setColumnIndex(int columnIndex)
        {
            this.columnIndex = columnIndex;
        }
    }
}
