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
            originalImage = new Bitmap(model.ColumnCount * textureSize, model.RowCount * textureSize);
            for (int i = 0; i < model.ColumnCount; i++)
                for (int j = 0; j < model.RowCount; j++)
                    for (int k = 0; k < textureSize; k++)
                        for (int l = 0; l < textureSize; l++)
                        {
                            if (model.GetElement(i, j) == null || model.GetElement(i, j).GetElementType() == ParkingModelElementType.Road)
                            {
                                originalImage.SetPixel(i * textureSize + k, j * textureSize + l, textures[10].GetPixel(k, l));
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
                                originalImage.SetPixel(i * textureSize + k, j * textureSize + l, textures[textureIndex].GetPixel(k, l));
                        }
            imageNow = (Bitmap)originalImage.Clone();
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
