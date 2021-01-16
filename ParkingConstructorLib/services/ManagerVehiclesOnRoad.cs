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
    public class ManagerVehiclesOnRoad<T> where T : class
    {
        private LinkedList<VehicleOnRoad> vehicles;
        private double percentEntering;
        private DynamicMap<T> dynamicMap;
        private int lastPositionIndex;
        private int enterPositionIndex;
        private int exitPositionIndex;
        private ParkingModel<T> model;
        private Random random;
        private SettingsModel settings;
        public ManagerVehiclesOnRoad(SettingsModel settings, ParkingModel<T> parkingModel, DynamicMap<T> dynamicMap, int lastPositionIndex)
        {
            vehicles = new LinkedList<VehicleOnRoad>();
            this.settings = settings;
            this.percentEntering = settings.EnteringProbability;
            this.dynamicMap = dynamicMap;
            this.lastPositionIndex = lastPositionIndex;
            this.model = parkingModel;
            random = new Random();
            if (model.RoadDirection == RoadDirections.Bottom || model.RoadDirection == RoadDirections.Top)
            {
                enterPositionIndex = dynamicMap.SpawnCol + 1;
                exitPositionIndex = dynamicMap.exitCoors.ColumnIndex - 1;
            }
            else
            {
                enterPositionIndex = dynamicMap.SpawnRow + 1;
                exitPositionIndex = dynamicMap.exitCoors.RowIndex - 1;
            }
        }
        public LinkedList<VehicleOnRoad> GetVehicleOnRoads()
        {
            return vehicles;
        }
        public void CreateNewVehicle(CarType carType, bool isExit)
        {
            if(!isExit)
                vehicles.AddLast(new VehicleOnRoad(carType));
            else
                vehicles.AddLast(new VehicleOnRoad(carType, exitPositionIndex));
        }
        public void Stop()
        {
            vehicles = new LinkedList<VehicleOnRoad>();
        }
        public void NextStep()
        {
            LinkedList<VehicleOnRoad> remVehs = new LinkedList<VehicleOnRoad>();
            foreach(VehicleOnRoad vehicle in vehicles)
            {
                vehicle.position++;
                if (vehicle.position > lastPositionIndex+1)
                {
                    remVehs.AddLast(vehicle);
                    continue;
                }
                if (vehicle.position == enterPositionIndex && random.NextDouble() < percentEntering && dynamicMap.IsCanAddVehicle(vehicle.carType) && !vehicle.isStayOnParkingInThisTime)
                {
                    var parkingTimeInMinutes = (int)(settings.ParkingTimeDistribution.GetRandNumber() * 60);
                    AbstractVehicleModel vehicleModel;
                    if(vehicle.carType == CarType.Car)
                    {
                        vehicleModel = new CarVehicleModel(dynamicMap.SpawnRow, dynamicMap.SpawnCol);
                        dynamicMap.AddCar(vehicleModel);
                    }
                    else
                    {
                        vehicleModel = new TruckVehicleModel(dynamicMap.SpawnRow, dynamicMap.SpawnCol);
                        dynamicMap.AddTruck(vehicleModel);
                    }
                    if (model.RoadDirection == RoadDirections.Bottom || model.RoadDirection == RoadDirections.Top)
                        vehicleModel.SetLastDirection(LastDirection.Vertical);
                    vehicleModel.SetSecondsOnParking(parkingTimeInMinutes);
                    remVehs.AddLast(vehicle);
                }
            }
            foreach(VehicleOnRoad remVeh in remVehs)
                vehicles.Remove(remVeh);
        }
        public bool isCanExit()
        {
            foreach (VehicleOnRoad veh in vehicles)
                if (veh.position == exitPositionIndex)
                    return false;
            return true;
        }
    }
}
