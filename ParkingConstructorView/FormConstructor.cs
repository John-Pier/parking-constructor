using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace ParkingConstructorView
{
    public partial class FormConstructor : Form
    {
        public FormConstructor()
        {
            InitializeComponent();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            ParkingModel parkingModel = new ParkingModel(3,4);
            ParkingSceneConstructor constructor = new ParkingSceneConstructor(parkingModel);


            VehicleModel carModel = new CarVehicleModel();
            ParkingModelElement grassElement = new GrassParkingElement();
            parkingModel.SetElement(0,0, grassElement);

            Console.WriteLine(grassElement.IsRideableElement(carModel));
        }
    }
}
