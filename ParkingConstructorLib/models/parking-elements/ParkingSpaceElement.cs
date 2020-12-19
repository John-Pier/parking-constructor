using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    [Serializable]
    public class ParkingSpaceElement: ParkingModelElement<Image>
    {
        private readonly Image model;

        public ParkingSpaceElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.ParkingSpace;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}
