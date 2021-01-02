using System.Drawing;
using System.Windows.Forms;
using ParkingConstructorLib;

namespace ParkingSimulationForms.views
{
    public static class MainFormVizualayzerController
    {
        public static ParkingSceneVisualization<Image> CurrentSceneVisualization;

        private static PictureBox pb;
        private static double accelerate;

        public static void changePercentValue(HScrollBar sb, Label label, Timer timer1)
        {
            accelerate = sb.Value;
            accelerate /= 100;
            label.Text = accelerate.ToString();
            timer1.Interval = (int)(1000 / accelerate);
        }
        
        public static void setPictureBox(PictureBox pbx)
        {
            pb = pbx;
        }
    }
}
