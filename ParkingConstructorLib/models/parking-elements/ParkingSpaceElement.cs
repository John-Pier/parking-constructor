using System;

namespace ParkingConstructorLib.models
{
    public class ParkingSpaceElement: ParkingModelElement
    {
        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.ParkingSpace;
        }

        public override object GetElementModel()
        {
            throw new NotImplementedException();
        }
    }
}
