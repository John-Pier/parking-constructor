using System;
using System.Windows.Forms;

namespace ParkingSimulationForms.views
{
    public static class MainFormSettingsController
    {
        public static double minGenerationValue;
        public static double maxGenerationValue;
        
        public static double mGenerationValue;
        public static double dGenerationValue;
        
        public static double minParkingTimeValue;
        public static double maxParkingTimeValue;
        
        public static double mParkingTimeValue;
        public static double dParkingTimeValue;
        
        public static void calcualePercent(TextBox tb, Label label)
        {
            try
            {
                var percent = Convert.ToInt32(tb.Text);
                if (percent >= 0 && percent <= 100)
                {
                    label.Text = (100 - percent) + "%";
                }
            }
            catch
            {
                label.Text = "?%";
            }
        }
    }
}
