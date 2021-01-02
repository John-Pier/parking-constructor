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
                        AbstractVehicleModel.setSpawnPoint(i, j);
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
                        if (isCoorsEquals(car.GetCoors(), new Coors(i, j)))
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
            dekstra(localMap, @abstract);
            return getWay(@abstract, localMap);
        }

        private AbstractVehicleModel[] nextSystemStep(int[,,] localMap, AbstractVehicleModel @abstract, Coors[] way, double accelerate)
        {
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            bool stopEnding = false;
            for (int i = 0; i < way.Length; i++)
                if (isCoorsEquals(@abstract.GetCoors(), way[i]))
                {
                    //Если машина на парковке
                    if (isCoorsEquals(@abstract.GetCoors(), @abstract.GetTarget()) && @abstract.GetTargetType() == TargetType.Parking)
                    {
                        if ((DateTime.Now - @abstract.GetDateTimeStopping()).TotalMilliseconds > (@abstract.GetSecondsOnParking() * 1000) / accelerate)
                        {
                            stopEnding = true;
                            @abstract.SetTarget(cashierCoors);
                            @abstract.SetTargetType(TargetType.Cashier);
                            if (@abstract.GetType() == "Car")
                            {
                                foreach (CarParkingPlace cpp in carParkingPlaces)
                                    if (isCoorsEquals(cpp.coors, @abstract.GetCoors()))
                                    {
                                        cpp.Abstract = null;
                                        cpp.isBusy = false;
                                        break;
                                    }
                            }
                            else
                            {
                                foreach (TruckParkingPlace tpp in truckParkingPlaces)
                                    if (isCoorsEquals(tpp.coors, @abstract.GetCoors()))
                                    {
                                        tpp.Abstract = null;
                                        tpp.isBusy = false;
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
                            foreach (CarParkingPlace cpp in carParkingPlaces)
                                if (isCoorsEquals(cpp.coors, way[i + 1]))
                                {
                                    cpp.Abstract = @abstract;
                                    break;
                                }
                        }
                        else
                        {
                            foreach (TruckParkingPlace tpp in truckParkingPlaces)
                                if (isCoorsEquals(tpp.coors, way[i + 1]))
                                {
                                    tpp.Abstract = @abstract;
                                    break;
                                }
                        }
                    }
                    //Машина на кассе
                    if (isCoorsEquals(cashierCoors, @abstract.GetCoors()))
                    {
                        @abstract.SetTarget(exitCoors);
                        @abstract.SetTargetType(TargetType.Exit);
                        //Логика, когда машина на кассе
                        //
                    }
                    //Машина на выезде
                    if (isCoorsEquals(exitCoors, @abstract.GetCoors()))
                    {
                        removedCars.AddLast(@abstract);
                    }
                    if (isCoorsEquals(@abstract.GetCoors(), way[i]) && i != way.Length - 1)
                    {
                        @abstract.SetNextCoors(way[i + 1]);
                    }
                    else if ((isCoorsEquals(@abstract.GetCoors(), way[i]) && i == way.Length - 1) &&
                        (stopEnding || isCoorsEquals(@abstract.GetCoors(), cashierCoors)))
                    {
                        int[,,] localMapTemp = initLocalMap(@abstract);
                        Coors[] wayTemp = foundWay(localMapTemp, @abstract);
                        //Ничего не трогать туть, несмотря на предупреждение о недостижимом коде
                        for (int j = 0; j < wayTemp.Length; j++)
                        {
                            if (isCoorsEquals(@abstract.GetCoors(), wayTemp[i]))
                                @abstract.SetNextCoors(wayTemp[i + 1]);
                            break;
                        }

                    }
                }
            bool canGo = true;
            foreach (AbstractVehicleModel carLocal in cars)
            {
                if (isCoorsEquals(carLocal.GetCoors(), @abstract.GetNextCoors()))
                {
                    canGo = false;
                    break;
                }
            }
            if (localMap[@abstract.GetNextCoors().columnIndex, @abstract.GetNextCoors().rowIndex, 2] >= 1000 && !isCoorsEquals(@abstract.GetTarget(), @abstract.GetNextCoors()))
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

        private void dekstra(int[,,] localMap, AbstractVehicleModel @abstract)
        {
            bool isFirstIteration = true;
            Coors coorsNowIteration = null;
            Coors[] neighbors = null;
            bool isExit = true;
            LinkedList<Coors> allNeighbors = new LinkedList<Coors>();
            Coors[] allNeighborsArr = null;
            localMap[@abstract.GetColumnIndex(), @abstract.GetRowIndex(), 2] = 0;
            while (true)
            {
                isExit = true;
                if (isFirstIteration)
                {
                    coorsNowIteration = new Coors(@abstract.GetColumnIndex(), @abstract.GetRowIndex());
                    isFirstIteration = false;
                    isExit = false;
                }
                else
                {
                    allNeighborsArr = allNeighbors.ToArray();
                    Array.Sort(allNeighborsArr);
                    foreach (Coors coor in allNeighborsArr)
                    {
                        if (localMap[coor.columnIndex, coor.rowIndex, 1] == 0)
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
                neighbors = getNeighbors(coorsNowIteration.columnIndex, coorsNowIteration.rowIndex, localMap);

                foreach (Coors coor in neighbors)
                {
                    if (localMap[coor.columnIndex, coor.rowIndex, 1] == 0 &&
                        localMap[coor.columnIndex, coor.rowIndex, 2] >
                        localMap[coor.columnIndex, coor.rowIndex, 0] + localMap[coorsNowIteration.columnIndex, coorsNowIteration.rowIndex, 2])
                    {
                        localMap[coor.columnIndex, coor.rowIndex, 2] =
                            localMap[coor.columnIndex, coor.rowIndex, 0] +
                            localMap[coorsNowIteration.columnIndex, coorsNowIteration.rowIndex, 2];
                    }
                    allNeighbors.AddLast(coor);
                }

                localMap[coorsNowIteration.columnIndex, coorsNowIteration.rowIndex, 1] = 1;
            }

        }

        private Coors[] getNeighbors(int indexCol, int indexRow, int[,,] localMap)
        {
            LinkedList<Coors> coors = new LinkedList<Coors>();
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
            Coors.setLocalMap(localMap);
            Array.Sort(coorsArr);
            return coorsArr;
        }

        private Coors[] getWay(AbstractVehicleModel @abstract, int[,,] localMap)
        {
            LinkedList<Coors> coors = new LinkedList<Coors>();
            int weight = localMap[@abstract.GetTarget().columnIndex, @abstract.GetTarget().rowIndex, 2];
            Coors[] neighboreCoors = null;
            coors.AddLast(new Coors(@abstract.GetTarget().columnIndex, @abstract.GetTarget().rowIndex));
            Coors mainCoors = new Coors(@abstract.GetTarget().columnIndex, @abstract.GetTarget().rowIndex);
            while (weight != 0)
            {
                neighboreCoors = getNeighbors(mainCoors.columnIndex, mainCoors.rowIndex, localMap);
                foreach (Coors coorsTemp in neighboreCoors)
                {
                    if (localMap[mainCoors.columnIndex, mainCoors.rowIndex, 2] ==
                        localMap[coorsTemp.columnIndex, coorsTemp.rowIndex, 2] +
                        localMap[mainCoors.columnIndex, mainCoors.rowIndex, 0])
                    {
                        weight -= localMap[mainCoors.columnIndex, mainCoors.rowIndex, 0];
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

        private bool isCoorsEquals(Coors coors1, Coors coors2)
        {
            if (coors1.columnIndex == coors2.columnIndex && coors1.rowIndex == coors2.rowIndex)
                return true;
            return false;
        }

        public bool isCanAddCar(CarType carType)
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
    }
}
