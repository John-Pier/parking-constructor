using System.Windows.Forms;
using ParkingSimulationForms.views.components;

namespace ParkingSimulationForms.views
{
    public static class MainFormStatisticsController
    {
        private static TableLayoutPanel table;
        public static void initTable(TableLayoutPanel tab)
        {
            table = tab;
            table.Controls.Add(new LabelForTable("среднее количество занятых мест грузовыми ТС", true), 0, 0);
            table.Controls.Add(new LabelForTable("среднее количество занятых мест легковыми ТС", true), 0, 1);
            table.Controls.Add(new LabelForTable("текущее количество занятых мест", true), 0, 2);
            table.Controls.Add(new LabelForTable("процент текущих занятых мест", true), 0, 3);
            table.Controls.Add(new LabelForTable("заработанная сумма (руб)", true), 0, 4);
            table.Controls.Add(new LabelForTable("средний доход за день (руб)", true), 0, 5);
            table.Controls.Add(new LabelForTable("средний доход за ночь (руб)", true), 0, 6);
            table.Controls.Add(new LabelForTable("0", false), 1, 0);
            table.Controls.Add(new LabelForTable("0", false), 1, 1);
            table.Controls.Add(new LabelForTable("0", false), 1, 2);
            table.Controls.Add(new LabelForTable("0", false), 1, 3);
            table.Controls.Add(new LabelForTable("0", false), 1, 4);
            table.Controls.Add(new LabelForTable("0", false), 1, 5);
            table.Controls.Add(new LabelForTable("0", false), 1, 6);
        }
    }
}
