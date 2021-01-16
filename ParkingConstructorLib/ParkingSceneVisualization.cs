using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using ParkingConstructorLib.services;
using ParkingConstructorLib.utils.distributions;

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
        private DrawerService<T> drawer; //Отрисовщик
        private MovementService<T> movement; //Передвижения
        private SettingsModel settingsModel;
        private StatisticModel statisticModel;
        private UniformDistribution generationStreamRandom = new UniformDistribution(0d, 100d);

        public static void SetTextures(Bitmap[] texturesArr)
        {
            textures = texturesArr;
        }

        public void NextStep(DateTime modelTime)
        {
            int[,,] localMap;
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            LinkedList<AbstractVehicleModel> vehicles = dynamicMap.getVehicles();
            foreach (AbstractVehicleModel vehicleModel in vehicles)
            {
                localMap = dynamicMap.CreateAndInitLocalMap();
                Coors[] way = movement.foundWay(localMap, vehicleModel);
                AbstractVehicleModel[] remCars = movement.nextSystemStep(localMap, vehicleModel, way, modelTime, statisticModel);
                foreach (var vehicle in remCars)
                    removedCars.AddLast(vehicle);
            }
            try
            {
                foreach (var carTemp in removedCars)
                    vehicles.Remove(carTemp);
            }
            catch (Exception) { }
            
            statisticModel.SetBusyParkingPlaces(dynamicMap.getParkingPlaces().Count(place => place.isBusy));
            
            drawer.Draw(vehicles);
        }

        public void CreateVehicle()
        {
            var parkingTimeInMinutes = (int)(settingsModel.ParkingTimeDistribution.GetRandNumber() * 60);
            AbstractVehicleModel vehicleModel;
            
            //TODO: Добавить учет процента заезда машин на парковку
            if (generationStreamRandom.GetRandNumber() > settingsModel.PercentOfTrack)
            {
                if(!IsCanAddVehicle(CarType.Car)) return;
                vehicleModel = new CarVehicleModel(dynamicMap.SpawnRow, dynamicMap.SpawnCol);
                dynamicMap.AddCar(vehicleModel);
            }
            else
            {
                if(!IsCanAddVehicle(CarType.Truck)) return;
                vehicleModel = new TruckVehicleModel(dynamicMap.SpawnRow, dynamicMap.SpawnCol);
                dynamicMap.AddTruck(vehicleModel);
            }
            vehicleModel.SetSecondsOnParking(parkingTimeInMinutes);
            drawer.Draw(dynamicMap.getVehicles());
        }

        public void SetParkingModel(ParkingModel<T> parkingModel)
        {
            this.parkingModel = parkingModel;
            dynamicMap = new DynamicMap<T>(this.parkingModel);
            movement = new MovementService<T>(this.parkingModel, dynamicMap);
            drawer = new DrawerService<T>(this.parkingModel, textures);
        }

        public void SetStatisticModel(StatisticModel statisticModel)
        {
            this.statisticModel = statisticModel;
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

        public List<AbstractParkingPlace> getParkingPlaces()
        {
            return dynamicMap.getParkingPlaces();
        }

        public void SetSettingsModel(SettingsModel settingsModel)
        {
            this.settingsModel = settingsModel;
        }

        public bool IsSettingsModelSet()
        {
            return this.settingsModel != null;
        }
    }
}
