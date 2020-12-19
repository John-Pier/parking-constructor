using System.Drawing;
using System.Windows.Forms;

namespace ParkingSimulationForms.views
{
    public static class MainFormVizualayzerController
    {
        private static PictureBox pb;
        private static double accelerate;
        public static void changePercentValue(HScrollBar sb, Label label)
        {
            accelerate = sb.Value;
            accelerate /= 100;
            label.Text = accelerate.ToString();
        }
        public static double getAccelerate()
        {
            return accelerate;
        }
        public static void setPictureBox(PictureBox pbx)
        {
            pb = pbx;
        }
        public static void setPicture(Bitmap bm)
        {
            pb.Image = bm;
            pb.Refresh();
        }
    }
}
