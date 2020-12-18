using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingConstructorLib;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.parking_elements;
using ParkingConstructorLib.models.vehicles;
using ParkingSimulationForms.views;

namespace ParkingSimulationForms
{
    public partial class MainForm : Form
    {
        private readonly ParkingSceneConstructor sceneConstructor = new ParkingSceneConstructor();
        private readonly ParkingSceneVisualization sceneVisualization = new ParkingSceneVisualization();

        public MainForm()
        {
            InitializeComponent();
            MainFormVizualayzerController.setPictureBox(pictureBox2);
            MainFormConstructorController.ImageList = imageList1;
            MainFormConstructorController.ElementsTablePanel = elementsTablePanel;
            MainFormConstructorController.DrawTemplate((int) counterHorizontal.Value,
                (int) counterVertical.Value);
            MainFormInformationController.initTable(tableLayoutPanel1, tableLayoutPanel2);
            MainFormStatisticsController.initTable(tableLayoutPanel3);

            elementsTablePanel.Enabled = false;
        }

        //Конструктор

        private void counterHorizontal_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate((int) counterHorizontal.Value,
                (int) counterVertical.Value);
        }

        private void counterVertical_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate((int) counterHorizontal.Value,
                (int) counterVertical.Value);
        }

        //Настройки

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.calcualePercent(textBox5, label14);
        }

        //Визуализатор

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            MainFormVizualayzerController.changePercentValue(hScrollBar1, label18);
        }

        private void test()
        {
            ParkingModel parkingModel = new ParkingModel(3, 4);
            ParkingSceneConstructor constructor = new ParkingSceneConstructor(parkingModel);


            VehicleModel carModel = new CarVehicleModel();
            ParkingModelElement grassElement = new GrassParkingElement();
            parkingModel.SetElement(0, 0, grassElement);

            Console.WriteLine(grassElement.IsRideableElement(carModel));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new RoadParkingElement(); // проезжая часть
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new ExitParkingElement(); // выезд
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new ParkingSpaceElement(); // парвокочное место Л
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new TruckParkingSpaceElement(); // парвокочное место Г
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new CashierParkingElement(); // касса
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new EntryParkingElement(); //вьезд
        }

        private void button8_Click(object sender, EventArgs e) // Clear
        {
            SetEnableEditSceneSize(true);
            MainFormConstructorController.DrawTemplate((int)counterHorizontal.Value, (int)counterVertical.Value);
            MainFormConstructorController.CurrentElement = null;
        }

        private void SetUpConstructorAndLockSize()
        {
            if (!sceneConstructor.IsParkingModelCreate())
            {
                sceneConstructor.CreateParkingModel((int) counterHorizontal.Value, (int) counterVertical.Value);
            }
            SetEnableEditSceneSize(false);
        }

        private void SetEnableEditSceneSize(bool enable)
        {
            elementsTablePanel.Enabled = !enable;
            counterHorizontal.Enabled = enable;
            counterVertical.Enabled = enable;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.LockRBs(radioButton3, radioButton4, radioButton5, textBoxWithPlaceholder1, textBoxWithPlaceholder2, textBoxWithPlaceholder3, textBoxWithPlaceholder4, textBoxWithPlaceholder5, textBox1, !((RadioButton)sender).Checked);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.LockRBs(radioButton6, radioButton7, radioButton8, textBoxWithPlaceholder6, textBoxWithPlaceholder7, textBoxWithPlaceholder8, textBoxWithPlaceholder9, textBoxWithPlaceholder10, textBoxWithPlaceholder11, !((RadioButton)sender).Checked);
        }
    }
}