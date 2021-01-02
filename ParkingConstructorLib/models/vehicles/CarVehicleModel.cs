using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public class CarVehicleModel : AbstractVehicleModel
    {
        public CarVehicleModel(int row, int column)
        {
            rowIndex = row;
            columnIndex = column;
            type = CarType.Car;
            targetType = TargetType.Parking;
            countErrors = 0;
        }
    }
}
