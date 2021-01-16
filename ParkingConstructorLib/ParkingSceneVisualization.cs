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
        private ManagerVehiclesOnRoad<T> roadManager; //Управление движением на дороге
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

            if(roadManager != null)
                roadManager.NextStep();
            
            drawer.Draw(vehicles, roadManager);

            
            statisticModel.SetBusyParkingPlaces(dynamicMap.getParkingPlaces().Count(place => place.isBusy));

        }

        public void CreateVehicle()
        {
            if (generationStreamRandom.GetRandNumber() > settingsModel.PercentOfTrack)
                roadManager.CreateNewVehicle(CarType.Car, false);
            else
                roadManager.CreateNewVehicle(CarType.Truck, false);
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
            if(settingsModel == null) return;
            int lastRoadPositionIndex = 0;
            if (parkingModel.RoadDirection == RoadDirections.Bottom || parkingModel.RoadDirection == RoadDirections.Top)
                lastRoadPositionIndex = parkingModel.ColumnCount - 1;
            else
                lastRoadPositionIndex = parkingModel.RowCount - 1;
            roadManager = new ManagerVehiclesOnRoad<T>(settingsModel, parkingModel, dynamicMap, lastRoadPositionIndex);
            movement.setRoadManager(roadManager);
        }

        public bool IsSettingsModelSet()
        {
            return settingsModel != null && settingsModel.GenerationStreamDistribution != null && settingsModel.ParkingTimeDistribution != null;
        }

        public void freeLastId()
        {
            AbstractParkingPlace.FreeLastId();
        }

        public void Stop()
        {
            roadManager.Stop();
            drawer.Stop();
            freeLastId();
            dynamicMap = new DynamicMap<T>(this.parkingModel);
            movement = new MovementService<T>(this.parkingModel, dynamicMap);
            drawer = new DrawerService<T>(this.parkingModel, textures);
            int lastRoadPositionIndex = 0;
            if (parkingModel.RoadDirection == RoadDirections.Bottom || parkingModel.RoadDirection == RoadDirections.Top)
                lastRoadPositionIndex = parkingModel.ColumnCount - 1;
            else
                lastRoadPositionIndex = parkingModel.RowCount - 1;
            roadManager = new ManagerVehiclesOnRoad<T>(settingsModel, parkingModel, dynamicMap, lastRoadPositionIndex);
            movement.setRoadManager(roadManager);
        }
    }
}
