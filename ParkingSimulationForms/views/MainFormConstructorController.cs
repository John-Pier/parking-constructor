using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingConstructorLib;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;

namespace ParkingSimulationForms.views
{
    public static class MainFormConstructorController
    {
        public static ParkingModelElement<Image> CurrentElement;

        public static ImageList ImageList;

        public static TableLayoutPanel ElementsTablePanel;

        public static ParkingSceneConstructor<Image> CurrentSceneConstructor;

        public static void createAndSetTexturesBitmapArray(ImageList texturesImageList)
        {
            Bitmap[] texturesBitmapArray = new Bitmap[11];
            for(int i = 0; i<11; i++)
                texturesBitmapArray[i] = new Bitmap(texturesImageList.Images[i]);
            ParkingSceneVisualization<Image>.setTextures(texturesBitmapArray);
        }

        public static void DrawTemplate(int horizontal, int vertical, ParkingModel<Image> model = null)
        {
            ElementsTablePanel.ColumnCount = horizontal;
            ElementsTablePanel.RowCount = vertical;

            ElementsTablePanel.Controls.Clear();
            ElementsTablePanel.BorderStyle = BorderStyle.FixedSingle;

            for (var i = 0; i < vertical; i++)
            {
                ElementsTablePanel.RowStyles[i].SizeType = SizeType.Percent;
                ElementsTablePanel.RowStyles[i].Height = 100f / vertical;
            }

            for (var i = 0; i < horizontal; i++)
            {
                ElementsTablePanel.ColumnStyles[i].SizeType = SizeType.Percent;
                ElementsTablePanel.ColumnStyles[i].Width = 100f / horizontal;
            }

            for (var i = 0; i < horizontal * vertical; i++)
            {
                var pictureBox = new PictureBox
                {
                    BackColor = Color.DarkGray,
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = Padding.Empty,
                };

                int column = i % horizontal;
                int row = i / horizontal;

                if (model?.GetElement(column, row) != null)
                {
                    pictureBox.Image = model
                        .GetElement(column, row)
                        .GetElementModel();
                }

                pictureBox.Click += (object sender, EventArgs e) =>
                {
                    var picture = ((PictureBox) sender);
                    if (picture.Image != null)
                    {
                        picture.Image = null;
                        CurrentSceneConstructor?.SetObjectToModel(column, row, null);
                    }
                    else
                    {
                        picture.Image = CurrentElement.GetElementModel();
                        CurrentSceneConstructor?.SetObjectToModel(column, row, CurrentElement);
                    }
                };

                ElementsTablePanel.Controls.Add(pictureBox);
            }
        }
    }
}