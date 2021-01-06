using ParkingConstructorLib.models.vehicles;

namespace ParkingConstructorLib.models
{
    public abstract class AbstractParkingPlace
    {
        public bool isBusy;
        public Coors coors;
        
        protected AbstractParkingPlace(Coors coors)
        {
            this.coors = coors;
            isBusy = false;
        }
        
        protected AbstractParkingPlace(int colIndex, int rowIndex)
        {
            coors = new Coors(colIndex, rowIndex);
            isBusy = false;
        }

        protected abstract bool IsRideableElement(AbstractVehicleModel vehicleModel);
    }
}