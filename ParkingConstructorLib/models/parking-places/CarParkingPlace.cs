using ParkingConstructorLib.models.vehicles;

namespace ParkingConstructorLib.models
{
    public class CarParkingPlace: AbstractParkingPlace
    {
        public CarParkingPlace(Coors coors): base(coors) { }
        
        public CarParkingPlace(int colIndex, int rowIndex): base(colIndex, rowIndex) { }
        
        protected override bool IsRideableElement(AbstractVehicleModel vehicleModel)
        {
            return vehicleModel.GetAvailableElementTypesForMovement().Contains(ParkingModelElementType.ParkingSpace);
        }
    }
}
