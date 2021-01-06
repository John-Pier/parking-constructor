using System.Collections.Generic;

namespace ParkingConstructorLib.models.vehicles
{
    public class TruckVehicleModel : AbstractVehicleModel
    {
        public TruckVehicleModel(int row, int column)
        {
            rowIndex = row;
            columnIndex = column;
            type = CarType.Truck;
            targetType = TargetType.Parking;
            countErrors = 0;
        }
        
        public new List<ParkingModelElementType> GetAvailableElementTypesForMovement()
        {
            var resultList = base.GetAvailableElementTypesForMovement();
            resultList.Add(ParkingModelElementType.TruckParkingSpace);
            return resultList;
        }
    }
}
