namespace ParkingConstructorLib.models
{
    public class GrassParkingElement: ParkingModelElement
    {
        public GrassParkingElement()
        {

        }

        public override ParkingModelElementType GetElementType()
        {
            return ParkingModelElementType.Grass;
        }

        public override object GetElementModel()
        {
            return null;
        }
    }
}
