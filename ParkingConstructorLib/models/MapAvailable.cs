using ParkingConstructorLib.logic;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
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
        public MapAvailable(ParkingModel<T> model)
        {
            this.model = model;
            map = new bool[model.ColumnCount, model.RowCount];
            cars = new LinkedList<CarVehicleModel>();
            carParkingPlaces = new LinkedList<CarParkingPlace>();
            truckParkingPlaces = new LinkedList<TruckParkingPlace>();
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
                }
        }
        public void reloadMap()
        {
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    map[i, j] = true ? model.GetElement(i, j) == null : false;
            foreach (CarVehicleModel car in cars)
                map[car.getColumnIndex(), car.getRowIndex()] = false;
        }
        public void printMap()
        {
            for (int i = 0; i < model.RowCount; i++)
            {
                for (int j = 0; j < model.ColumnCount; j++)
                    Console.Write(map[j, i] + " ");
                Console.WriteLine();
            }
        }
        public void printLocalMap(int[,,] localMap, int mode)
        {
            for (int i = 0; i < model.RowCount; i++)
            {
                for (int j = 0; j < model.ColumnCount; j++)
                    Console.Write("{0,8}", localMap[j, i, mode]);
                Console.WriteLine();
            }
        }
        public void addCar(CarVehicleModel car)
        {
            if (car.GetType().Equals("Car"))
                foreach (CarParkingPlace placeC in carParkingPlaces)
                    if (!placeC.isBusy)
                    {
                        car.setTarget(placeC.coors);
                        placeC.isBusy = true;
                    }
                    else { }
            else
                foreach (TruckParkingPlace placeT in truckParkingPlaces)
                    if (!placeT.isBusy)
                    {
                        car.setTarget(placeT.coors);
                        placeT.isBusy = true;
                    }
            cars.AddLast(car);
            reloadMap();
        }

        private void initLocalMap(int[,,] localMap, CarVehicleModel car)
        {
            localMap = new int[model.ColumnCount, model.RowCount, 3];
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                {
                    localMap[i, j, 0] = map[i, j] ? 1 : 1000;//Карта весов
                    localMap[i, j, 1] = 0; //0 - непосещённая; 1 - посещённая
                    localMap[i, j, 2] = 100000; //Сумма весов до точки
                }

            foreach (CarVehicleModel carTemp in cars)
                localMap[carTemp.getColumnIndex(), carTemp.getRowIndex(), 0] = 1000;

            dekstra(localMap, car);
            Coors[] way = getWay(car, localMap);
            for(int i = 0; i<way.Length; i++)
                if(isCoorsEquals(car.getCoors(), way[i]))
                {
                    //Exception - машина находится на парковке
                    car.setNextCoors(way[i + 1]);
                    break;
                }
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
            
            printLocalMap(localMap, 0);
            Console.WriteLine("--------------------------------");
            printLocalMap(localMap, 1);
            Console.WriteLine("--------------------------------");
            printLocalMap(localMap, 2);
            Console.WriteLine("--------------------------------");
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

        public void nextStep()
        {
            int[,,] localMap = null;
            foreach (CarVehicleModel car in cars)
            {
                initLocalMap(localMap, car);
            }
        }
    }
}
