using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    [Serializable]
    public class GrassParkingElement: ParkingModelElement<Image>
    {
        private readonly Image model;

        public GrassParkingElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Grass;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}
