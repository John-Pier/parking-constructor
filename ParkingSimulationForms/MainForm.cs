using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using ParkingConstructorLib;
using ParkingConstructorLib.logic;
using ParkingConstructorLib.models;
using ParkingConstructorLib.services;
using ParkingConstructorLib.utils.distributions;
using ParkingSimulationForms.views;
using ParkingSimulationForms.views.components;
using ParkingSimulationForms.views.services;

namespace ParkingSimulationForms
{
    public partial class MainForm : Form
    {
        private readonly ParkingSceneConstructor<Image> sceneConstructor = new ParkingSceneConstructor<Image>();
        private readonly ParkingSceneVisualization<Image> sceneVisualization = new ParkingSceneVisualization<Image>();
        private readonly FormFilesService formFilesService = new FormFilesService();
        private readonly MainFormConstructorController constructorController = new MainFormConstructorController();
        private DateTime dateTimeModel;
        private readonly SettingsModel SettingsModel = new SettingsModel();
        private readonly StatisticModel statisticModel = new StatisticModel();
        private bool isFirstOpenTabVizualization = true;
        
        public MainForm()
        {
            InitializeComponent();

            MainFormVizualayzerController.setPictureBox(pictureBox2);
            MainFormVizualayzerController.CurrentSceneVisualization = sceneVisualization;

            constructorController.ImageList = elementsImageList;
            constructorController.ElementsTablePanel = elementsTablePanel;
            constructorController.CurrentSceneConstructor = sceneConstructor;
            constructorController.DrawTemplate((int) counterHorizontal.Value, (int) counterVertical.Value);

            sceneVisualization.SetStatisticModel(statisticModel);
            
            constructorController.ImageList = texturesImageList; // TODO: В конструктор
            constructorController.CreateAndSetTexturesBitmapArray();
            InitSettingsForm();

            radioButton1.Checked = true;
            radioButton9.Checked = true;

            domainUpDown1.SelectedIndex = 0;

            SetUpRoadImages(RoadDirections.Top);
            InitRoadImages();

            elementsTablePanel.Enabled = false;
            saveButton.Enabled = false;

            InitFormFields();
        }

        private void InitFormFields()
        {
            textBoxWithPlaceholder3.SetNumberChangeHandler(
                SettingModelService.MinGenerationNormalDistributionMValue,
                SettingModelService.MaxGenerationNormalDistributionMValue
            );
            textBoxWithPlaceholder4.SetNumberChangeHandler(
                SettingModelService.MinGenerationNormalDistributionDValue,
                SettingModelService.MaxGenerationNormalDistributionDValue
            );

            textBoxWithPlaceholder5.SetNumberChangeHandler(
                SettingModelService.MinGenerationExponentialDistributionValue,
                SettingModelService.MaxGenerationExponentialDistributionValue
            );

            textBoxWithPlaceholder9.SetNumberChangeHandler(
                SettingModelService.MinParkingTimeUniformDistributionValue,
                SettingModelService.MaxParkingTimeUniformDistributionValue
            );
            textBoxWithPlaceholder10.SetNumberChangeHandler(
                SettingModelService.MinParkingTimeUniformDistributionValue,
                SettingModelService.MaxParkingTimeUniformDistributionValue
            );

            textBoxWithPlaceholder8.SetNumberChangeHandler(
                SettingModelService.MinParkingTimeNormalDistributionMValue,
                SettingModelService.MaxParkingTimeNormalDistributionMValue
            );
            textBoxWithPlaceholder7.SetNumberChangeHandler(
                SettingModelService.MinParkingTimeNormalDistributionDValue,
                SettingModelService.MaxParkingTimeNormalDistributionDValue
            );
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
            constructorController.DrawTemplate(
                (int) counterHorizontal.Value,
                (int) counterVertical.Value
            );
        }

        private void counterVertical_ValueChanged(object sender, EventArgs e)
        {
            constructorController.DrawTemplate(
                (int) counterHorizontal.Value,
                (int) counterVertical.Value
            );
        }

        //Визуализатор
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            MainFormVizualayzerController.changePercentValue(hScrollBar1, label18, modelGeneralTimer);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            constructorController.CurrentElement =
                new GrassParkingElement(elementsImageList.Images[4]); // газон
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            constructorController.CurrentElement = new ExitParkingElement(elementsImageList.Images[3]); // выезд
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            constructorController.CurrentElement =
                new ParkingSpaceElement(elementsImageList.Images[5]); // парвокочное место Л
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            constructorController.CurrentElement =
                new TruckParkingSpaceElement(elementsImageList.Images[7]); // парвокочное место Г
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            constructorController.CurrentElement =
                new CashierParkingElement(elementsImageList.Images[1]); // касса
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetUpConstructorAndLockSize();
            constructorController.CurrentElement = new EntryParkingElement(elementsImageList.Images[2]); //вьезд
        }

        private void button8_Click(object sender, EventArgs e) // Clear
        {
            SetEnableEditSceneSize(true);
            constructorController.CurrentElement = null;
            constructorController.DrawTemplate((int) counterHorizontal.Value, (int) counterVertical.Value);
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
            if (!radioButton1.Checked) return;
            if (double.TryParse(textBox1.Text, out var value) && 
                SettingsModel.SettingService.CheckGenerationDeterminedDistributionValue(value))
            {
                SettingsModel.SetGenerationStreamDistribution(new DeterminedDistribution(value));
            }
            else
            {
                SettingsModel.SetGenerationStreamDistribution(null);
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton9.Checked) return;
            if (double.TryParse(textBoxWithPlaceholder12.Text, out var value) && 
                SettingsModel.SettingService.CheckParkingTimeDistributionValue(value))
            {
                SettingsModel.SetParkingTimeDistribution(new DeterminedDistribution(value));
            }
            else
            {
                SettingsModel.SetParkingTimeDistribution(null);
            }
        }

        private void OnLoadClick(object sender, EventArgs e)
        {
            var parkingModel = formFilesService.OpenDialogAndLoadModel<Image>();

            if (parkingModel == null) return;

            sceneConstructor.SetParkingModel(parkingModel);
            counterHorizontal.Value = sceneConstructor.ParkingModel.ColumnCount;
            counterVertical.Value = sceneConstructor.ParkingModel.RowCount;

            constructorController.DrawTemplate(
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
            if (tabControl1.SelectedIndex == 2 && isFirstOpenTabVizualization)
            {
                isFirstOpenTabVizualization = false;
                MainFormInformationController.initTable(tableLayoutPanel1, tableLayoutPanel2);
                if (sceneConstructor.IsParkingModelCreate() && sceneConstructor.ParkingModel.IsParkingModelCorrect())
                {
                    InitModelTime();

                    sceneVisualization.SetParkingModel(sceneConstructor.ParkingModel);
                    sceneVisualization.NextStep(dateTimeModel);

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
                    sceneVisualization.SetSettingsModel(null);

                    var result = MessageBox.Show(
                        "Вы не можете запустить визуализатор, потому что текущие настройки не валидны или не заданы.\nХотите вернуться ?",
                        "Настройки не валидны",
                        MessageBoxButtons.YesNo
                    );
                    if (result == DialogResult.Yes)
                    {
                        tabControl1.SelectedIndex = 1;
                    }
                }
            }
        }

        #region Timers logic

        private void StartGeneralTimerClick(object sender, EventArgs e)
        {
            if (!sceneVisualization.IsSettingsModelSet()) return;

            modelGeneralTimer.Stop();
            generationStreamTimer.Stop();

            generationStreamTimer.Interval = (int) (SettingsModel.GenerationStreamDistribution.GetRandNumber() * 1000);


            modelGeneralTimer.Start();
            generationStreamTimer.Start();
            generationStreamTimer.Enabled = true;
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
            sceneVisualization.NextStep(dateTimeModel);
            sceneVisualization.Stop();
            DrawImage();
        }

        private void modelGeneralTimer_Tick(object sender, EventArgs e)
        {
            sceneVisualization.NextStep(dateTimeModel);
            MainFormInformationController.updateInformation(sceneVisualization, dateTimeModel, SettingsModel);
            DrawImage();
            SetModelTime();
        }

        private void generationStreamTimer_Tick(object sender, EventArgs e)
        {
            sceneVisualization.CreateVehicle();
            
            var generalInterval = (int) (SettingsModel.GenerationStreamDistribution.GetRandNumber() * 1000);
            generationStreamTimer.Interval = generalInterval;
            generationStreamTimer.Start();
        }

        private void DrawImage()
        {
            var image = sceneVisualization.GetImage();
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
            textBox4.Text = SettingsModel.EnteringProbability.ToString(CultureInfo.CurrentCulture);
            label14.Text = SettingsModel.PercentOfCar.ToString();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.calcualePercent(textBox5, label14);
            if (int.TryParse(textBox5.Text, out var value))
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
            if (int.TryParse(textBox2.Text, out var value) && SettingsModel.SettingService.CheckDayTimeRate(value))
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
            if (int.TryParse(textBox3.Text, out var value) && SettingsModel.SettingService.CheckNightTimeRate(value))
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
            if(!radioButton1.Checked) return;
            
            if (double.TryParse(textBox1.Text, out var value) && 
                SettingsModel.SettingService.CheckGenerationDeterminedDistributionValue(value))
            {
                textBox1.BackColor = Color.White;
                SettingsModel.SetGenerationStreamDistribution(new DeterminedDistribution(value));
            }
            else
            {
                SettingsModel.SetGenerationStreamDistribution(null);
                textBox1.BackColor = Color.Crimson;
            }
        }

        private void textBoxWithPlaceholder12_Leave(object sender, EventArgs e)
        {
            if(!radioButton9.Checked) return;
        
            if (double.TryParse(textBoxWithPlaceholder12.Text, out var value) && 
                SettingsModel.SettingService.CheckParkingTimeDistributionValue(value))
            {
                textBoxWithPlaceholder12.BackColor = Color.White;
                SettingsModel.SetParkingTimeDistribution(new DeterminedDistribution(value));
            }
            else
            {
                SettingsModel.SetParkingTimeDistribution(null);
                textBoxWithPlaceholder12.BackColor = Color.Crimson;
            }
        }

        private void ShowUncorrectedValueMessage()
        {
            MessageBox.Show("Введено некорректное значение!", "Ошибка распознавания", MessageBoxButtons.OK);
        }

        private void textBoxMin_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton3.Checked) return;
            
            var isMinCorrect = double.TryParse(textBoxWithPlaceholder1.Text, out var min);
            var isMaxCorrect = double.TryParse(textBoxWithPlaceholder2.Text, out var max);
            if (isMinCorrect &&
                isMaxCorrect &&
                CheckGenerationUniformDistributionValues(min, max))
            {
                textBoxWithPlaceholder2.BackColor = Color.White;
                textBoxWithPlaceholder1.BackColor = Color.White;
                SettingsModel.SetGenerationStreamDistribution(new UniformDistribution(min, max));
            }

            if (!isMinCorrect ||
                !SettingsModel.SettingService.CheckGenerationUniformDistributionValue(min) ||
                isMaxCorrect && !CheckGenerationUniformDistributionValues(min, max))
            {
                SettingsModel.SetGenerationStreamDistribution(null);
                textBoxWithPlaceholder1.BackColor = Color.Crimson;
            }
        }

        private void textBoxMax_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton3.Checked) return;
            
            var isMinCorrect = double.TryParse(textBoxWithPlaceholder1.Text, out var min);
            var isMaxCorrect = double.TryParse(textBoxWithPlaceholder2.Text, out var max);
            if (isMinCorrect &&
                isMaxCorrect &&
                CheckGenerationUniformDistributionValues(min, max))
            {
                textBoxWithPlaceholder2.BackColor = Color.White;
                textBoxWithPlaceholder1.BackColor = Color.White;
                SettingsModel.SetGenerationStreamDistribution(new UniformDistribution(min, max));
            }

            if (!isMaxCorrect ||
                !SettingsModel.SettingService.CheckGenerationUniformDistributionValue(max) ||
                isMinCorrect && !CheckGenerationUniformDistributionValues(min, max))
            {
                SettingsModel.SetGenerationStreamDistribution(null);
                textBoxWithPlaceholder2.BackColor = Color.Crimson;
            }
        }

        private bool CheckGenerationUniformDistributionValues(double minValue, double maxValue)
        {
            return minValue < maxValue &&
                   SettingsModel.SettingService.CheckGenerationUniformDistributionValue(minValue) &&
                   SettingsModel.SettingService.CheckGenerationUniformDistributionValue(maxValue);
        }

        private void textBoxWithPlaceholder3_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton4.Checked) return;
            
            if (textBoxWithPlaceholder3.IsCorrect)
            {
                textBoxWithPlaceholder3.BackColor = Color.White;
                if (!textBoxWithPlaceholder4.IsCorrect) return;
                textBoxWithPlaceholder4.BackColor = Color.White;
                SettingsModel.SetGenerationStreamDistribution(
                    new NormalDistribution(textBoxWithPlaceholder3.CurrentValue, textBoxWithPlaceholder4.CurrentValue)
                );
            }
            else
            {
                SettingsModel.SetGenerationStreamDistribution(null);
                textBoxWithPlaceholder3.BackColor = Color.Crimson;
            }
        }

        private void textBoxWithPlaceholder4_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton4.Checked) return;
            if (textBoxWithPlaceholder4.IsCorrect)
            {
                textBoxWithPlaceholder4.BackColor = Color.White;
                if (!textBoxWithPlaceholder3.IsCorrect) return;
                textBoxWithPlaceholder3.BackColor = Color.White;
                SettingsModel.SetGenerationStreamDistribution(
                    new NormalDistribution(textBoxWithPlaceholder3.CurrentValue,
                        textBoxWithPlaceholder4.CurrentValue)
                );
            }
            else
            {
                SettingsModel.SetGenerationStreamDistribution(null);
                textBoxWithPlaceholder4.BackColor = Color.Crimson;
            }
        }

        private void radioButton5Normal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton5.Checked) return;
            SettingsModel.SetGenerationStreamDistribution(
                textBoxWithPlaceholder5.IsCorrect
                    ? new ExponentialDistribution(textBoxWithPlaceholder5.CurrentValue)
                    : null
            );
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton4.Checked) return;

            SettingsModel.SetGenerationStreamDistribution(
                textBoxWithPlaceholder3.IsCorrect && textBoxWithPlaceholder4.IsCorrect
                    ? new NormalDistribution(textBoxWithPlaceholder3.CurrentValue,
                        textBoxWithPlaceholder4.CurrentValue)
                    : null
            );
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton3.Checked) return;
            
            var isMinCorrect = double.TryParse(textBoxWithPlaceholder1.Text, out var min);
            var isMaxCorrect = double.TryParse(textBoxWithPlaceholder2.Text, out var max);
            if (isMinCorrect &&
                isMaxCorrect &&
                CheckGenerationUniformDistributionValues(min, max))
            {
                SettingsModel.SetGenerationStreamDistribution(new UniformDistribution(min, max));
            }
            else
            {
                SettingsModel.SetGenerationStreamDistribution(null);
            }
        }

        private void textBoxWithPlaceholder5_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton5.Checked) return;
            
            if (textBoxWithPlaceholder5.IsCorrect)
            {
                textBoxWithPlaceholder5.BackColor = Color.White;
                SettingsModel.SetGenerationStreamDistribution(
                    new ExponentialDistribution(textBoxWithPlaceholder5.CurrentValue)
                );
            }
            else
            {
                SettingsModel.SetGenerationStreamDistribution(null);
                textBoxWithPlaceholder5.BackColor = Color.Crimson;
            }
        }

        private void radioButton8Uniform_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton8.Checked) return;
            SettingsModel.SetParkingTimeDistribution(
                checkCorrectDistributionValues(textBoxWithPlaceholder9, textBoxWithPlaceholder10)
                    ? new UniformDistribution(textBoxWithPlaceholder9.CurrentValue,
                        textBoxWithPlaceholder10.CurrentValue)
                    : null
            );
        }

        private void radioButton7Normal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton7.Checked) return;
            SettingsModel.SetParkingTimeDistribution(
                checkCorrectDistributionValues(textBoxWithPlaceholder8, textBoxWithPlaceholder7)
                    ? new NormalDistribution(textBoxWithPlaceholder8.CurrentValue, textBoxWithPlaceholder7.CurrentValue)
                    : null
            );
        }

        private void textBoxWithPlaceholder9MinUniform_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton8.Checked) return;
            if (textBoxWithPlaceholder9.IsCorrect)
            {
                textBoxWithPlaceholder9.BackColor = Color.White;
                if (!checkCorrectDistributionValues(textBoxWithPlaceholder9, textBoxWithPlaceholder10)) return;
                textBoxWithPlaceholder10.BackColor = Color.White;
                SettingsModel.SetParkingTimeDistribution(
                    new UniformDistribution(
                        textBoxWithPlaceholder9.CurrentValue,
                        textBoxWithPlaceholder10.CurrentValue
                    )
                );
            }
            else
            {
                SettingsModel.SetParkingTimeDistribution(null);
                textBoxWithPlaceholder9.BackColor = Color.Crimson;
            }
        }

        private void textBoxWithPlaceholder10MaxUniform_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton8.Checked) return;
            if (textBoxWithPlaceholder10.IsCorrect)
            {
                textBoxWithPlaceholder10.BackColor = Color.White;
                if (!checkCorrectDistributionValues(textBoxWithPlaceholder9, textBoxWithPlaceholder10)) return;
                textBoxWithPlaceholder9.BackColor = Color.White;
                SettingsModel.SetParkingTimeDistribution(
                    new UniformDistribution(
                        textBoxWithPlaceholder9.CurrentValue,
                        textBoxWithPlaceholder10.CurrentValue
                    )
                );
            }
            else
            {
                SettingsModel.SetParkingTimeDistribution(null);
                textBoxWithPlaceholder10.BackColor = Color.Crimson;
            }
        }

        private void textBoxWithPlaceholder8MNormal_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton7.Checked) return;
            if (textBoxWithPlaceholder8.IsCorrect)
            {
                textBoxWithPlaceholder8.BackColor = Color.White;
                if (!textBoxWithPlaceholder7.IsCorrect) return;
                textBoxWithPlaceholder7.BackColor = Color.White;
                SettingsModel.SetParkingTimeDistribution(
                    new NormalDistribution(
                        textBoxWithPlaceholder8.CurrentValue,
                        textBoxWithPlaceholder7.CurrentValue
                    )
                );
            }
            else
            {
                SettingsModel.SetParkingTimeDistribution(null);
                textBoxWithPlaceholder8.BackColor = Color.Crimson;
            }
        }

        private void textBoxWithPlaceholder7DNormal_TextChanged(object sender, EventArgs e)
        {
            if (!radioButton7.Checked) return;
            if (textBoxWithPlaceholder7.IsCorrect)
            {
                textBoxWithPlaceholder7.BackColor = Color.White;
                if (!textBoxWithPlaceholder8.IsCorrect) return;
                textBoxWithPlaceholder8.BackColor = Color.White;
                SettingsModel.SetParkingTimeDistribution(
                    new NormalDistribution(
                        textBoxWithPlaceholder8.CurrentValue,
                        textBoxWithPlaceholder7.CurrentValue
                    )
                );
            }
            else
            {
                SettingsModel.SetParkingTimeDistribution(null);
                textBoxWithPlaceholder7.BackColor = Color.Crimson;
            }
        }

        private bool checkCorrectDistributionValues(TextBoxWithPlaceholder minTextBox,
            TextBoxWithPlaceholder maxTextBox)
        {
            return minTextBox.IsCorrect &&
                   maxTextBox.IsCorrect &&
                   minTextBox.CurrentValue < maxTextBox.CurrentValue;
        }

        #endregion

        private void button13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\resources\\help\\help.html"); // ParkingSimulationForms/resources/help/help.html \\resources\\help\\help.html
        }
    }
}