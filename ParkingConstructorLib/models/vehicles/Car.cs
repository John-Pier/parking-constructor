using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public class Car : CarVehicleModel
    {
        public Car(int row, int column)
        {
            rowIndex = row;
            columnIndex = column;
            type = CarType.Car;
            targetType = TargetType.Parking;
        }
    }
}
