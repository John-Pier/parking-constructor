using System;
using System.Windows.Forms;
using ParkingConstructorLib.logic;

namespace ParkingSimulationForms.views
{
    public static class MainFormSettingsController
    {
        public static SettingsModel SettingsModel = new SettingsModel();

        public static void calcualePercent(TextBox tb, Label label)
        {
            string newText = tb.Text;
            bool isGood = true;
            int percent = -1;
            try
            {
                percent = Convert.ToInt32(newText);
            }
            catch
            {
                isGood = false;
            }
            if (percent < 0 || percent > 100)
                isGood = false;
            if (isGood)
                label.Text = (100 - percent).ToString() + "%";
            else
                label.Text = "?%";
        }

        // TODO: Refactoring - delete from this
        public static void LockRBs(RadioButton rb1, RadioButton rb2, RadioButton rb3, TextBox tb1, TextBox tb2, TextBox tb3, TextBox tb4, TextBox tb5, TextBox tb6, bool isLock)
        {
            rb1.Enabled = isLock;
            rb2.Enabled = isLock;
            rb3.Enabled = isLock;
            tb1.Enabled = isLock;
            tb2.Enabled = isLock;
            tb3.Enabled = isLock;
            tb4.Enabled = isLock;
            tb5.Enabled = isLock;
            tb6.Enabled = !isLock;
        }
    }
}
