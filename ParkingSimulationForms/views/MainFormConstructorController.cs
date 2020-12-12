using System;
using System.Drawing;
using System.Windows.Forms;
using ParkingConstructorLib.models;

namespace ParkingSimulationForms.views
{
    public static class MainFormConstructorController
    {
        public static ParkingModelElement CurrentElement;

        public static ImageList ImageList;

        public static TableLayoutPanel ElementsTablePanel;

        public static void DrawTemplate(int horizontal, int vertical)
        {
            ElementsTablePanel.ColumnCount = horizontal;
            ElementsTablePanel.RowCount = vertical;

            ElementsTablePanel.Controls.Clear();
            ElementsTablePanel.BorderStyle = BorderStyle.FixedSingle;

            for (int i = 0; i < vertical; i++)
            {
                ElementsTablePanel.RowStyles[i].SizeType = SizeType.Percent;
                ElementsTablePanel.RowStyles[i].Height = 100f / vertical;
            }

            for (int i = 0; i < horizontal; i++)
            {
                ElementsTablePanel.ColumnStyles[i].SizeType = SizeType.Percent;
                ElementsTablePanel.ColumnStyles[i].Width = 100f / horizontal;
            }

            for (int i = 0; i < horizontal * vertical; i++)
            {
                PictureBox pictureBox = new PictureBox
                {
                    BackColor = Color.White,
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Margin = Padding.Empty,
                };

                pictureBox.Click += ClickEvent;
                ElementsTablePanel.Controls.Add(pictureBox);
            }
        }

        private static void ClickEvent(Object sender, EventArgs e)
        {
            Image one = ImageList.Images[0]; // Image from CurrentElement
            ((PictureBox)sender).Image = one;
        }
    }
}