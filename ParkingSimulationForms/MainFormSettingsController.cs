﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingSimulationForms
{
    public static class MainFormSettingsController
    {
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
    }
}
