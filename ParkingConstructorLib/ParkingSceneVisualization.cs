using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;

namespace ParkingConstructorLib
{
    /// <summary>
    /// Визуализатор парковки
    /// </summary>
    public class ParkingSceneVisualization<T> where T: class
    {
        private ParkingModel<T> parkingModel;
        private MapAvailable<T> mapAvailable;

        public ParkingSceneVisualization()
        {
        }

        public void SetParkingModel(ParkingModel<T> parkingModel)
        {
            this.parkingModel = parkingModel;
            mapAvailable = new MapAvailable<T>(this.parkingModel);
            CarVehicleModel car = CarVehicleModel.spawnCar(CarVehicleModel.CarType.Car);
            CarVehicleModel car2 = CarVehicleModel.spawnCar(CarVehicleModel.CarType.Truck);
            //mapAvailable.addCar(car);
            //mapAvailable.printMap();
            //car.setRowIndex(2);
            //mapAvailable.reloadMap();
            //mapAvailable.printMap();
            Console.WriteLine(car.GetType());
            Console.WriteLine(car2.GetType());
        }
    }
}
