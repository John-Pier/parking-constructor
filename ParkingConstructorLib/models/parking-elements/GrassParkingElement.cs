using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.parking_elements
{
    public class GrassParkingElement: ParkingModelElement
    {
        public GrassParkingElement()
        {

        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Grass;
        }

        public override object GetElementModel()
        {
            return null;
        }
    }
}
