using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public abstract class AbstractVehicleModel: IVehicleModel
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
        protected LastDirection lastDirection;
        protected int countErrors;
        
        public List<ParkingModelElementType> GetAvailableElementTypesForMovement()
        {
            return new List<ParkingModelElementType>
            {
                ParkingModelElementType.Road, 
                ParkingModelElementType.ParkingSpace, 
                ParkingModelElementType.Entry
            };
        }

        public void SetCountErrors(int countErrors)
        {
            this.countErrors = countErrors;
        }

        public int GetCountErrors()
        {
            return countErrors;
        }

        public LastDirection GetLastDirection()
        {
            return lastDirection;
        }

        public void Drive()
        {
            if (columnIndex == nextStepCoors.columnIndex)
                lastDirection = LastDirection.Vertical;
            if (rowIndex == nextStepCoors.rowIndex)
                lastDirection = LastDirection.Horizontal;
            columnIndex = nextStepCoors.columnIndex;
            rowIndex = nextStepCoors.rowIndex;
            countErrors = 0;
        }

        public DateTime GetDateTimeStopping()
        {
            return dateTimeStopping;
        }

        public void SetDateTimeStopping(DateTime dateTimeStopping)
        {
            this.dateTimeStopping = dateTimeStopping;
        }

        public void SetSecondsOnParking(int seconds)
        {
            secondsOnParking = seconds;
        }

        public int GetSecondsOnParking()
        {
            return secondsOnParking;
        }

        public Coors GetCoors()
        {
            return new Coors(columnIndex, rowIndex);
        }

        public void SetNextCoors(Coors coors)
        {
            this.nextStepCoors = coors;
        }

        public Coors GetNextCoors()
        {
            return nextStepCoors;
        }

        public void SetTarget(Coors target)
        {
            this.target = target;
        }

        public Coors GetTarget()
        {
            return target;
        }

        public TargetType GetTargetType()
        {
            return targetType;
        }

        public void SetTargetType(TargetType targetType)
        {
            this.targetType = targetType;
        }

        public new string GetType()
        {
            return type.ToString();
        }

        //TODO: Убрать, это тут не должно быть, вообще такое писать не стоит
        public static AbstractVehicleModel SpawnCar(CarType carType)
        {
            if (carType == CarType.Car) return new CarVehicleModel(spawnRow, spawnCol);
            else return new Truck(spawnRow, spawnCol);
        }

        public static void setSpawnPoint(int col, int row)
        {
            spawnCol = col;
            spawnRow = row;
        }

        public int GetRowIndex()
        {
            return rowIndex;
        }

        public int GetColumnIndex()
        {
            return columnIndex;
        }
    }
}
