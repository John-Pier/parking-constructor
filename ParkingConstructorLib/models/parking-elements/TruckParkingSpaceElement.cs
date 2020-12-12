using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.parking_elements
{
    public class TruckParkingSpaceElement: ParkingModelElement
    {
        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.TruckParkingSpace;
        }

        public override object GetElementModel()
        {
            throw new NotImplementedException();
        }
    }
}
