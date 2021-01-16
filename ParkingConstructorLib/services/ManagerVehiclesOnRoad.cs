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
    //Менеджер управления автомобилями на прилегающей дороге
    public class ManagerVehiclesOnRoad<T> where T : class
    {
        private LinkedList<VehicleOnRoad> vehicles; //Автомобили на прилегающей дороге
        private double percentEntering; //Процент заезда на парковку
        private DynamicMap<T> dynamicMap; //Диинамическая карта
        private int lastPositionIndex; //Индекс, после которого машина исчезает
        private int enterPositionIndex; //Индекс, совпадающий с въездом на парковку
        private int exitPositionIndex; //Индекс, совпадающий с выездом из парковки
        private ParkingModel<T> model; //Модель парковки
        private Random random; //Объект рандома
        private SettingsModel settings; //Настройки
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
        public LinkedList<VehicleOnRoad> GetVehicleOnRoads()//Получить все автомобили на прилегающей дороге
        {
            return vehicles;
        }
        public void CreateNewVehicle(CarType carType, bool isExit)//Создать новый автомобиль на прилегающей дороге
        {
            if(!isExit)
                vehicles.AddLast(new VehicleOnRoad(carType));
            else
                vehicles.AddLast(new VehicleOnRoad(carType, exitPositionIndex));
        }
        public void Stop()//остановка симуляции - очистка списка автомобилей на прилегающей дороге
        {
            vehicles = new LinkedList<VehicleOnRoad>();
        }
        public void NextStep()//Следующий шаг - передвижение, заезд на парковку
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
        public bool isCanExit()//Может ли сейчас автомобиль выехать с парковки на прилегающую дорогу
        {
            foreach (VehicleOnRoad veh in vehicles)
                if (veh.position == exitPositionIndex)
                    return false;
            return true;
        }
    }
}
