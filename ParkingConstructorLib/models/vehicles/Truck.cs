using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public class Truck : CarVehicleModel
    {
        public Truck(int row, int column)
        {
            rowIndex = row;
            columnIndex = column;
            type = CarType.Truck;
            targetType = TargetType.Parking;
            countErrors = 0;
        }
    }
}
