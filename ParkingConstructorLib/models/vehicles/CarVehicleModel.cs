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
        protected CarType type;
        protected TargetType targetType;
        protected Coors target;
        protected Coors nextStepCoors;
        protected int secondsOnParking;
        protected DateTime dateTimeStopping;
        public List<ParkingModelElementType> GetAvailableElementTypesForMovement()
        {
            return new List<ParkingModelElementType>
            {
                ParkingModelElementType.Road, 
                ParkingModelElementType.ParkingSpace, 
                ParkingModelElementType.Entry
            };
        }

        public void drive()
        {
            columnIndex = nextStepCoors.columnIndex;
            rowIndex = nextStepCoors.rowIndex;
        }

        public DateTime getDateTimeStopping()
        {
            return dateTimeStopping;
        }

        public void setDateTimeStopping(DateTime dateTimeStopping)
        {
            this.dateTimeStopping = dateTimeStopping;
        }

        public void setSecondsOnParking(int seconds)
        {
            secondsOnParking = seconds;
        }

        public int getSecondsOnParking()
        {
            return secondsOnParking;
        }

        public Coors getCoors()
        {
            return new Coors(columnIndex, rowIndex);
        }

        public void setNextCoors(Coors coors)
        {
            this.nextStepCoors = coors;
        }

        public Coors getNextCoors()
        {
            return nextStepCoors;
        }

        public void setTarget(Coors target)
        {
            this.target = target;
        }

        public Coors getTarget()
        {
            return target;
        }

        public enum CarType{
            Car,
            Truck
        };

        public enum TargetType
        {
            Parking,
            Cashier,
            Exit,
        }

        public TargetType getTargetType()
        {
            return targetType;
        }

        public void setTargetType(TargetType targetType)
        {
            this.targetType = targetType;
        }

        public new string GetType()
        {
            return type.ToString();
        }

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
