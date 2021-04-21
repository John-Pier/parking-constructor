using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingConstructorLib.services
{
    //Класс отрисовки
    public class DrawerService<T> where T : class
    {
        private Bitmap originalImage;//Оригинальное изображение без автомобилей
        private Bitmap imageNow;//Изображение текущего кадра с автомобилями
        private ParkingModel<T> model;//Модель парковки
        private Bitmap[] textures;//Текстуры для отрисовки
        public DrawerService(ParkingModel<T> model, Bitmap[] textures)
        {
            this.model = model;
            this.textures = textures;
            const int textureSize = 10;
            originalImage = null;
            if(model.RoadDirection == RoadDirections.Bottom || model.RoadDirection == RoadDirections.Top)
                originalImage = new Bitmap(model.ColumnCount * textureSize, (model.RowCount+1) * textureSize);
            else
                originalImage = new Bitmap((model.ColumnCount + 1) * textureSize, model.RowCount * textureSize);

            int i_drawing_delta = 0;
            int j_drawing_delta = 0;

            //Сдвиг из-за расположения доороги
            switch (model.RoadDirection)
            {
                case RoadDirections.Bottom:
                case RoadDirections.Right:
                    i_drawing_delta = 0;
                    j_drawing_delta = 0;
                    break;
                case RoadDirections.Left:
                    i_drawing_delta = 1;
                    j_drawing_delta = 0;
                    break;
                case RoadDirections.Top:
                    i_drawing_delta = 0;
                    j_drawing_delta = 1;
                    break;
            }

            //Отрисовка всех блоков
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    for (int k = 0; k < textureSize; k++)
                        for (int l = 0; l < textureSize; l++)
                        {
                            if (model.GetElement(i, j) == null || model.GetElement(i, j).GetElementType() == ParkingModelElementType.Road)
                            {
                                originalImage.SetPixel((i+i_drawing_delta) * textureSize + k, (j+j_drawing_delta) * textureSize + l, textures[10].GetPixel(k, l));
                                continue;
                            }
                            int textureIndex = -1;
                            switch (model.GetElement(i, j).GetElementType())
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
                            if (textureIndex != -1)
                                originalImage.SetPixel((i+i_drawing_delta) * textureSize + k, (j+j_drawing_delta) * textureSize + l, textures[textureIndex].GetPixel(k, l));
                        }
            DrawRoad(model.RoadDirection);//отрисовка прилегающей дороги
            imageNow = (Bitmap)originalImage.Clone();
        }

        //Отрисовка прилегающей дороги
        private void DrawRoad(RoadDirections dir)
        {
            int textureSize = 10;
            switch (dir)
            {
                case RoadDirections.Bottom:
                    for(int i = 0; i<model.ColumnCount*textureSize; i++)
                        for(int j = 0; j<textureSize; j++)
                            originalImage.SetPixel(i, (model.RowCount * textureSize) + j, textures[11].GetPixel(i%textureSize, j));
                    break;
                case RoadDirections.Top:
                    for (int i = 0; i < model.ColumnCount * textureSize; i++)
                        for (int j = 0; j < textureSize; j++)
                            originalImage.SetPixel(i, j, textures[11].GetPixel(i % textureSize, j));
                    break;
                case RoadDirections.Left:
                    for (int j = 0; j < model.RowCount * textureSize; j++)
                        for (int i = 0; i < textureSize; i++)
                            originalImage.SetPixel(i, j, textures[11].GetPixel(j % textureSize, i));
                    break;
                case RoadDirections.Right:
                    for (int j = 0; j < model.RowCount * textureSize; j++)
                        for (int i = 0; i < textureSize; i++)
                            originalImage.SetPixel((model.ColumnCount * textureSize) + i, j, textures[11].GetPixel(j % textureSize, i));
                    break;
            }
        }

        //отрисовка автомобилей (каждый кадр)
        public void Draw(LinkedList<AbstractVehicleModel> cars, ManagerVehiclesOnRoad<T> roadManager)
        {
            const int textureSize = 10;
            Bitmap result = (Bitmap)originalImage.Clone();
            foreach (AbstractVehicleModel car in cars)
            {
                for (int i = 0; i < model.ColumnCount; i++)
                    for (int j = 0; j < model.RowCount; j++)
                        if (car.GetCoors().Equals(new Coors(i, j)))
                            for (int k = 0; k < textureSize; k++)
                                for (int l = 0; l < textureSize; l++)
                                {
                                    int i_new = 0;
                                    int j_new = 0;
                                    switch (model.RoadDirection)
                                    {
                                        case RoadDirections.Bottom:
                                        case RoadDirections.Right:
                                            i_new = i;
                                            j_new = j;
                                            break;
                                        case RoadDirections.Top:
                                            i_new = i;
                                            j_new = j + 1;
                                            break;
                                        case RoadDirections.Left:
                                            i_new = i + 1;
                                            j_new = j;
                                            break;
                                    }
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Horizontal && textures[6].GetPixel(k, l).A > 100)
                                        result.SetPixel(i_new * textureSize + k, j_new * textureSize + l, textures[6].GetPixel(k, l));
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Vertical && textures[7].GetPixel(k, l).A > 100)
                                        result.SetPixel(i_new * textureSize + k, j_new * textureSize + l, textures[7].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Horizontal && textures[8].GetPixel(k, l).A > 100)
                                        result.SetPixel(i_new * textureSize + k, j_new * textureSize + l, textures[8].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Vertical && textures[9].GetPixel(k, l).A > 100)
                                        result.SetPixel(i_new * textureSize + k, j_new * textureSize + l, textures[9].GetPixel(k, l));
                                }
            }
            //Отрисовка машин на прилегающей дороге
            if(roadManager != null)
            {
                Bitmap textureCar = null;
                Bitmap textureTruck = null;
                if (model.RoadDirection == RoadDirections.Bottom || model.RoadDirection == RoadDirections.Top)
                {
                    textureCar = textures[6];
                    textureTruck = textures[8];
                }
                else
                {
                    textureCar = textures[7];
                    textureTruck = textures[9];
                }
                LinkedList<VehicleOnRoad> vehiclesOnRoad = roadManager.GetVehicleOnRoads();
                
                foreach (VehicleOnRoad veh in vehiclesOnRoad)
                {
                    try
                    {
                        if (model.RoadDirection == RoadDirections.Top)
                        {
                            if (veh.carType == CarType.Car)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureCar.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel((veh.position * textureSize) + i, j, textureCar.GetPixel(i, j));
                                            else
                                                result.SetPixel((veh.position * textureSize) + j, i, textureCar.GetPixel(i, j));


                            if (veh.carType == CarType.Truck)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureTruck.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel((veh.position * textureSize) + i, j, textureTruck.GetPixel(i, j));
                                            else
                                                result.SetPixel((veh.position * textureSize) + j, i, textureTruck.GetPixel(i, j));

                        }
                        if (model.RoadDirection == RoadDirections.Bottom)
                        {
                            if (veh.carType == CarType.Car)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureCar.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel((veh.position * textureSize) + i, model.RowCount * textureSize + j, textureCar.GetPixel(i, j));
                                            else
                                                result.SetPixel((veh.position * textureSize) + j, model.RowCount * textureSize + i, textureCar.GetPixel(i, j));
                            if (veh.carType == CarType.Truck)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureTruck.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel((veh.position * textureSize) + i, model.RowCount * textureSize + j, textureTruck.GetPixel(i, j));
                                            else
                                                result.SetPixel((veh.position * textureSize) + j, model.RowCount * textureSize + i, textureTruck.GetPixel(i, j));
                        }
                        if (model.RoadDirection == RoadDirections.Left)
                        {
                            if (veh.carType == CarType.Car)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureCar.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel(i, veh.position * textureSize + j, textureCar.GetPixel(i, j));
                                            else
                                                result.SetPixel(j, veh.position * textureSize + i, textureCar.GetPixel(i, j));
                            if (veh.carType == CarType.Truck)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureTruck.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel(i, veh.position * textureSize + j, textureTruck.GetPixel(i, j));
                                            else
                                                result.SetPixel(j, veh.position * textureSize + i, textureTruck.GetPixel(i, j));
                        }
                        if (model.RoadDirection == RoadDirections.Right)
                        {
                            if (veh.carType == CarType.Car)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureCar.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel(model.ColumnCount * textureSize + i, veh.position * textureSize + j, textureCar.GetPixel(i, j));
                                            else
                                                result.SetPixel(model.ColumnCount * textureSize + j, veh.position * textureSize + i, textureCar.GetPixel(i, j));
                            if (veh.carType == CarType.Truck)
                                for (int i = 0; i < textureSize; i++)
                                    for (int j = 0; j < textureSize; j++)
                                        if (textureTruck.GetPixel(i, j).A > 100)
                                            if (!veh.isExited)
                                                result.SetPixel(model.ColumnCount * textureSize + i, veh.position * textureSize + j, textureTruck.GetPixel(i, j));
                                            else
                                                result.SetPixel(model.ColumnCount * textureSize + j, veh.position * textureSize + i, textureTruck.GetPixel(i, j));
                        }
                        if (veh.isExited)
                            veh.isExited = false;
                    }
                    catch
                    {

                    }
                }
            }
            imageNow = result;
        }

        public Bitmap getImage()//Получить текущий кадр
        {
            return imageNow;
        }

        public void Stop()//Остановка симуляции - удаление автомобилей с изображения
        {
            imageNow = originalImage;
        }
    }
}
