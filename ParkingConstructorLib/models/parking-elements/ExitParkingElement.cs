using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    public class ExitParkingElement: ParkingModelElement<Image>
    {
        private readonly Image model;

        public ExitParkingElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Exit;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}
