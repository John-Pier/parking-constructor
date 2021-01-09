using System;
using System.Collections.Generic;
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

        public void NextStep(DateTime modelTime)
        {
            mapAvailable.nextStep(modelTime);
        }

        public void CreateCar(int timeWaitOnParkingInSeconds)
        {
            if(!IsCanAddVehicle(CarType.Car)) return;
            var carVehicleModel = new CarVehicleModel(mapAvailable.SpawnRow, mapAvailable.SpawnCol);
            carVehicleModel.SetSecondsOnParking(timeWaitOnParkingInSeconds);
            
            mapAvailable.addCar(carVehicleModel);
        }

        public void CreateTruck(int timeWaitOnParkingInSeconds)
        {
            if(!IsCanAddVehicle(CarType.Truck)) return;
            var truckVehicleModel = new TruckVehicleModel(mapAvailable.SpawnRow, mapAvailable.SpawnCol);
            truckVehicleModel.SetSecondsOnParking(timeWaitOnParkingInSeconds);
            
            mapAvailable.addCar(truckVehicleModel);
        }

        public void SetParkingModel(ParkingModel<T> parkingModel)
        {
            this.parkingModel = parkingModel;
            mapAvailable = new MapAvailable<T>(this.parkingModel, textures);
        }

        public static void SetImage(Bitmap imageMap)
        {
            image = imageMap;
        }

        public Bitmap GetImage()
        {
            return image;
        }

        public bool IsCanAddVehicle(CarType carType)
        {
            return mapAvailable.IsCanAddVehicle(carType);
        }

        public LinkedList<AbstractVehicleModel> getVehicles()
        {
            return mapAvailable.getVehicles();
        }

        public LinkedList<AbstractParkingPlace> getParkingPlaces()
        {
            return mapAvailable.getParkingPlaces();
        }
    }
}
