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
    public class DrawerService<T> where T : class
    {
        private Bitmap originalImage;
        private Bitmap imageNow;
        private ParkingModel<T> model;
        private Bitmap[] textures;
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

            switch (model.RoadDirection)
            {
                case RoadDirections.Bottom:
                    i_drawing_delta = 0;
                    j_drawing_delta = 0;
                    break;
                case RoadDirections.Left:
                    i_drawing_delta = 1;
                    j_drawing_delta = 0;
                    break;
                case RoadDirections.Right:
                    i_drawing_delta = 0;
                    j_drawing_delta = 0;
                    break;
                case RoadDirections.Top:
                    i_drawing_delta = 0;
                    j_drawing_delta = 1;
                    break;
            }

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
            DrawRoad(model.RoadDirection);
            imageNow = (Bitmap)originalImage.Clone();
        }

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
            }
        }

        public void Draw(LinkedList<AbstractVehicleModel> cars)
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
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Horizontal && textures[6].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[6].GetPixel(k, l));
                                    if (car.GetType() == "Car" && car.GetLastDirection() == LastDirection.Vertical && textures[7].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[7].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Horizontal && textures[8].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[8].GetPixel(k, l));
                                    if (car.GetType() == "Truck" && car.GetLastDirection() == LastDirection.Vertical && textures[9].GetPixel(k, l).A > 100)
                                        result.SetPixel(i * textureSize + k, j * textureSize + l, textures[9].GetPixel(k, l));
                                }
            }
            imageNow = result;
        }

        public Bitmap getImage()
        {
            return imageNow;
        }
    }
}
