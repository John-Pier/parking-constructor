using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingSimulationForms
{
    public static class MainFormConstructorController
    {
        public static void DrawTemplate(PictureBox pb, int horizontal, int vertical)
        {
            Bitmap b = new Bitmap(40 * horizontal, 40 * vertical);

            for (int i = 0; i < 40 * horizontal; i++)
                for (int j = 0; j < 40 * vertical; j++)
                    b.SetPixel(i, j, Color.White);

            for (int i = 0; i < 40*horizontal; i += 40)
                for (int j = 0; j < 40 * vertical; j++)
                    b.SetPixel(i, j, Color.Black);

            for (int i = 0; i < 40 * vertical; i += 40)
                for (int j = 0; j < 40 * horizontal; j++)
                    b.SetPixel(j, i, Color.Black);

            for (int i = 0; i < 40 * horizontal; i++)
                b.SetPixel(i, 40 * vertical - 1, Color.Black);

            for (int i = 0; i < 40 * vertical; i++)
                b.SetPixel(40 * horizontal - 1, i, Color.Black);

            pb.Image = b;
            pb.Refresh();
            MainFormVizualayzerController.setPicture(b);
        }
    }
}
