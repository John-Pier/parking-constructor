using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public class CarVehicleModel: VehicleModel
    {
        public List<ParkingModelElementType> GetAvailableElementTypesForMovement()
        {
            return new List<ParkingModelElementType>
            {
                ParkingModelElementType.Road, 
                ParkingModelElementType.ParkingSpace, 
                ParkingModelElementType.Entry
            };
        }

        public int GetRequiredSize()
        {
            return 1;
        }
    }
}
