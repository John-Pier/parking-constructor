using ParkingConstructorLib.logic;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections;
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
        private LinkedList<CarParkingPlace> carParkingPlaces;
        private LinkedList<TruckParkingPlace> truckParkingPlaces;
        private Coors cashierCoors = null;
        private Coors exitCoors = null;
        private Random rnd;
        private Bitmap drawMapOriginal;
        private Bitmap[] textures;
        public MapAvailable(ParkingModel<T> model, Bitmap[] textures)
        {
            this.model = model;
            this.textures = textures;
            map = new bool[model.ColumnCount, model.RowCount];
            cars = new LinkedList<AbstractVehicleModel>();
            carParkingPlaces = new LinkedList<CarParkingPlace>();
            truckParkingPlaces = new LinkedList<TruckParkingPlace>();
            rnd = new Random();
            reloadMap();
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                {
                    if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Entry)
                        AbstractVehicleModel.SetSpawnPoint(i, j);
                    if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.ParkingSpace)
                        carParkingPlaces.AddLast(new CarParkingPlace(new Coors(i, j)));
                    if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.TruckParkingSpace)
                        truckParkingPlaces.AddLast(new TruckParkingPlace(new Coors(i, j)));
                    if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Cashier)
                        cashierCoors = new Coors(i, j);
                    if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Exit)
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

        private AbstractVehicleModel[] nextSystemStep(int[,,] localMap, AbstractVehicleModel @abstract, Coors[] way, double accelerate)
        {
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            bool stopEnding = false;
            for (int i = 0; i < way.Length; i++)
                if (@abstract.GetCoors().Equals(way[i]))
                {
                    //Если машина на парковке
                    if (@abstract.GetCoors().Equals(@abstract.GetTarget()) && @abstract.GetTargetType() == TargetType.Parking)
                    {
                        if ((DateTime.Now - @abstract.GetDateTimeStopping()).TotalMilliseconds > (@abstract.GetSecondsOnParking() * 1000) / accelerate)
                        {
                            stopEnding = true;
                            @abstract.SetTarget(cashierCoors);
                            @abstract.SetTargetType(TargetType.Cashier);
                            if (@abstract.GetType() == "Car")
                            {
                                foreach (CarParkingPlace cpp in carParkingPlaces)
                                    if (cpp.coors.Equals(@abstract.GetCoors()))
                                    {
                                        cpp.Abstract = null;
                                        cpp.isBusy = false;
                                        break;
                                    }
                            }
                            else
                            {
                                foreach (var truckParkingPlace in truckParkingPlaces.Where(tpp => tpp.coors.Equals(@abstract.GetCoors())))
                                {
                                    truckParkingPlace.Abstract = null;
                                    truckParkingPlace.isBusy = false;
                                    break;
                                }
                            }
                        }
                    }
                    //Если машина будет на парковке на следующем шаге
                    if (i == way.Length - 2 && @abstract.GetTargetType() == TargetType.Parking)
                    {
                        @abstract.SetDateTimeStopping(DateTime.Now);
                        if (@abstract.GetType() == "Car")
                        {
                            foreach (var carParkingPlace in carParkingPlaces.Where(carParkingPlace => carParkingPlace.coors.Equals(way[i + 1])))
                            {
                                carParkingPlace.Abstract = @abstract;
                                break;
                            }
                        }
                        else
                        {
                            foreach (TruckParkingPlace tpp in truckParkingPlaces)
                                if (tpp.coors.Equals(way[i + 1]))
                                {
                                    tpp.Abstract = @abstract;
                                    break;
                                }
                        }
                    }
                    //Машина на кассе
                    if (cashierCoors.Equals(@abstract.GetCoors()))
                    {
                        @abstract.SetTarget(exitCoors);
                        @abstract.SetTargetType(TargetType.Exit);
                        //Логика, когда машина на кассе
                        //
                    }
                    //Машина на выезде
                    if (exitCoors.Equals(@abstract.GetCoors()))
                    {
                        removedCars.AddLast(@abstract);
                    }
                    if (@abstract.GetCoors().Equals(way[i]) && i != way.Length - 1)
                    {
                        @abstract.SetNextCoors(way[i + 1]);
                    }
                    else if (@abstract.GetCoors().Equals(way[i]) 
                             && i == way.Length - 1 
                             && (stopEnding || @abstract.GetCoors().Equals(cashierCoors)))
                    {
                        int[,,] localMapTemp = initLocalMap(@abstract);
                        Coors[] wayTemp = foundWay(localMapTemp, @abstract);
                        //Ничего не трогать туть, несмотря на предупреждение о недостижимом коде
                        for (int j = 0; j < wayTemp.Length; j++)
                        {
                            if (@abstract.GetCoors().Equals(wayTemp[i]))
                                @abstract.SetNextCoors(wayTemp[i + 1]);
                            break;
                        }

                    }
                }
            var canGo = cars.All(carLocal => !carLocal.GetCoors().Equals(@abstract.GetNextCoors()));
            if (localMap[@abstract.GetNextCoors().ColumnIndex, @abstract.GetNextCoors().RowIndex, 2] >= 1000 
                && !@abstract.GetTarget().Equals(@abstract.GetNextCoors()))
            {
                if (@abstract.GetCountErrors() < 6)
                {
                    @abstract.SetCountErrors(@abstract.GetCountErrors() + 1);
                    canGo = false;
                }
                else
                {
                    @abstract.SetCountErrors(0);
                    canGo = true;
                }
            }
            if (canGo) @abstract.Drive();
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

        public bool IsCanAddCar(CarType carType)
        {
            if (carType == CarType.Car)
            {
                foreach (CarParkingPlace placeForCar in carParkingPlaces)
                {
                    if (!placeForCar.isBusy)
                        return true;
                }
                return false;
            }
            else
            {
                foreach (TruckParkingPlace placeForTruck in truckParkingPlaces)
                {
                    if (!placeForTruck.isBusy)
                        return true;
                }
                return false;
            }
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

        public Comparison<Coors> GetCoorsComparison(int[,,] localMap)
        {
            return (a, b) => localMap[a.ColumnIndex, a.RowIndex, 2] - localMap[b.ColumnIndex, b.RowIndex, 2];
        }
    }
}
