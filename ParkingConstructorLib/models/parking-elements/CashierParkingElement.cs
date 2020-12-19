using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    [Serializable]
    public class CashierParkingElement : ParkingModelElement<Image>
    {
        private readonly Image model;

        public CashierParkingElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Cashier;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}