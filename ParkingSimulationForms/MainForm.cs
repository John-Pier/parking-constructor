using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        public SettingsModel SettingsModel = new SettingsModel();

        public MainForm()
        {
            InitializeComponent();
            MainFormVizualayzerController.setPictureBox(pictureBox2);
            MainFormVizualayzerController.CurrentSceneVisualization = sceneVisualization;

            MainFormConstructorController.ImageList = elementsImageList;
            MainFormConstructorController.ElementsTablePanel = elementsTablePanel;
            MainFormConstructorController.CurrentSceneConstructor = sceneConstructor;
            MainFormConstructorController.DrawTemplate((int)counterHorizontal.Value, (int)counterVertical.Value);

            MainFormInformationController.initTable(tableLayoutPanel1, tableLayoutPanel2);
            MainFormStatisticsController.initTable(tableLayoutPanel3);

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
            MainFormConstructorController.DrawTemplate((int)counterHorizontal.Value,
                (int)counterVertical.Value);
        }

        private void counterVertical_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate((int)counterHorizontal.Value,
                (int)counterVertical.Value);
        }

        //Визуализатор
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            MainFormVizualayzerController.changePercentValue(hScrollBar1, label18);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            MainFormConstructorController.CurrentElement = new GrassParkingElement(elementsImageList.Images[4]); // газон
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
            MainFormConstructorController.DrawTemplate((int)counterHorizontal.Value, (int)counterVertical.Value);
            sceneConstructor.ClearModel();
        }

        private void SetUpConstructorAndLockSize()
        {
            if (!sceneConstructor.IsParkingModelCreate())
            {
                sceneConstructor.CreateParkingModel((int)counterHorizontal.Value, (int)counterVertical.Value, (RoadDirections)domainUpDown1.SelectedIndex);
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
            MainFormSettingsController.LockRBs(radioButton3, radioButton4, radioButton5, textBoxWithPlaceholder1,
                textBoxWithPlaceholder2, textBoxWithPlaceholder3, textBoxWithPlaceholder4, textBoxWithPlaceholder5,
                textBox1, !((RadioButton)sender).Checked);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.LockRBs(radioButton6, radioButton7, radioButton8, textBoxWithPlaceholder6,
                textBoxWithPlaceholder7, textBoxWithPlaceholder8, textBoxWithPlaceholder9, textBoxWithPlaceholder10,
                textBoxWithPlaceholder11, !((RadioButton)sender).Checked);
        }

        private void OnLoadClick(object sender, EventArgs e)
        {
            var parkingModel = formFilesService.OpenDialogAndLoadModel<Image>();

            if (parkingModel == null) return;

            sceneConstructor.SetParkingModel(parkingModel);
            counterHorizontal.Value = sceneConstructor.ParkingModel.ColumnCount;
            counterVertical.Value = sceneConstructor.ParkingModel.RowCount;

            MainFormConstructorController.DrawTemplate(
                (int)counterHorizontal.Value,
                (int)counterVertical.Value,
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
                    sceneVisualization.SetParkingModel(sceneConstructor.ParkingModel);
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
                    // TODO: Добавит логику в sceneVisualization
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

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            var dropdown = (DomainUpDown)sender;
            var direction = (RoadDirections)dropdown.SelectedIndex;
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
                ShowUncorrectedValueMessage();
            }
            InitSettingsForm();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int value) && SettingsModel.SettingService.CheckDayTimeRate(value))
            {
                SettingsModel.SetDayTimeRate(value);
            }
            else
            {
                ShowUncorrectedValueMessage();
            }
            InitSettingsForm();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox3.Text, out int value) && SettingsModel.SettingService.CheckNightTimeRate(value))
            {
                SettingsModel.SetNightTimeRate(value);
            }
            else
            {
                ShowUncorrectedValueMessage();
            }
            InitSettingsForm();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox4.Text, out var value))
            {
                SettingsModel.SetProbabilityOfEnteringToParking(value);
            }
            else
            {
                ShowUncorrectedValueMessage();
            }
            InitSettingsForm();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out var value) 
                && SettingsModel.SettingService.CheckGenerationDeterminedDistributionValue(value))
            {
                SettingsModel.SetGenerationStreamDistribution(new DeterminedDistribution(value));
            }
            else
            {
                ShowUncorrectedValueMessage();
            }
            InitSettingsForm();
        }

        private void textBoxWithPlaceholder11_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxWithPlaceholder11.Text, out var value) 
                && SettingsModel.SettingService.CheckParkingTimeDistributionValue(value))
            {
                SettingsModel.SetParkingTimeDistribution(new DeterminedDistribution(value));
            }
            else
            {
                ShowUncorrectedValueMessage();
            }
            InitSettingsForm();
        }
        
        private void RandomGenerationStreamCheckboxCheckedChanged(object sender, EventArgs e)
        {
            SettingsModel.SetGenerationStreamDistribution(null);
        }
        
        private void ShowUncorrectedValueMessage()
        {
            MessageBox.Show("Введено некорректное значение!", "Ошибка распознавания", MessageBoxButtons.OK);
        }
        
        #endregion

        private void textBoxMin_TextChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked && 
                double.TryParse(textBoxWithPlaceholder1.Text, out var value) &&
                SettingsModel.SettingService.CheckGenerationUniformDistributionValue(value))
            {
                MainFormSettingsController.minGenerationValue = value;
            }
        }

        private void textBoxMax_TextChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked && 
                double.TryParse(textBoxWithPlaceholder1.Text, out var value) &&
                SettingsModel.SettingService.CheckGenerationUniformDistributionValue(value))
            {
                MainFormSettingsController.maxGenerationValue = value;
            }
        }
    }
}