using System;
using System.Drawing;

namespace ParkingConstructorLib.models
{
    [Serializable]
    public class EntryParkingElement: ParkingModelElement<Image>
    {
        private readonly Image model;

        public EntryParkingElement(Image initModel)
        {
            model = initModel;
        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Entry;
        }

        public override Image GetElementModel()
        {
            return model;
        }
    }
}