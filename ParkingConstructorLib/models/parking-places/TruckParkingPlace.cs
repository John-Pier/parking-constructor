using ParkingConstructorLib.models.vehicles;

namespace ParkingConstructorLib.models
{
    public class TruckParkingPlace: AbstractParkingPlace
    {
        public TruckParkingPlace(Coors coors): base(coors) { }
        
        public TruckParkingPlace(int colIndex, int rowIndex): base(colIndex, rowIndex) { }
        
        protected override bool IsRideableElement(AbstractVehicleModel vehicleModel)
        {
            return vehicleModel.GetAvailableElementTypesForMovement().Contains(ParkingModelElementType.TruckParkingSpace);
        }
    }
}
