using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkingConstructorLib;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.models.vehicles;
using ParkingConstructorLib.utils.distributions;
using ParkingSimulationForms.views;
using ParkingSimulationForms.views.services;

namespace ParkingSimulationForms
{
    public partial class MainForm : Form
    {
        private readonly ParkingSceneConstructor<Image> sceneConstructor = new ParkingSceneConstructor<Image>();
        private readonly ParkingSceneVisualization<Image> sceneVisualization = new ParkingSceneVisualization<Image>();
        private readonly FormFilesService formFilesService = new FormFilesService();
        private DateTime dateTimeModel;

        public SettingsModel SettingsModel = new SettingsModel();

        private UniformDistribution generationStreamRandom;

        public MainForm()
        {
            InitializeComponent();

            MainFormVizualayzerController.setPictureBox(pictureBox2);
            MainFormVizualayzerController.CurrentSceneVisualization = sceneVisualization;

            MainFormConstructorController.ImageList = elementsImageList;
            MainFormConstructorController.ElementsTablePanel = elementsTablePanel;
            MainFormConstructorController.CurrentSceneConstructor = sceneConstructor;
            MainFormConstructorController.DrawTemplate((int) counterHorizontal.Value, (int) counterVertical.Value);

            MainFormInformationController.initTable(tableLayoutPanel1, tableLayoutPanel2);
            MainFormStatisticsController.initTable(tableLayoutPanel3);

            MainFormConstructorController.createAndSetTexturesBitmapArray(texturesImageList);
            InitSettingsForm();

            domainUpDown1.SelectedIndex = 0;

            SetUpRoadImages(RoadDirections.Top);
            InitRoadImages();

            elementsTablePanel.Enabled = false;
            saveButton.Enabled = false;
        }

        private void InitRoadImages()
        {
            pictureRoadBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureRoadBox1.Image = elementsImageList.Images[8];
            pictureRoadBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            pictureRoadBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureRoadBox2.Image = elementsImageList.Images[8];

            pictureRoadBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureRoadBox3.Image = elementsImageList.Images[8];
            pictureRoadBox3.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            pictureRoadBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureRoadBox4.Image = elementsImageList.Images[8];
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

        //Визуализатор
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            MainFormVizualayzerController.changePercentValue(hScrollBar1, label18, modelGeneralTimer);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement =
                new GrassParkingElement(elementsImageList.Images[4]); // газон
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new ExitParkingElement(elementsImageList.Images[3]); // выезд
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement =
                new ParkingSpaceElement(elementsImageList.Images[5]); // парвокочное место Л
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement =
                new TruckParkingSpaceElement(elementsImageList.Images[7]); // парвокочное место Г
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement =
                new CashierParkingElement(elementsImageList.Images[1]); // касса
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new EntryParkingElement(elementsImageList.Images[2]); //вьезд
        }

        private void button8_Click(object sender, EventArgs e) // Clear
        {
            SetEnableEditSceneSize(true);
            MainFormConstructorController.CurrentElement = null;
            MainFormConstructorController.DrawTemplate((int) counterHorizontal.Value, (int) counterVertical.Value);
            sceneConstructor.ClearModel();
        }

        private void SetUpConstructorAndLockSize()
        {
            if (!sceneConstructor.IsParkingModelCreate())
            {
                sceneConstructor.CreateParkingModel((int) counterHorizontal.Value, (int) counterVertical.Value,
                    (RoadDirections) domainUpDown1.SelectedIndex);
            }

            SetEnableEditSceneSize(false);
        }

        private void SetEnableEditSceneSize(bool enable)
        {
            elementsTablePanel.Enabled = !enable;
            counterHorizontal.Enabled = enable;
            counterVertical.Enabled = enable;
            saveButton.Enabled = !enable;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LockElements(!((RadioButton) sender).Checked);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            var isLock = !((RadioButton) sender).Checked;
            radioButton6.Enabled = isLock;
            radioButton7.Enabled = isLock;
            radioButton8.Enabled = isLock;
            textBoxWithPlaceholder6.Enabled = isLock;
            textBoxWithPlaceholder7.Enabled = isLock;
            textBoxWithPlaceholder8.Enabled = isLock;
            textBoxWithPlaceholder9.Enabled = isLock;
            textBoxWithPlaceholder10.Enabled = isLock;
            textBoxWithPlaceholder11.Enabled = !isLock;
        }

        private void OnLoadClick(object sender, EventArgs e)
        {
            var parkingModel = formFilesService.OpenDialogAndLoadModel<Image>();

            if (parkingModel == null) return;

            sceneConstructor.SetParkingModel(parkingModel);
            counterHorizontal.Value = sceneConstructor.ParkingModel.ColumnCount;
            counterVertical.Value = sceneConstructor.ParkingModel.RowCount;

            MainFormConstructorController.DrawTemplate(
                (int) counterHorizontal.Value,
                (int) counterVertical.Value,
                parkingModel
            );

            SetEnableEditSceneSize(false);
        }

        private void OnSaveClick(object sender, EventArgs e)
        {
            formFilesService.OpenDialogAndSaveModel(sceneConstructor.ParkingModel);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                if (sceneConstructor.IsParkingModelCreate() && sceneConstructor.ParkingModel.IsParkingModelCorrect())
                {
                    InitModelTime();
                    
                    sceneVisualization.SetParkingModel(sceneConstructor.ParkingModel);
                    sceneVisualization.nextStep(Convert.ToDouble(label18.Text));
                    
                    DrawImage();
                }
                else
                {
                    var result = MessageBox.Show(
                        "Вы не можете запустить визуализатор, потому что текущая модель парковки не соответствует необходимым требованиям.\nХотите вернуться ?",
                        "Модель не корректна",
                        MessageBoxButtons.YesNo
                    );
                    if (result == DialogResult.Yes)
                    {
                        tabControl1.SelectedIndex = 0;
                    }

                    return;
                }

                if (SettingsModel.IsModelValid())
                {
                    sceneVisualization.SetSettingsModel(SettingsModel);
                }
                else
                {
                    var result = MessageBox.Show(
                        "Вы не можете запустить визуализатор, потому что текущие настройки не валидны или не заданы.\nХотите вернуться ?",
                        "Настройки не валидны",
                        MessageBoxButtons.YesNo
                    );
                    if (result == DialogResult.Yes)
                    {
                        tabControl1.SelectedIndex = 1;
                    }

                    return;
                }
            }
        }

        #region Timers logic

        public void StartGeneralTimerClick(object sender, EventArgs e)
        {
            //Set timers interval in ms
            generationStreamTimer.Interval = (int) (SettingsModel.GenerationStreamDistribution.GetRandNumber() * 1000);
            generationStreamRandom = new UniformDistribution(0d, 100d);

            modelGeneralTimer.Start();
            generationStreamTimer.Start();
        }

        private void PauseGeneralTimerClick(object sender, EventArgs e)
        {
            modelGeneralTimer.Stop();
            generationStreamTimer.Stop();
        }

        private void StopGeneralTimerClick(object sender, EventArgs e)
        {
            modelGeneralTimer.Stop();
            generationStreamTimer.Stop();
            sceneVisualization.SetParkingModel(sceneConstructor.ParkingModel);
            sceneVisualization.nextStep(Convert.ToDouble(label18.Text));

            DrawImage();
        }

        private void modelGeneralTimer_Tick(object sender, EventArgs e)
        {
            sceneVisualization.nextStep(Convert.ToDouble(label18.Text));
            
            DrawImage();
            
            SetModelTime();
        }

        private void generationStreamTimer_Tick(object sender, EventArgs e)
        {
            if (generationStreamRandom.GetRandNumber() > SettingsModel.PercentOfTrack)
            {
                if (sceneVisualization.isCanAddThisCar(CarVehicleModel.CarType.Car))
                {
                    sceneVisualization.createCar((int)(SettingsModel.ParkingTimeDistribution.GetRandNumber()));
                } 
            }
            else
            {
                if (sceneVisualization.isCanAddThisCar(CarVehicleModel.CarType.Truck))
                {
                    sceneVisualization.createTruck((int)(SettingsModel.ParkingTimeDistribution.GetRandNumber()));
                }  
            }
        }

        private void DrawImage()
        {
            var image = sceneVisualization.getImage();
            var imageSize = image.Size;
            
            pictureBox2.Image?.Dispose();
            pictureBox2.Image = new Bitmap(imageSize.Width * 20, imageSize.Height * 20);

            using (var g = Graphics.FromImage(pictureBox2.Image))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(image, new Rectangle(Point.Empty, pictureBox2.Image.Size));
            }

        }

        private void InitModelTime()
        {
            dateTimeModel = DateTime.Now;
            label27.Text = dateTimeModel.ToString("dd.MM.yyyy HH:mm");
        }
        
        private void SetModelTime()
        {
            dateTimeModel = dateTimeModel.AddMinutes(1);
            label27.Text = dateTimeModel.ToString("dd.MM.yyyy HH:mm");
        }

        #endregion

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            var dropdown = (DomainUpDown) sender;
            var direction = (RoadDirections) dropdown.SelectedIndex;
            SetUpRoadImages(direction);
            if (sceneConstructor.IsParkingModelCreate())
            {
                sceneConstructor.SetRoadDirection(direction);
            }
        }

        private void SetUpRoadImages(RoadDirections direction)
        {
            switch (direction)
            {
                case RoadDirections.Top:
                    pictureRoadBox2.Visible = true;
                    pictureRoadBox1.Visible = false;
                    pictureRoadBox3.Visible = false;
                    pictureRoadBox4.Visible = false;
                    break;
                case RoadDirections.Bottom:
                    pictureRoadBox4.Visible = true;
                    pictureRoadBox1.Visible = false;
                    pictureRoadBox3.Visible = false;
                    pictureRoadBox2.Visible = false;
                    break;
                case RoadDirections.Right:
                    pictureRoadBox1.Visible = true;
                    pictureRoadBox2.Visible = false;
                    pictureRoadBox3.Visible = false;
                    pictureRoadBox4.Visible = false;
                    break;
                case RoadDirections.Left:
                    pictureRoadBox3.Visible = true;
                    pictureRoadBox2.Visible = false;
                    pictureRoadBox1.Visible = false;
                    pictureRoadBox4.Visible = false;
                    break;
            }
        }

        #region Settings Form

        //Настройки
        private void InitSettingsForm()
        {
            textBox2.Text = SettingsModel.DayTimeRate.ToString();
            textBox5.Text = SettingsModel.PercentOfTrack.ToString();
            textBox3.Text = SettingsModel.NightTimeRate.ToString();
            textBox4.Text = SettingsModel.EnteringProbability.ToString();
            label14.Text = SettingsModel.PercentOfCar.ToString();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.calcualePercent(textBox5, label14);
            if (int.TryParse(textBox5.Text, out int value))
            {
                SettingsModel.SetPercentOfTrack(value);
            }
            else
            {
                ShowUncorrectValueMessage();
            }

            InitSettingsForm();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int value))
            {
                SettingsModel.SetDayTimeRate(value);
            }
            else
            {
                ShowUncorrectValueMessage();
            }

            InitSettingsForm();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox3.Text, out int value))
            {
                SettingsModel.SetNightTimeRate(value);
            }
            else
            {
                ShowUncorrectValueMessage();
            }

            InitSettingsForm();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox4.Text, out double value))
            {
                SettingsModel.SetProbabilityOfEnteringToParking(value);
            }
            else
            {
                ShowUncorrectValueMessage();
            }

            InitSettingsForm();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double value))
            {
                SettingsModel.SetGenerationStreamDistribution(new DeterminedDistribution(value));
            }
            else
            {
                ShowUncorrectValueMessage();
            }

            InitSettingsForm();
        }

        private void textBoxWithPlaceholder11_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxWithPlaceholder11.Text, out double value))
            {
                SettingsModel.SetParkingTimeDistribution(new DeterminedDistribution(value));
            }
            else
            {
                ShowUncorrectValueMessage();
            }

            InitSettingsForm();
        }

        private void ShowUncorrectValueMessage()
        {
            MessageBox.Show("Введено некорректное значение!", "Ошибка распознавания", MessageBoxButtons.OK);
        }

        #endregion

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\Help\\help.html");
        }

        private void LockElements(bool isLock)
        {
            radioButton3.Enabled = isLock;
            radioButton4.Enabled = isLock;
            radioButton5.Enabled = isLock;
            textBoxWithPlaceholder1.Enabled = isLock;
            textBoxWithPlaceholder2.Enabled = isLock;
            textBoxWithPlaceholder3.Enabled = isLock;
            textBoxWithPlaceholder4.Enabled = isLock;
            textBoxWithPlaceholder5.Enabled = isLock;
            textBox1.Enabled = !isLock;
        }
    }
}