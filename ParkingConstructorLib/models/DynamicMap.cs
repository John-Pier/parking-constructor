using ParkingConstructorLib.logic;
using ParkingConstructorLib.models.vehicles;
using ParkingConstructorLib.services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingConstructorLib.models
{
    public class DynamicMap<T> where T : class
    {
        private bool[,] map;
        private ParkingModel<T> model;
        private LinkedList<AbstractVehicleModel> cars;
        private LinkedList<AbstractParkingPlace> carParkingPlaces;
        private LinkedList<AbstractParkingPlace> truckParkingPlaces;
        public Coors cashierCoors;
        public Coors exitCoors;

        public int SpawnRow;
        public int SpawnCol;

        public DynamicMap(ParkingModel<T> model)
        {
            this.model = model;
            map = new bool[model.ColumnCount, model.RowCount];
            cars = new LinkedList<AbstractVehicleModel>();
            carParkingPlaces = new LinkedList<AbstractParkingPlace>();
            truckParkingPlaces = new LinkedList<AbstractParkingPlace>();
            reloadMap();

            for (int i = 0; i < model.ColumnCount; i++)
            {
                for (int j = 0; j < model.RowCount; j++)
                {
                    if (model.GetElement(i, j) == null) continue;
                    if (model.GetElement(i, j).GetElementType() == ParkingModelElementType.Entry)
                        SetSpawnPoint(i, j);
                    if (model.GetElement(i, j).GetElementType() == ParkingModelElementType.ParkingSpace)
                        carParkingPlaces.AddLast(new CarParkingPlace(i, j));
                    if (model.GetElement(i, j).GetElementType() == ParkingModelElementType.TruckParkingSpace)
                        truckParkingPlaces.AddLast(new TruckParkingPlace(new Coors(i, j)));
                    if (model.GetElement(i, j).GetElementType() == ParkingModelElementType.Cashier)
                        cashierCoors = new Coors(i, j);
                    if (model.GetElement(i, j).GetElementType() == ParkingModelElementType.Exit)
                        exitCoors = new Coors(i, j);
                }
            }
        }

        public void reloadMap()
        {
            for (int i = 0; i < model.ColumnCount; i++)
            {
                for (int j = 0; j < model.RowCount; j++)
                    map[i, j] = model.GetElement(i, j) == null;
            }
            foreach (AbstractVehicleModel car in cars)
                map[car.GetColumnIndex(), car.GetRowIndex()] = false;
        }

        public void AddTruck(AbstractVehicleModel vehicleModel)
        {
            AddVehicle(vehicleModel, truckParkingPlaces);
        }

        public void AddCar(AbstractVehicleModel vehicleModel)
        {
            AddVehicle(vehicleModel, carParkingPlaces);
        }

        private void AddVehicle(AbstractVehicleModel vehicleModel, LinkedList<AbstractParkingPlace> parkingPlaces)
        {
            var palace = parkingPlaces.First(value => !value.isBusy);
            if (palace != null)
            {
                vehicleModel.SetTarget(palace.coors);
                palace.isBusy = true;
                vehicleModel.setParkingID(palace.id);
                vehicleModel.isOnParkingPlace = false;
                cars.AddLast(vehicleModel);
            }

            reloadMap();
        }

        public int[,,] CreateAndInitLocalMap()
        {
            int[,,] localMap = new int[model.ColumnCount, model.RowCount, 3];
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                {
                    localMap[i, j, 0] = map[i, j] ? 1 : 1000;//Карта весов
                    localMap[i, j, 1] = 0; //0 - непосещённая; 1 - посещённая
                    localMap[i, j, 2] = 100000; //Сумма весов до точки
                }

            foreach (AbstractVehicleModel carTemp in cars)
                localMap[carTemp.GetColumnIndex(), carTemp.GetRowIndex(), 0] = 2000;
            return localMap;
        }

        public bool IsCanAddVehicle(CarType carType)
        {
            LinkedList<AbstractParkingPlace> parkingPlaces;
            switch (carType)
            {
                case CarType.Car:
                    parkingPlaces = carParkingPlaces;
                    break;
                case CarType.Truck:
                    parkingPlaces = truckParkingPlaces;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(carType), carType, null);
            }

            return parkingPlaces != null && parkingPlaces.Any(placeForCar => !placeForCar.isBusy);
        }

        private void SetSpawnPoint(int col, int row)
        {
            SpawnCol = col;
            SpawnRow = row;
        }

        public LinkedList<AbstractVehicleModel> getVehicles()
        {
            return cars;
        }

        public LinkedList<AbstractParkingPlace> getCarsParkingPlaces()
        {
            return carParkingPlaces;
        }

        public LinkedList<AbstractParkingPlace> getTruckParkingPlaces()
        {
            return truckParkingPlaces;
        }

        public List<AbstractParkingPlace> getParkingPlaces()
        {
            return carParkingPlaces.Concat(truckParkingPlaces).ToList();
        }
    }
}