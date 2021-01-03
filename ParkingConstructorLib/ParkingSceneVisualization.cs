using System.Drawing;
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
        private SettingsModel settingsModel;
        private MapAvailable<T> mapAvailable;
        private static Bitmap[] textures;
        private static Bitmap image;

        public ParkingSceneVisualization()
        {
        }

        public static void SetTextures(Bitmap[] texturesArr)
        {
            textures = texturesArr;
        }

        public void NextStep(double accelerate)
        {
            mapAvailable.nextStep(accelerate);
        }

        public void CreateCar(int timeWaitOnParkingInSeconds)
        {
            var carVehicleModel = AbstractVehicleModel.SpawnCar(CarType.Car);
            // var carVehicleModel = new CarVehicleModel(spawnRow, spawnCol);
            carVehicleModel.SetSecondsOnParking(timeWaitOnParkingInSeconds);
            mapAvailable.addCar(carVehicleModel);
        }

        public void CreateTruck(int timeWaitOnParkingInSeconds)
        {
            AbstractVehicleModel @abstract = AbstractVehicleModel.SpawnCar(CarType.Truck);
            @abstract.SetSecondsOnParking(timeWaitOnParkingInSeconds);
            mapAvailable.addCar(@abstract);
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

        public static void SetImage(Bitmap imageMap)
        {
            image = imageMap;
        }

        public Bitmap GetImage()
        {
            return image;
        }

        public bool IsCanAddThisCar(CarType carType)
        {
            return mapAvailable.IsCanAddCar(carType);
        }
    }
}
