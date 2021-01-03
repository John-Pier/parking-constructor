using ParkingConstructorLib.models.vehicles;

namespace ParkingConstructorLib.models
{
    public class CarParkingPlace
    {
        public AbstractVehicleModel Abstract;
        public bool isBusy;
        public Coors coors;
        
        public CarParkingPlace(Coors coors)
        {
            this.coors = coors;
            isBusy = false;
        }
        
        public void setCar(CarVehicleModel carVehicleModel)
        {
            this.Abstract = carVehicleModel;
        }
    }
}
