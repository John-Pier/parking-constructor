using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class CarParkingPlace
    {
        public CarVehicleModel car;
        public bool isBusy;
        public Coors coors;
        public CarParkingPlace(Coors coors)
        {
            this.coors = coors;
            isBusy = false;
        }
        public void setCar(Car car)
        {
            this.car = car;
        }
    }
}
