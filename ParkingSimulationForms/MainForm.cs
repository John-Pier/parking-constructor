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
using ParkingConstructorLib.models.vehicles;

namespace ParkingSimulationForms
{
    public partial class MainForm : Form
    {
        private ParkingSceneConstructor SceneConstructor = new ParkingSceneConstructor();
        private ParkingSceneVisualization SceneVisualization = new ParkingSceneVisualization();

        public MainForm()
        {
            InitializeComponent();
            MainFormVizualayzerController.setPictureBox(pictureBox2);
            MainFormConstructorController.DrawTemplate(pictureBox1, (int) counterHorizontal.Value,
                (int) counterVertical.Value);
            MainFormInformationController.initTable(tableLayoutPanel1, tableLayoutPanel2);
            MainFormStatisticsController.initTable(tableLayoutPanel3);
        }

        //Конструктор

        private void counterHorizontal_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate(pictureBox1, (int) counterHorizontal.Value,
                (int) counterVertical.Value);
        }

        private void counterVertical_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate(pictureBox1, (int) counterHorizontal.Value,
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
            MainFormConstructorController.CurrentElement = null; // проезжая часть
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = null; // выезд
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = null; // парвокочное место Л
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = null; // парвокочное место Г
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = null; // касса
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = null; //вьезд
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SetEnableEditSceneSize(true);
            MainFormConstructorController.CurrentElement = null;
        }

        private void SetUpConstructorAndLockSize()
        {
            if (!SceneConstructor.IsParkingModelCreate())
            {
                SceneConstructor.CreateParkingModel((int) counterHorizontal.Value, (int) counterVertical.Value);
            }
            SetEnableEditSceneSize(false);
        }

        private void SetEnableEditSceneSize(bool enable)
        {
            counterHorizontal.Enabled = enable;
            counterVertical.Enabled = enable;
        }
    }
}