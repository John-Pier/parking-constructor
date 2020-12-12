using System;

namespace ParkingConstructorLib.models
{
    public class EntryParkingElement: ParkingModelElement
    {
        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Entry;
        }

        public override object GetElementModel()
        {
            throw new NotImplementedException();
        }
    }
}