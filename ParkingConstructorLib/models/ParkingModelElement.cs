using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public abstract class ParkingModelElement
    {
        public abstract ParkingModelElementType GetType();

        public abstract Object GetElementModel();

        public bool IsRideableElement(VehicleModel vehicleModel)
        {
            return vehicleModel.GetAvailableElementTypesForMovement().Contains(this.GetType());
        }
    }

    public enum ParkingModelElementType
    {
        Grass,
        Road,
        TruckParkingSpace,
        ParkingSpace,
        Entry,
        Exit,
        ParkingMeter
    }
}
