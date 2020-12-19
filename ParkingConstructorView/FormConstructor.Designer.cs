namespace ParkingConstructorView
{
    partial class FormConstructor
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.конструкторToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTest = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.dataConstructorGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataConstructorGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.конструкторToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // конструкторToolStripMenuItem
            // 
            this.конструкторToolStripMenuItem.Name = "конструкторToolStripMenuItem";
            this.конструкторToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.конструкторToolStripMenuItem.Text = "Конструктор";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(15, 68);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "buttonTest";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 39);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(88, 13);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Тестовая лабла";
            // 
            // dataConstructorGridView
            // 
            this.dataConstructorGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataConstructorGridView.Location = new System.Drawing.Point(418, 91);
            this.dataConstructorGridView.Name = "dataConstructorGridView";
            this.dataConstructorGridView.Size = new System.Drawing.Size(358, 241);
            this.dataConstructorGridView.TabIndex = 5;
            // 
            // FormConstructor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataConstructorGridView);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormConstructor";
            this.Text = "FormConstructor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataConstructorGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem конструкторToolStripMenuItem;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DataGridView dataConstructorGridView;
    }
}

