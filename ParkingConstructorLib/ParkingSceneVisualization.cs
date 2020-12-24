using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using static ParkingConstructorLib.models.vehicles.CarVehicleModel;

namespace ParkingConstructorLib
{
    /// <summary>
    /// Визуализатор парковки
    /// </summary>
    public class ParkingSceneVisualization<T> where T: class
    {
        private ParkingModel<T> parkingModel;
        private SettingsModel settingsModel;
        private MapAvailable<T> mapAvailable;
        private static Bitmap[] textures;
        private static Bitmap image;

        public ParkingSceneVisualization()
        {
        }

        public static void setTextures(Bitmap[] texturesArr)
        {
            textures = texturesArr;
        }

        public void nextStep(double accelerate)
        {
            mapAvailable.nextStep(accelerate);
        }

        public void createCar(int timeWaitOnParkingInSeconds)
        {
            CarVehicleModel car = spawnCar(CarType.Car);
            car.setSecondsOnParking(timeWaitOnParkingInSeconds);
            mapAvailable.addCar(car);
        }

        public void createTruck(int timeWaitOnParkingInSeconds)
        {
            CarVehicleModel car = spawnCar(CarType.Truck);
            car.setSecondsOnParking(timeWaitOnParkingInSeconds);
            mapAvailable.addCar(car);
        }

        public void SetParkingModel(ParkingModel<T> parkingModel)
        {
            this.parkingModel = parkingModel;
            mapAvailable = new MapAvailable<T>(this.parkingModel, textures);

        }

        public void SetSettingsModel(SettingsModel model)
        {
            settingsModel = model;
        }

        public static void setImage(Bitmap imageMap)
        {
            image = imageMap;
        }

        public Bitmap getImage()
        {
            return image;
        }

        public bool isCanAddThisCar(CarType carType)
        {
            return mapAvailable.isCanAddCar(carType);
        }
    }
}
