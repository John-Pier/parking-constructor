using System;

namespace ParkingConstructorLib.models
{
    public class CashierParkingElement : ParkingModelElement
    {
        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Cashier;
        }

        public override object GetElementModel()
        {
            throw new NotImplementedException();
        }
    }
}