using System;

namespace ParkingConstructorLib.models
{
    public class RoadParkingElement: ParkingModelElement
    {
        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Road;
        }

        public override object GetElementModel()
        {
            throw new NotImplementedException();
        }
    }
}
