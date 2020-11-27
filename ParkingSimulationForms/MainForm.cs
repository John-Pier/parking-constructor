using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingSimulationForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            MainFormVizualayzerController.setPictureBox(pictureBox2);
            MainFormConstructorController.DrawTemplate(pictureBox1, (int)counterHorizontal.Value, (int)counterVertical.Value);
            MainFormInformationController.initTable(tableLayoutPanel1, tableLayoutPanel2);
            MainFormStatisticsController.initTable(tableLayoutPanel3);
            
        }

        //Конструктор

        private void counterHorizontal_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate(pictureBox1, (int)counterHorizontal.Value, (int)counterVertical.Value);
        }

        private void counterVertical_ValueChanged(object sender, EventArgs e)
        {
            MainFormConstructorController.DrawTemplate(pictureBox1, (int)counterHorizontal.Value, (int)counterVertical.Value);
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.LockRBs(radioButton3, radioButton4, radioButton5, !radioButton1.Checked);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            MainFormSettingsController.LockRBs(radioButton8, radioButton7, radioButton6, !radioButton9.Checked);
        }
    }
}
