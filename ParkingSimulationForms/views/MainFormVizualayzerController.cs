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

        //Изменить ускорение системы
        public static double changePercentValue(HScrollBar sb)
        {
            accelerate = sb.Value;
            accelerate /= 100;
            return accelerate;
        }
        
        public static void setPictureBox(PictureBox pbx)
        {
            pb = pbx;
        }
    }
}
