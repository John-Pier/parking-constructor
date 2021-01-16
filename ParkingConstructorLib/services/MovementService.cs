using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.services
{
    public class MovementService<T> where T : class
    {
        private ParkingModel<T> model;
        private LinkedList<AbstractParkingPlace> carParkingPlaces;
        private LinkedList<AbstractParkingPlace> truckParkingPlaces;
        private LinkedList<AbstractVehicleModel> cars;
        private DynamicMap<T> dynamicMap;
        private ManagerVehiclesOnRoad<T> roadManager;

        public MovementService(ParkingModel<T> model, DynamicMap<T> dynamicMap)
        {
            this.model = model;
            this.carParkingPlaces = dynamicMap.getCarsParkingPlaces();
            this.truckParkingPlaces = dynamicMap.getTruckParkingPlaces();
            this.cars = dynamicMap.getVehicles();
            this.dynamicMap = dynamicMap;
        }

        public void setRoadManager(ManagerVehiclesOnRoad<T> roadManager)
        {
            this.roadManager = roadManager;
        }

        public Coors[] foundWay(int[,,] localMap, AbstractVehicleModel @abstract)
        {
            RunDijkstraAlgorithm(localMap, @abstract);
            return GetWay(@abstract, localMap);
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

        private Comparison<Coors> GetCoorsComparison(int[,,] localMap)
        {
            return (a, b) => localMap[a.ColumnIndex, a.RowIndex, 2] - localMap[b.ColumnIndex, b.RowIndex, 2];
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
                foreach (var coorsTemp in neighboreCoors)
                {
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

        public AbstractVehicleModel[] nextSystemStep(int[,,] localMap, AbstractVehicleModel vehicleModel, Coors[] way, DateTime modelDateTime, StatisticModel stats)
        {
            LinkedList<AbstractVehicleModel> removedCars = new LinkedList<AbstractVehicleModel>();
            bool stopEnding = false;
            for (int i = 0; i < way.Length; i++)
                if (vehicleModel.GetCoors().Equals(way[i]))
                {
                    //Если машина на парковке
                    if (vehicleModel.GetCoors().Equals(vehicleModel.GetTarget()) && vehicleModel.GetTargetType() == TargetType.Parking)
                    {
                        if (!vehicleModel.checkedOnStatisticStopOnPlace)
                        {
                            vehicleModel.checkedOnStatisticStopOnPlace = true;
                            stats.TakeParkingPlace();
                        }
                        if ((modelDateTime - vehicleModel.GetDateTimeStopping()).TotalMinutes >= vehicleModel.GetSecondsOnParking())
                        {
                            stopEnding = true;
                            vehicleModel.SetTarget(dynamicMap.cashierCoors);
                            vehicleModel.SetTargetType(TargetType.Cashier);
                            vehicleModel.isOnParkingPlace = false;
                            if (vehicleModel.GetType() == "Car")
                            {
                                foreach (var cpp in carParkingPlaces.Where(cpp => cpp.coors.Equals(vehicleModel.GetCoors())))
                                {
                                    cpp.isBusy = false;
                                    break;
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
                    if (dynamicMap.cashierCoors.Equals(vehicleModel.GetCoors()))
                    {
                        vehicleModel.SetTarget(dynamicMap.exitCoors);
                        vehicleModel.SetTargetType(TargetType.Exit);
                        stats.AddToFinalScope(Convert.ToDouble(vehicleModel.getPrice()), modelDateTime);
                    }
                    //Машина на выезде
                    if (dynamicMap.exitCoors.Equals(vehicleModel.GetCoors()) && roadManager.isCanExit())
                    {
                        removedCars.AddLast(vehicleModel);
                        stats.FreeParkingPlace();
                        CarType carType;
                        carType = vehicleModel.GetType().Equals("Car") ? CarType.Car : CarType.Truck;
                        roadManager.CreateNewVehicle(carType, true);
                    }
                    if (vehicleModel.GetCoors().Equals(way[i]) && i != way.Length - 1)
                    {
                        vehicleModel.SetNextCoors(way[i + 1]);
                    }
                    else if (vehicleModel.GetCoors().Equals(way[i])
                             && i == way.Length - 1
                             && (stopEnding || vehicleModel.GetCoors().Equals(dynamicMap.cashierCoors)))
                    {
                        int[,,] localMapTemp = dynamicMap.CreateAndInitLocalMap();
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
            dynamicMap.reloadMap();
            return removedCars.ToArray();
        }
    }
}
