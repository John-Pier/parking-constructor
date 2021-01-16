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
        private static DoubleBufferedTable infoTable;
        private static bool isInit = false;
        public static void initTable(TableLayoutPanel tlp, DoubleBufferedTable infoTablePar)
        {
            if (!isInit)
            {
                LabelForTable label1 = new LabelForTable("№ парковочного места", true, false);
                LabelForTable label2 = new LabelForTable("время приезда", true, false);
                LabelForTable label3 = new LabelForTable("время стоянки", true, false);
                LabelForTable label4 = new LabelForTable("сумма (руб)", true, false);
                tlp.Controls.Add(label1, 0, 0);
                tlp.Controls.Add(label2, 1, 0);
                tlp.Controls.Add(label3, 2, 0);
                tlp.Controls.Add(label4, 3, 0);
                label1.enableBorder();
                label2.enableBorder();
                label3.enableBorder();
                label4.enableBorder();
                infoTable = infoTablePar;
                isInit = true;
            }
        }
        private static void addRow(string number, string timeComing, string timeParking, string price)
        {
            LabelForTable[] labels = new LabelForTable[4];
            labels[0] = new LabelForTable(number, false, true);
            labels[1] = new LabelForTable(timeComing, false, true);
            labels[2] = new LabelForTable(timeParking, false, true);
            labels[3] = new LabelForTable(price, false, true);
            infoTable.Controls.AddRange(labels);
            infoTable.RowCount++;
            TableLayoutRowStyleCollection styles = infoTable.RowStyles;
            foreach (RowStyle style in styles)
            {
                style.SizeType = SizeType.Absolute;
                style.Height = 30;
            }
            labels[0].enableBorder();
            labels[1].enableBorder();
            labels[2].enableBorder();
            labels[3].enableBorder();
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
