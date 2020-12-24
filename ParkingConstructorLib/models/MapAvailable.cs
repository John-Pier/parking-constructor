using ParkingConstructorLib.logic;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class MapAvailable<T> where T : class
    {
        private bool[,] map;
        private ParkingModel<T> model;
        private LinkedList<CarVehicleModel> cars;
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
            cars = new LinkedList<CarVehicleModel>();
            carParkingPlaces = new LinkedList<CarParkingPlace>();
            truckParkingPlaces = new LinkedList<TruckParkingPlace>();
            rnd = new Random();
            reloadMap();
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                {
                    if (model.GetElement(i, j) != null && model.GetElement(i, j).GetElementType() == ParkingModelElementType.Entry)
                        CarVehicleModel.setSpawnPoint(i, j);
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
            for(int i = 0; i< model.ColumnCount; i++)
                for(int j = 0; j<model.RowCount; j++)
                    for(int k = 0; k < 10; k++)
                        for(int l = 0; l<10; l++)
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
            ParkingSceneVisualization<Image>.setImage(drawMapOriginal);
        }
        public void Draw()
        {
            Bitmap result = (Bitmap)drawMapOriginal.Clone();
            foreach (CarVehicleModel car in cars)
            {
                for (int i = 0; i < model.ColumnCount; i++)
                    for (int j = 0; j < model.RowCount; j++)
                        if(isCoorsEquals(car.getCoors(), new Coors(i,j)))
                            for (int k = 0; k < 10; k++)
                                for (int l = 0; l < 10; l++)
                                {
                                    if(car.GetType() == "Car" && car.getLastDirection() == CarVehicleModel.LastDirection.Horizontal && textures[6].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[6].GetPixel(k, l));
                                    if (car.GetType() == "Car" && car.getLastDirection() == CarVehicleModel.LastDirection.Vertical && textures[7].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[7].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.getLastDirection() == CarVehicleModel.LastDirection.Horizontal && textures[8].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[8].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.getLastDirection() == CarVehicleModel.LastDirection.Vertical && textures[9].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * 10 + k, j * 10 + l, textures[9].GetPixel(k, l));
                                }
            }
            ParkingSceneVisualization<Image>.setImage(result);
        }
        public void reloadMap()
        {
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    map[i, j] = model.GetElement(i, j) == null ? true : false;
            foreach (CarVehicleModel car in cars)
                map[car.getColumnIndex(), car.getRowIndex()] = false;
        }
        public void addCar(CarVehicleModel car)
        {
            bool isCarCreated = false;
            if (car.GetType().Equals("Car"))
                foreach (CarParkingPlace placeC in carParkingPlaces)
                    if (!placeC.isBusy)
                    {
                        car.setTarget(placeC.coors);
                        placeC.isBusy = true;
                        isCarCreated = true;
                        break;
                    }
                    else { }
            else
                foreach (TruckParkingPlace placeT in truckParkingPlaces)
                    if (!placeT.isBusy)
                    {
                        car.setTarget(placeT.coors);
                        placeT.isBusy = true;
                        isCarCreated = true;
                        break;
                    }
            if(isCarCreated)cars.AddLast(car);
            reloadMap();
            Draw();
        }

        private int[,,] initLocalMap(CarVehicleModel car)
        {
            int[,,] localMap = new int[model.ColumnCount, model.RowCount, 3];
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                {
                    localMap[i, j, 0] = map[i, j] ? 1 : 1000;//Карта весов
                    localMap[i, j, 1] = 0; //0 - непосещённая; 1 - посещённая
                    localMap[i, j, 2] = 100000; //Сумма весов до точки
                }

            foreach (CarVehicleModel carTemp in cars)
                localMap[carTemp.getColumnIndex(), carTemp.getRowIndex(), 0] = 2000;
            return localMap;
        }

        private Coors[] foundWay(int[,,] localMap, CarVehicleModel car)
        {
            dekstra(localMap, car);
            return getWay(car, localMap);
        }

        private CarVehicleModel[] nextSystemStep(int[,,] localMap, CarVehicleModel car, Coors[] way, double accelerate)
        {
            LinkedList<CarVehicleModel> removedCars = new LinkedList<CarVehicleModel>();
            bool stopEnding = false;
            for(int i = 0; i<way.Length; i++)
                if(isCoorsEquals(car.getCoors(), way[i]))
                {
                    //Если машина на парковке
                    if (isCoorsEquals(car.getCoors(), car.getTarget()) && car.getTargetType() == CarVehicleModel.TargetType.Parking)
                    {
                        if((DateTime.Now - car.getDateTimeStopping()).TotalMilliseconds > (car.getSecondsOnParking() * 1000)/accelerate)
                        {
                            stopEnding = true;
                            car.setTarget(cashierCoors);
                            car.setTargetType(CarVehicleModel.TargetType.Cashier);
                            if (car.GetType() == "Car")
                            {
                                foreach (CarParkingPlace cpp in carParkingPlaces)
                                    if (isCoorsEquals(cpp.coors, car.getCoors()))
                                    {
                                        cpp.car = null;
                                        cpp.isBusy = false;
                                        break;
                                    }
                            }
                            else
                            {
                                foreach (TruckParkingPlace tpp in truckParkingPlaces)
                                    if (isCoorsEquals(tpp.coors, car.getCoors()))
                                    {
                                        tpp.car = null;
                                        tpp.isBusy = false;
                                        break;
                                    }
                            }
                        }
                    }
                    //Если машина будет на парковке на следующем шаге
                    if(i == way.Length - 2 && car.getTargetType() == CarVehicleModel.TargetType.Parking)
                    {
                        car.setDateTimeStopping(DateTime.Now);
                        if(car.GetType() == "Car")
                        {
                            foreach (CarParkingPlace cpp in carParkingPlaces)
                                if(isCoorsEquals(cpp.coors, way[i + 1]))
                                {
                                    cpp.car = car;
                                    break;
                                }
                        }
                        else
                        {
                            foreach (TruckParkingPlace tpp in truckParkingPlaces)
                                if (isCoorsEquals(tpp.coors, way[i + 1]))
                                {
                                    tpp.car = car;
                                    break;
                                }
                        }
                    }
                    //Машина на кассе
                    if(isCoorsEquals(cashierCoors, car.getCoors()))
                    {
                        car.setTarget(exitCoors);
                        car.setTargetType(CarVehicleModel.TargetType.Exit);
                        //Логика, когда машина на кассе
                        //
                    }
                    //Машина на выезде
                    if(isCoorsEquals(exitCoors, car.getCoors()))
                    {
                        removedCars.AddLast(car);
                    }
                    if (isCoorsEquals(car.getCoors(), way[i]) && i != way.Length-1)
                    {
                        car.setNextCoors(way[i + 1]);
                    }
                    else if((isCoorsEquals(car.getCoors(), way[i]) && i == way.Length - 1) &&
                        (stopEnding || isCoorsEquals(car.getCoors(), cashierCoors)))
                    {
                        int[,,] localMapTemp = initLocalMap(car);
                        Coors[] wayTemp = foundWay(localMapTemp, car);
                        //Ничего не трогать туть, несмотря на предупреждение о недостижимом коде
                        for(int j = 0; j<wayTemp.Length; j++)
                        {
                            if (isCoorsEquals(car.getCoors(), wayTemp[i]))
                                car.setNextCoors(wayTemp[i + 1]);
                            break;
                        }
                            
                    }
                }
            bool canGo = true;
            foreach(CarVehicleModel carLocal in cars)
            {
                if(isCoorsEquals(carLocal.getCoors(), car.getNextCoors())){
                    canGo = false;
                    break;
                }
            }
            if (localMap[car.getNextCoors().columnIndex, car.getNextCoors().rowIndex, 2] >= 1000 && !isCoorsEquals(car.getTarget(), car.getNextCoors()))
            {
                if(car.getCountErrors() < 6)
                {
                    car.setCountErrors(car.getCountErrors()+1);
                    canGo = false;
                }
                else
                {
                    car.setCountErrors(0);
                    canGo = true;
                }
            }
            if (canGo) car.drive();
            reloadMap();
            return removedCars.ToArray();
        }

        private void dekstra(int[,,] localMap, CarVehicleModel car)
        {
            bool isFirstIteration = true;
            Coors coorsNowIteration = null;
            Coors[] neighbors = null;
            bool isExit = true;
            LinkedList<Coors> allNeighbors = new LinkedList<Coors>();
            Coors[] allNeighborsArr = null;
            localMap[car.getColumnIndex(), car.getRowIndex(), 2] = 0;
            while (true)
            {
                isExit = true;
                if (isFirstIteration)
                {
                    coorsNowIteration = new Coors(car.getColumnIndex(), car.getRowIndex());
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

        private Coors[] getWay(CarVehicleModel car, int[,,] localMap)
        {
            LinkedList<Coors> coors = new LinkedList<Coors>();
            int weight = localMap[car.getTarget().columnIndex, car.getTarget().rowIndex, 2];
            Coors[] neighboreCoors = null;
            coors.AddLast(new Coors(car.getTarget().columnIndex, car.getTarget().rowIndex));
            Coors mainCoors = new Coors(car.getTarget().columnIndex, car.getTarget().rowIndex);
            while(weight != 0)
            {
                neighboreCoors = getNeighbors(mainCoors.columnIndex, mainCoors.rowIndex, localMap);
                foreach(Coors coorsTemp in neighboreCoors)
                {
                    if(localMap[mainCoors.columnIndex, mainCoors.rowIndex, 2] ==
                        localMap[coorsTemp.columnIndex, coorsTemp.rowIndex, 2]+
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

        public void nextStep(double accelerate)
        {
            int[,,] localMap = null;
            LinkedList<CarVehicleModel> removedCars = new LinkedList<CarVehicleModel>();
            foreach (CarVehicleModel car in cars)
            {
                localMap = initLocalMap(car);
                Coors[] way = foundWay(localMap, car);
                CarVehicleModel[] remCars = nextSystemStep(localMap, car, way, accelerate);
                for (int i = 0; i < remCars.Length; i++)
                    removedCars.AddLast(remCars[i]);
            }
            try
            {
                foreach (CarVehicleModel carTemp in removedCars)
                    cars.Remove(carTemp);
            }
            catch(Exception e)
            {

            }
            Draw();
        }
    }
}
