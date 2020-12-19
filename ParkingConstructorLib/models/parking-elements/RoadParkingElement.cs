using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    [Serializable]
    public class RoadParkingElement : ParkingModelElement<Image>
    {
        private readonly Image model;

        public RoadParkingElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Road;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}