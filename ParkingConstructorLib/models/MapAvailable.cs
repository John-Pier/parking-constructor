using ParkingConstructorLib.logic;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ParkingConstructorLib.models
{
    public class MapAvailable<T> where T : class
    {
        private bool[,] map;
        private ParkingModel<T> model;
        private LinkedList<AbstractVehicleModel> cars;
        private LinkedList<AbstractParkingPlace> carParkingPlaces;
        private LinkedList<AbstractParkingPlace> truckParkingPlaces;
        private Coors cashierCoors = null;
        private Coors exitCoors = null;
        private Bitmap drawMapOriginal;
        private Bitmap[] textures;
        
        public int SpawnRow;
        public int SpawnCol;

        public MapAvailable(ParkingModel<T> model, Bitmap[] textures)
        {
            this.model = model;
            this.textures = textures;
            map = new bool[model.ColumnCount, model.RowCount];
            cars = new LinkedList<AbstractVehicleModel>();
            carParkingPlaces = new LinkedList<AbstractParkingPlace>();
            truckParkingPlaces = new LinkedList<AbstractParkingPlace>();
            reloadMap();
            const int textureSize = 10;
            for (int i = 0; i < model.ColumnCount; i++)
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
            drawMapOriginal = new Bitmap(model.ColumnCount * textureSize, model.RowCount * textureSize);
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    for (int k = 0; k < textureSize; k++)
                        for (int l = 0; l < textureSize; l++)
                        {
                            if (model.GetElement(i, j) == null || model.GetElement(i, j).GetElementType() == ParkingModelElementType.Road)
                            {
                                drawMapOriginal.SetPixel(i * textureSize + k, j * textureSize + l, textures[10].GetPixel(k, l));
                                continue;
                            }
                            int textureIndex = -1;
                            switch(model.GetElement(i, j).GetElementType())
                            {
                                case ParkingModelElementType.Entry:
                                    textureIndex = 0;
                                    break;
                                case ParkingModelElementType.Exit:
                                    textureIndex = 1;
                                    break;
                                case ParkingModelElementType.Cashier:
                                    textureIndex = 2;
                                    break;
                                case ParkingModelElementType.ParkingSpace:
                                    textureIndex = 4;
                                    break;
                                case ParkingModelElementType.TruckParkingSpace:
                                    textureIndex = 5;
                                    break;
                                case ParkingModelElementType.Grass:
                                    textureIndex = 3;
                                    break;
                            }
                            if(textureIndex != -1)
                                drawMapOriginal.SetPixel(i * textureSize + k, j * textureSize + l, textures[textureIndex].GetPixel(k, l));
                        }
            ParkingSceneVisualization<Image>.SetImage(drawMapOriginal);
        }
        public void Draw()
        {
            const int textureSize = 10;
            Bitmap result = (Bitmap)drawMapOriginal.Clone();
            foreach (AbstractVehicleModel car in cars)
            {
                for (int i = 0; i < model.ColumnCount; i++)
                    for (int j = 0; j < model.RowCount; j++)
                        if (car.GetCoors().Equals(new Coors(i, j)))
                            for (int k = 0; k < textureSize; k++)
                                for (int l = 0; l < textureSize; l++)
                                {
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Horizontal && textures[6].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[6].GetPixel(k, l));
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Vertical && textures[7].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[7].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Horizontal && textures[8].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[8].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Vertical && textures[9].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[9].GetPixel(k, l));
                                }
            }
            ParkingSceneVisualization<Image>.SetImage(result);
        }
        public void reloadMap()
        {
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    map[i, j] = model.GetElement(i, j) == null ? true : false;
            foreach (AbstractVehicleModel car in cars)
                map[car.GetColumnIndex(), car.GetRowIndex()] = false;
        }
        public void addCar(AbstractVehicleModel vehicleModel)
        {
            bool isCarCreated = false;
            if (vehicleModel.GetType().Equals("Car"))
                foreach (CarParkingPlace placeC in carParkingPlaces)
                    if (!placeC.isBusy)
                    {
                        vehicleModel.SetTarget(placeC.coors);
                        placeC.isBusy = true;
                        isCarCreated = true;
                        vehicleModel.setParkingID(placeC.id);
                        vehicleModel.isOnParkingPlace = false;
                        break;
                    }
                    else { }
            else
                foreach (TruckParkingPlace placeT in truckParkingPlaces)
                    if (!placeT.isBusy)
                    {
                        vehicleModel.SetTarget(placeT.coors);
                        placeT.isBusy = true;
                        isCarCreated = true;
                        vehicleModel.setParkingID(placeT.id);
                        vehicleModel.isOnParkingPlace = false;
                        break;
                    }
            if (isCarCreated) cars.AddLast(vehicleModel);
            reloadMap();
            Draw();
        }

        private int[,,] CreateAndInitLocalMap(AbstractVehicleModel @abstract)
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

        private Coors[] foundWay(int[,,] localMap, AbstractVehicleModel @abstract)
        {
            RunDijkstraAlgorithm(localMap, @abstract);
            return GetWay(@abstract, localMap);
        }

        private AbstractVehicleModel[] nextSystemStep(int[,,] localMap, AbstractVehicleModel vehicleModel, Coors[] way, DateTime modelDateTime)
        {
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            bool stopEnding = false;
            for (int i = 0; i < way.Length; i++)
                if (vehicleModel.GetCoors().Equals(way[i]))
                {
                    //Если машина на парковке
                    if (vehicleModel.GetCoors().Equals(vehicleModel.GetTarget()) && vehicleModel.GetTargetType() == TargetType.Parking)
                    {
                        if ((modelDateTime-vehicleModel.GetDateTimeStopping()).TotalMinutes>=vehicleModel.GetSecondsOnParking())
                        {
                            stopEnding = true;
                            vehicleModel.SetTarget(cashierCoors);
                            vehicleModel.SetTargetType(TargetType.Cashier);
                            vehicleModel.isOnParkingPlace = false;
                            if (vehicleModel.GetType() == "Car")
                            {
                                foreach (CarParkingPlace cpp in carParkingPlaces)
                                {
                                    if (cpp.coors.Equals(vehicleModel.GetCoors()))
                                    {
                                        cpp.isBusy = false;
                                        break;
                                    }  
                                }
                            }
                            else
                            {
                                foreach (var truckParkingPlace in truckParkingPlaces.Where(tpp => tpp.coors.Equals(vehicleModel.GetCoors())))
                                {
                                    truckParkingPlace.isBusy = false;
                                    break;
                                }
                            }
                        }
                    }
                    //Если машина будет на парковке на следующем шаге
                    if (i == way.Length - 2 && vehicleModel.GetTargetType() == TargetType.Parking)
                    {
                        vehicleModel.SetDateTimeStopping(modelDateTime.AddMinutes(1));
                        vehicleModel.isOnParkingPlace = true;
                    }
                    //Машина на кассе
                    if (cashierCoors.Equals(vehicleModel.GetCoors()))
                    {
                        vehicleModel.SetTarget(exitCoors);
                        vehicleModel.SetTargetType(TargetType.Exit);
                        //Логика, когда машина на кассе
                        //
                    }
                    //Машина на выезде
                    if (exitCoors.Equals(vehicleModel.GetCoors()))
                    {
                        removedCars.AddLast(vehicleModel);
                    }
                    if (vehicleModel.GetCoors().Equals(way[i]) && i != way.Length - 1)
                    {
                        vehicleModel.SetNextCoors(way[i + 1]);
                    }
                    else if (vehicleModel.GetCoors().Equals(way[i]) 
                             && i == way.Length - 1 
                             && (stopEnding || vehicleModel.GetCoors().Equals(cashierCoors)))
                    {
                        int[,,] localMapTemp = CreateAndInitLocalMap(vehicleModel);
                        Coors[] wayTemp = foundWay(localMapTemp, vehicleModel);
                       
                        for (int j = 0; j < wayTemp.Length; j++)
                        {
                            if (vehicleModel.GetCoors().Equals(wayTemp[j]))
                            {
                                vehicleModel.SetNextCoors(wayTemp[j + 1]);
                                break;
                            }
                        }

                    }
                }
            var canGo = cars.All(carLocal => !carLocal.GetCoors().Equals(vehicleModel.GetNextCoors()));
            if (localMap[vehicleModel.GetNextCoors().ColumnIndex, vehicleModel.GetNextCoors().RowIndex, 2] >= 1000 
                && !vehicleModel.GetTarget().Equals(vehicleModel.GetNextCoors()))
            {
                if (vehicleModel.GetCountErrors() < 6)
                {
                    vehicleModel.SetCountErrors(vehicleModel.GetCountErrors() + 1);
                    canGo = false;
                }
                else
                {
                    vehicleModel.SetCountErrors(0);
                    canGo = true;
                }
            }
            if (canGo) vehicleModel.Drive();
            reloadMap();
            return removedCars.ToArray();
        }

        private void RunDijkstraAlgorithm(int[,,] localMap, AbstractVehicleModel vehicleModel)
        {
            bool isFirstIteration = true;
            Coors coorsNowIteration = null;
            var allNeighbors = new LinkedList<Coors>();
            localMap[vehicleModel.GetColumnIndex(), vehicleModel.GetRowIndex(), 2] = 0;
            while (true)
            {
                var isExit = true;
                if (isFirstIteration)
                {
                    coorsNowIteration = new Coors(vehicleModel.GetColumnIndex(), vehicleModel.GetRowIndex());
                    isFirstIteration = false;
                    isExit = false;
                }
                else
                {
                    var allNeighborsArr = allNeighbors.ToArray();
                    Array.Sort(allNeighborsArr, GetCoorsComparison(localMap));
                    foreach (var coor in allNeighborsArr)
                    {
                        if (localMap[coor.ColumnIndex, coor.RowIndex, 1] == 0)
                        {
                            isExit = false;
                            coorsNowIteration = coor;
                            allNeighbors.Remove(coor);
                            break;
                        }

                        allNeighbors.Remove(coor);
                    }
                }

                if (isExit) break;
                var neighbors = GetNeighbors(coorsNowIteration.ColumnIndex, coorsNowIteration.RowIndex, localMap);

                foreach (var coor in neighbors)
                {
                    if (localMap[coor.ColumnIndex, coor.RowIndex, 1] == 0 &&
                        localMap[coor.ColumnIndex, coor.RowIndex, 2] >
                        localMap[coor.ColumnIndex, coor.RowIndex, 0] + localMap[coorsNowIteration.ColumnIndex,
                            coorsNowIteration.RowIndex, 2])
                    {
                        localMap[coor.ColumnIndex, coor.RowIndex, 2] =
                            localMap[coor.ColumnIndex, coor.RowIndex, 0] +
                            localMap[coorsNowIteration.ColumnIndex, coorsNowIteration.RowIndex, 2];
                    }

                    allNeighbors.AddLast(coor);
                }

                localMap[coorsNowIteration.ColumnIndex, coorsNowIteration.RowIndex, 1] = 1;
            }

        }

        private Coors[] GetNeighbors(int indexCol, int indexRow, int[,,] localMap)
        {
            var coors = new LinkedList<Coors>();
            //Свободная точка
            if (indexCol > 0 && indexCol < model.ColumnCount - 1 && indexRow > 0 && indexRow < model.RowCount - 1)
            {
                coors.AddLast(new Coors(indexCol - 1, indexRow));
                coors.AddLast(new Coors(indexCol + 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow - 1));
                coors.AddLast(new Coors(indexCol, indexRow + 1));
            }
            //Прилегает к стене
            if (indexCol == model.ColumnCount - 1 && indexRow > 0 && indexRow < model.RowCount - 1)
            {
                coors.AddLast(new Coors(indexCol, indexRow + 1));
                coors.AddLast(new Coors(indexCol, indexRow - 1));
                coors.AddLast(new Coors(indexCol - 1, indexRow));
            }
            if (indexCol == 0 && indexRow > 0 && indexRow < model.RowCount - 1)
            {
                coors.AddLast(new Coors(indexCol, indexRow + 1));
                coors.AddLast(new Coors(indexCol, indexRow - 1));
                coors.AddLast(new Coors(indexCol + 1, indexRow));
            }
            if (indexCol > 0 && indexCol < model.ColumnCount - 1 && indexRow == model.RowCount - 1)
            {
                coors.AddLast(new Coors(indexCol + 1, indexRow));
                coors.AddLast(new Coors(indexCol - 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow - 1));
            }
            if (indexCol > 0 && indexCol < model.ColumnCount - 1 && indexRow == 0)
            {
                coors.AddLast(new Coors(indexCol + 1, indexRow));
                coors.AddLast(new Coors(indexCol - 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow + 1));
            }
            //Прилегает к углу
            if (indexCol == 0 && indexRow == 0)
            {
                coors.AddLast(new Coors(indexCol + 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow + 1));
            }
            if (indexCol == model.ColumnCount - 1 && indexRow == model.RowCount - 1)
            {
                coors.AddLast(new Coors(indexCol - 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow - 1));
            }
            if (indexCol == 0 && indexRow == model.RowCount - 1)
            {
                coors.AddLast(new Coors(indexCol + 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow - 1));
            }
            if (indexCol == model.ColumnCount - 1 && indexRow == 0)
            {
                coors.AddLast(new Coors(indexCol - 1, indexRow));
                coors.AddLast(new Coors(indexCol, indexRow + 1));
            }
            Coors[] coorsArr = coors.ToArray();
            Array.Sort(coorsArr, GetCoorsComparison(localMap));
            return coorsArr;
        }

        private Coors[] GetWay(AbstractVehicleModel vehicleModel, int[,,] localMap)
        {
            LinkedList<Coors> coors = new LinkedList<Coors>();
            int weight = localMap[vehicleModel.GetTarget().ColumnIndex, vehicleModel.GetTarget().RowIndex, 2];
            Coors[] neighboreCoors = null;
            coors.AddLast(new Coors(vehicleModel.GetTarget().ColumnIndex, vehicleModel.GetTarget().RowIndex));
            Coors mainCoors = new Coors(vehicleModel.GetTarget().ColumnIndex, vehicleModel.GetTarget().RowIndex);
            while (weight != 0)
            {
                neighboreCoors = GetNeighbors(mainCoors.ColumnIndex, mainCoors.RowIndex, localMap);
                for (var index = 0; index < neighboreCoors.Length; index++)
                {
                    Coors coorsTemp = neighboreCoors[index];
                    if (localMap[mainCoors.ColumnIndex, mainCoors.RowIndex, 2] ==
                        localMap[coorsTemp.ColumnIndex, coorsTemp.RowIndex, 2] +
                        localMap[mainCoors.ColumnIndex, mainCoors.RowIndex, 0])
                    {
                        weight -= localMap[mainCoors.ColumnIndex, mainCoors.RowIndex, 0];
                        mainCoors = coorsTemp;
                        coors.AddLast(mainCoors);
                        break;
                    }
                }
            }
            Coors[] returnCoors = coors.ToArray();
            Array.Reverse(returnCoors);
            return returnCoors;
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

        public void nextStep(DateTime modelDateTime)
        {
            int[,,] localMap = null;
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            foreach (AbstractVehicleModel car in cars)
            {
                localMap = CreateAndInitLocalMap(car);
                Coors[] way = foundWay(localMap, car);
                AbstractVehicleModel[] remCars = nextSystemStep(localMap, car, way, modelDateTime);
                for (int i = 0; i < remCars.Length; i++)
                    removedCars.AddLast(remCars[i]);
            }
            try
            {
                foreach (AbstractVehicleModel carTemp in removedCars)
                    cars.Remove(carTemp);
            }
            catch (Exception) { }
            Draw();
        }

        private Comparison<Coors> GetCoorsComparison(int[,,] localMap)
        {
            return (a, b) => localMap[a.ColumnIndex, a.RowIndex, 2] - localMap[b.ColumnIndex, b.RowIndex, 2];
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

        public LinkedList<AbstractParkingPlace> getParkingPlaces()
        {
            LinkedList<AbstractParkingPlace> places = new LinkedList<AbstractParkingPlace>();
            foreach (AbstractParkingPlace placeC in carParkingPlaces)
                places.AddLast(placeC);
            foreach (AbstractParkingPlace placeT in truckParkingPlaces)
                places.AddLast(placeT);
            return places;
        }
    }
}
