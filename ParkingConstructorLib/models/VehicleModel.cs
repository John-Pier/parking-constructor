using System.Collections.Generic;

namespace ParkingConstructorLib.models
{
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

    public enum LastDirection
    {
        Horizontal,
        Vertical,
    }
    
    public interface IVehicleModel
    {
        List<ParkingModelElementType> GetAvailableElementTypesForMovement();
    }
}
