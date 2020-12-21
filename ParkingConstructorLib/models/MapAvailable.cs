using ParkingConstructorLib.logic;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.models
{
    public class MapAvailable<T> where T: class
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
            foreach(CarVehicleModel car in cars)
                map[car.getColumnIndex(), car.getRowIndex()] = false;
        }
        public void printMap()
        {
            for(int i = 0; i<model.RowCount; i++)
            {
                for (int j = 0; j < model.ColumnCount; j++)
                    Console.Write(map[j, i] + " ");
                Console.WriteLine();
            }
        }
        public void addCar(CarVehicleModel car)
        {
            cars.AddLast(car);
            reloadMap();
        }
    }
}
