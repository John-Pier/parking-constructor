using System;
using System.Collections.Generic;
using System.Drawing;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using ParkingConstructorLib.services;

namespace ParkingConstructorLib
{
    /// <summary>
    /// Визуализатор парковки
    /// </summary>
    public class ParkingSceneVisualization<T> where T: class
    {
        private ParkingModel<T> parkingModel;
        private DynamicMap<T> dynamicMap;
        private static Bitmap[] textures;
        private DrawerService<T> drawer;//Отрисовщик
        private MovementService<T> movement;//Передвижения

        public ParkingSceneVisualization()
        {
        }

        public static void SetTextures(Bitmap[] texturesArr)
        {
            textures = texturesArr;
        }

        public void NextStep(DateTime modelTime)
        {
            int[,,] localMap;
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            LinkedList<AbstractVehicleModel> cars = dynamicMap.getVehicles();
            foreach (AbstractVehicleModel car in cars)
            {
                localMap = dynamicMap.CreateAndInitLocalMap();
                Coors[] way = movement.foundWay(localMap, car);
                AbstractVehicleModel[] remCars = movement.nextSystemStep(localMap, car, way, modelTime);
                for (int i = 0; i < remCars.Length; i++)
                    removedCars.AddLast(remCars[i]);
            }
            try
            {
                foreach (AbstractVehicleModel carTemp in removedCars)
                    cars.Remove(carTemp);
            }
            catch (Exception) { }
            drawer.Draw(cars);
        }

        public void CreateCar(int timeWaitOnParkingInSeconds)
        {
            if(!IsCanAddVehicle(CarType.Car)) return;
            var carVehicleModel = new CarVehicleModel(dynamicMap.SpawnRow, dynamicMap.SpawnCol);
            carVehicleModel.SetSecondsOnParking(timeWaitOnParkingInSeconds);
            
            dynamicMap.addCar(carVehicleModel);
            drawer.Draw(dynamicMap.getVehicles());
        }

        public void CreateTruck(int timeWaitOnParkingInSeconds)
        {
            if(!IsCanAddVehicle(CarType.Truck)) return;
            var truckVehicleModel = new TruckVehicleModel(dynamicMap.SpawnRow, dynamicMap.SpawnCol);
            truckVehicleModel.SetSecondsOnParking(timeWaitOnParkingInSeconds);
            
            dynamicMap.addCar(truckVehicleModel);
            drawer.Draw(dynamicMap.getVehicles());
        }

        public void SetParkingModel(ParkingModel<T> parkingModel)
        {
            this.parkingModel = parkingModel;
            dynamicMap = new DynamicMap<T>(this.parkingModel);
            movement = new MovementService<T>(this.parkingModel, dynamicMap);
            drawer = new DrawerService<T>(this.parkingModel, textures);
        }

        public Bitmap GetImage()
        {
            return drawer.getImage();
        }

        public bool IsCanAddVehicle(CarType carType)
        {
            return dynamicMap.IsCanAddVehicle(carType);
        }

        public LinkedList<AbstractVehicleModel> getVehicles()
        {
            return dynamicMap.getVehicles();
        }

        public LinkedList<AbstractParkingPlace> getParkingPlaces()
        {
            return dynamicMap.getParkingPlaces();
        }
    }
}
