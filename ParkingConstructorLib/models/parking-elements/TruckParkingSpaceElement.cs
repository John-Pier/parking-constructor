using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    [Serializable]
    public class TruckParkingSpaceElement: ParkingModelElement<Image>
    {
        private readonly Image model;

        public TruckParkingSpaceElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.TruckParkingSpace;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}
