using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ParkingConstructorLib;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using ParkingSimulationForms.views.components;

namespace ParkingSimulationForms.views
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
        private static void addRow(string number, string timeComing, string timeParking, string price)
        {
            infoTable.Controls.Add(new LabelForTable(number, false));
            infoTable.Controls.Add(new LabelForTable(timeComing, false));
            infoTable.Controls.Add(new LabelForTable(timeParking, false));
            infoTable.Controls.Add(new LabelForTable(price, false));
            infoTable.RowCount++;
            TableLayoutRowStyleCollection styles = infoTable.RowStyles;
            foreach (RowStyle style in styles)
            {
                style.SizeType = SizeType.Absolute;
                style.Height = 30;
            }
        }
        public static void updateInformation(ParkingSceneVisualization<Image> psv, DateTime dateTimeModel, SettingsModel settings)
        {
            LinkedList<AbstractVehicleModel> cars = psv.getVehicles();
            infoTable.Controls.Clear();
            foreach(AbstractVehicleModel car in cars)
            {
                if (car.isOnParkingPlace)
                {
                    string id;
                    string timeComing;
                    string timeParking;
                    string price;
                    id = car.getParkingID().ToString();
                    timeComing = car.GetDateTimeStopping().ToString("HH:mm");
                    DateTime dtParking = new DateTime() + (dateTimeModel - car.GetDateTimeStopping().AddMinutes(-1));
                    timeParking = dtParking.ToString("HH:mm");
                    decimal moneyPerTick = 0m;
                    if (dateTimeModel.Hour == 0 || dateTimeModel.Hour == 1 || dateTimeModel.Hour == 2 || dateTimeModel.Hour == 3 || dateTimeModel.Hour == 4 || dateTimeModel.Hour == 5 || dateTimeModel.Hour == 21 || dateTimeModel.Hour == 22 || dateTimeModel.Hour == 23)
                        moneyPerTick = Convert.ToDecimal(settings.NightTimeRate / 60.0);
                    else
                        moneyPerTick = Convert.ToDecimal(settings.DayTimeRate / 60.0);
                    if(dtParking.Hour != 0 || dtParking.Minute != 0)
                        car.addPrice(moneyPerTick);
                    price = String.Format("{0:f2}",car.getPrice());
                    addRow(id, timeComing, timeParking, price);
                }
            }
        }
    }
}
