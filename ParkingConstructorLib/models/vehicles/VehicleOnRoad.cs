using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models.vehicles
{
    public class VehicleOnRoad
    {
        public int position;
        public CarType carType;
        public bool isExited;
        public bool isStayOnParkingInThisTime;
        public VehicleOnRoad(CarType carType)
        {
            position = -1;
            this.carType = carType;
            this.isExited = false;
            isStayOnParkingInThisTime = false;
        }
        public VehicleOnRoad(CarType carType, int position)
        {
            this.position = position;
            this.carType = carType;
            this.isExited = true;
            isStayOnParkingInThisTime = true;
        }
    }
}
