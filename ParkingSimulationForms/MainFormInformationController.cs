using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingSimulationForms
{
    public static class MainFormInformationController
    {
        private static TableLayoutPanel infoTable;
        public static void initTable(TableLayoutPanel tlp, TableLayoutPanel infoTablePar)
        {
            LabelForTable label1 = new LabelForTable("№ парковочного места", true);
            LabelForTable label2 = new LabelForTable("время приезда", true);
            LabelForTable label3 = new LabelForTable("время стоянки", true);
            LabelForTable label4 = new LabelForTable("сумма (руб)", true);
            tlp.Controls.Add(label1, 0, 0);
            tlp.Controls.Add(label2, 1, 0);
            tlp.Controls.Add(label3, 2, 0);
            tlp.Controls.Add(label4, 3, 0);
            infoTable = infoTablePar;
        }
        public static void addRow(string number, string timeComing, string timeParking, string price)
        {
            infoTable.Controls.Add(new LabelForTable(number, false));
            infoTable.Controls.Add(new LabelForTable(timeComing, false));
            infoTable.Controls.Add(new LabelForTable(timeParking, false));
            infoTable.Controls.Add(new LabelForTable(price, false));
            infoTable.RowCount++;
        }
    }
}
