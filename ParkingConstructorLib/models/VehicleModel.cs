using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
