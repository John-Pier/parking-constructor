using System;

namespace ParkingConstructorLib.models
{
    public class ExitParkingElement: ParkingModelElement
    {
        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Exit;
        }

        public override object GetElementModel()
        {
            throw new NotImplementedException();
        }
    }
}
