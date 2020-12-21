using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class TruckParkingPlace
    {
        public CarVehicleModel car;
        public bool isBusy;
        public Coors coors;
        public TruckParkingPlace(Coors coors)
        {
            this.coors = coors;
            isBusy = false;
        }
        public void setTruck(Truck truck)
        {
            car = truck;
        }
    }
}
