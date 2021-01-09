using ParkingConstructorLib.models.vehicles;

namespace ParkingConstructorLib.models
{
    public abstract class AbstractParkingPlace
    {
        private static int lastID = 0;
        public int id;
        public bool isBusy;
        public Coors coors;
        
        protected AbstractParkingPlace(Coors coors)
        {
            this.coors = coors;
            isBusy = false;
            id = lastID;
            lastID++;
        }
        
        protected AbstractParkingPlace(int colIndex, int rowIndex)
        {
            coors = new Coors(colIndex, rowIndex);
            isBusy = false;
            id = lastID;
            lastID++;
        }

        protected abstract bool IsRideableElement(AbstractVehicleModel vehicleModel);
    }
}