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
        private Random rnd;
        private Bitmap drawMapOriginal;
        private Bitmap[] textures;
        
        public int SpawnRow;
        public  int SpawnCol;

        public MapAvailable(ParkingModel<T> model, Bitmap[] textures)
        {
            this.model = model;
            this.textures = textures;
            map = new bool[model.ColumnCount, model.RowCount];
            cars = new LinkedList<AbstractVehicleModel>();
            carParkingPlaces = new LinkedList<AbstractParkingPlace>();
            truckParkingPlaces = new LinkedList<AbstractParkingPlace>();
            rnd = new Random();
            reloadMap();
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
            drawMapOriginal = new Bitmap(model.ColumnCount * 10, model.RowCount * 10);
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    for (int k = 0; k < 10; k++)
                        for (int l = 0; l < 10; l++)
                        {
                            if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Entry)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[0].GetPixel(k, l));
                            if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Exit)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[1].GetPixel(k, l));
                            if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Cashier)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[2].GetPixel(k, l));
                            if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.ParkingSpace)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[4].GetPixel(k, l));
                            if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.TruckParkingSpace)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[5].GetPixel(k, l));
                            if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Grass)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[3].GetPixel(k, l));
                            if (model.GetElement(i, j) == null || model.GetElement(i, j).GetElementType() == ParkingModelElementType.Road)
                                drawMapOriginal.SetPixel(i * 10 + k, j * 10 + l, textures[10].GetPixel(k, l));
                        }
            ParkingSceneVisualization<Image>.SetImage(drawMapOriginal);
        }
        public void Draw()
        {
            Bitmap result = (Bitmap)drawMapOriginal.Clone();
            foreach (AbstractVehicleModel car in cars)
            {
                for (int i = 0; i < model.ColumnCount; i++)
                    for (int j = 0; j < model.RowCount; j++)
                        if (car.GetCoors().Equals(new Coors(i, j)))
                            for (int k = 0; k < 10; k++)
                                for (int l = 0; l < 10; l++)
                                {
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Horizontal && textures[6].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[6].GetPixel(k, l));
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Vertical && textures[7].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[7].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Horizontal && textures[8].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[8].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Vertical && textures[9].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[9].GetPixel(k, l));
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
        public void addCar(AbstractVehicleModel @abstract)
        {
            bool isCarCreated = false;
            if (@abstract.GetType().Equals("Car"))
                foreach (CarParkingPlace placeC in carParkingPlaces)
                    if (!placeC.isBusy)
                    {
                        @abstract.SetTarget(placeC.coors);
                        placeC.isBusy = true;
                        isCarCreated = true;
                        break;
                    }
                    else { }
            else
                foreach (TruckParkingPlace placeT in truckParkingPlaces)
                    if (!placeT.isBusy)
                    {
                        @abstract.SetTarget(placeT.coors);
                        placeT.isBusy = true;
                        isCarCreated = true;
                        break;
                    }
            if (isCarCreated) cars.AddLast(@abstract);
            reloadMap();
            Draw();
        }

        private int[,,] initLocalMap(AbstractVehicleModel @abstract)
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

        private AbstractVehicleModel[] nextSystemStep(int[,,] localMap, AbstractVehicleModel vehicleModel, Coors[] way, double accelerate)
        {
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            bool stopEnding = false;
            for (int i = 0; i < way.Length; i++)
                if (vehicleModel.GetCoors().Equals(way[i]))
                {
                    //Если машина на парковке
                    if (vehicleModel.GetCoors().Equals(vehicleModel.GetTarget()) && vehicleModel.GetTargetType() == TargetType.Parking)
                    {
                        if ((DateTime.Now - vehicleModel.GetDateTimeStopping()).TotalMilliseconds > (vehicleModel.GetSecondsOnParking() * 1000) / accelerate)
                        {
                            stopEnding = true;
                            vehicleModel.SetTarget(cashierCoors);
                            vehicleModel.SetTargetType(TargetType.Cashier);
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
                        vehicleModel.SetDateTimeStopping(DateTime.Now);
                        if (vehicleModel.GetType() == "Car")
                        {
                            foreach (var carParkingPlace in carParkingPlaces.Where(carParkingPlace => carParkingPlace.coors.Equals(way[i + 1])))
                            {
                                break;
                            }
                        }
                        else
                        {
                            foreach (var place in truckParkingPlaces.Where(tpp => tpp.coors.Equals(way[i + 1])))
                            {
                                break;
                            }
                        }
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
                        int[,,] localMapTemp = initLocalMap(vehicleModel);
                        Coors[] wayTemp = foundWay(localMapTemp, vehicleModel);
                        //Ничего не трогать туть, несмотря на предупреждение о недостижимом коде
                        for (int j = 0; j < wayTemp.Length; j++)
                        {
                            if (vehicleModel.GetCoors().Equals(wayTemp[i]))
                                vehicleModel.SetNextCoors(wayTemp[i + 1]);
                            break;
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

        private void RunDijkstraAlgorithm(int[,,] localMap, AbstractVehicleModel @abstract)
        {
            bool isFirstIteration = true;
            Coors coorsNowIteration = null;
            var allNeighbors = new LinkedList<Coors>();
            localMap[@abstract.GetColumnIndex(), @abstract.GetRowIndex(), 2] = 0;
            while (true)
            {
                var isExit = true;
                if (isFirstIteration)
                {
                    coorsNowIteration = new Coors(@abstract.GetColumnIndex(), @abstract.GetRowIndex());
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

        private Coors[] GetWay(AbstractVehicleModel @abstract, int[,,] localMap)
        {
            LinkedList<Coors> coors = new LinkedList<Coors>();
            int weight = localMap[@abstract.GetTarget().ColumnIndex, @abstract.GetTarget().RowIndex, 2];
            Coors[] neighboreCoors = null;
            coors.AddLast(new Coors(@abstract.GetTarget().ColumnIndex, @abstract.GetTarget().RowIndex));
            Coors mainCoors = new Coors(@abstract.GetTarget().ColumnIndex, @abstract.GetTarget().RowIndex);
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

        public void nextStep(double accelerate)
        {
            int[,,] localMap = null;
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            foreach (AbstractVehicleModel car in cars)
            {
                localMap = initLocalMap(car);
                Coors[] way = foundWay(localMap, car);
                AbstractVehicleModel[] remCars = nextSystemStep(localMap, car, way, accelerate);
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
    }
}
