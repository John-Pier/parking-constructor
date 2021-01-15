using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingConstructorLib;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;

namespace ParkingSimulationForms.views
{
    public class MainFormConstructorController
    {
        public ParkingModelElement<Image> CurrentElement;

        public ImageList ImageList;

        public TableLayoutPanel ElementsTablePanel;

        public ParkingSceneConstructor<Image> CurrentSceneConstructor;

        public void CreateAndSetTexturesBitmapArray()
        {
            var texturesBitmapArray = new Bitmap[12];
            for(var i = 0; i<12; i++)
                texturesBitmapArray[i] = new Bitmap(ImageList.Images[i]);
            ParkingSceneVisualization<Image>.SetTextures(texturesBitmapArray);
        }

        public void DrawTemplate(int horizontal, int vertical, ParkingModel<Image> model = null)
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

                var column = i % horizontal;
                var row = i / horizontal;

                if (model?.GetElement(column, row) != null)
                {
                    pictureBox.Image = model
                        .GetElement(column, row)
                        .GetElementModel();
                }

                pictureBox.Click += (sender, e) =>
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