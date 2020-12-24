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

        public void nextStep()
        {
            mapAvailable.nextStep();
        }

        public void createCar()
        {
            CarVehicleModel car = CarVehicleModel.spawnCar(CarVehicleModel.CarType.Car);
            car.setSecondsOnParking(5);
            mapAvailable.addCar(car);
        }

        public void createTruck()
        {
            CarVehicleModel car = CarVehicleModel.spawnCar(CarVehicleModel.CarType.Truck);
            car.setSecondsOnParking(5);
            mapAvailable.addCar(car);
        }

        public void SetParkingModel(ParkingModel<T> parkingModel)
        {
            this.parkingModel = parkingModel;
            mapAvailable = new MapAvailable<T>(this.parkingModel);
        }
    }
}
