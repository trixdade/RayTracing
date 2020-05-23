namespace RayTracing
{
    partial class RayTracing
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
            this.tbPosX = new System.Windows.Forms.TrackBar();
            this.tbPosY = new System.Windows.Forms.TrackBar();
            this.comboSize = new System.Windows.Forms.ComboBox();
            this.comboColor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textCubePosZ = new System.Windows.Forms.TextBox();
            this.textCubePosY = new System.Windows.Forms.TextBox();
            this.textCubePosX = new System.Windows.Forms.TextBox();
            this.buttonAddCube = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPosY)).BeginInit();
            this.SuspendLayout();
            // 
            // tbPosX
            // 
            this.tbPosX.Location = new System.Drawing.Point(12, 613);
            this.tbPosX.Maximum = 50;
            this.tbPosX.Minimum = -50;
            this.tbPosX.Name = "tbPosX";
            this.tbPosX.Size = new System.Drawing.Size(595, 45);
            this.tbPosX.TabIndex = 2;
            this.tbPosX.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbPosX.Value = 2;
            this.tbPosX.Scroll += new System.EventHandler(this.tbPosX_Scroll);
            // 
            // tbPosY
            // 
            this.tbPosY.Location = new System.Drawing.Point(613, 12);
            this.tbPosY.Maximum = 50;
            this.tbPosY.Minimum = -50;
            this.tbPosY.Name = "tbPosY";
            this.tbPosY.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbPosY.Size = new System.Drawing.Size(45, 595);
            this.tbPosY.TabIndex = 3;
            this.tbPosY.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbPosY.Value = 2;
            this.tbPosY.Scroll += new System.EventHandler(this.tbPosY_Scroll);
            // 
            // comboSize
            // 
            this.comboSize.FormattingEnabled = true;
            this.comboSize.Items.AddRange(new object[] {
            "0.1",
            "0.3",
            "0.5",
            "1"});
            this.comboSize.Location = new System.Drawing.Point(670, 159);
            this.comboSize.Name = "comboSize";
            this.comboSize.Size = new System.Drawing.Size(83, 21);
            this.comboSize.TabIndex = 8;
            this.comboSize.Text = "Size";
            // 
            // comboColor
            // 
            this.comboColor.FormattingEnabled = true;
            this.comboColor.Items.AddRange(new object[] {
            "RED",
            "GREEN",
            "BLUE"});
            this.comboColor.Location = new System.Drawing.Point(670, 132);
            this.comboColor.Name = "comboColor";
            this.comboColor.Size = new System.Drawing.Size(83, 21);
            this.comboColor.TabIndex = 7;
            this.comboColor.Text = "Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(667, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Z:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(667, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(667, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "X:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textCubePosZ
            // 
            this.textCubePosZ.Location = new System.Drawing.Point(690, 92);
            this.textCubePosZ.Name = "textCubePosZ";
            this.textCubePosZ.Size = new System.Drawing.Size(67, 20);
            this.textCubePosZ.TabIndex = 3;
            this.textCubePosZ.Text = "0";
            this.textCubePosZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textCubePosY
            // 
            this.textCubePosY.Location = new System.Drawing.Point(690, 64);
            this.textCubePosY.Name = "textCubePosY";
            this.textCubePosY.Size = new System.Drawing.Size(67, 20);
            this.textCubePosY.TabIndex = 2;
            this.textCubePosY.Text = "0";
            this.textCubePosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textCubePosX
            // 
            this.textCubePosX.Location = new System.Drawing.Point(690, 38);
            this.textCubePosX.Name = "textCubePosX";
            this.textCubePosX.Size = new System.Drawing.Size(67, 20);
            this.textCubePosX.TabIndex = 1;
            this.textCubePosX.Text = "0";
            this.textCubePosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonAddCube
            // 
            this.buttonAddCube.Location = new System.Drawing.Point(670, 196);
            this.buttonAddCube.Name = "buttonAddCube";
            this.buttonAddCube.Size = new System.Drawing.Size(144, 67);
            this.buttonAddCube.TabIndex = 0;
            this.buttonAddCube.Text = "Add cube";
            this.buttonAddCube.UseVisualStyleBackColor = true;
            this.buttonAddCube.Click += new System.EventHandler(this.buttonAddCube_Click);
            // 
            // RayTracing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 668);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboColor);
            this.Controls.Add(this.textCubePosZ);
            this.Controls.Add(this.tbPosY);
            this.Controls.Add(this.textCubePosY);
            this.Controls.Add(this.tbPosX);
            this.Controls.Add(this.textCubePosX);
            this.Controls.Add(this.buttonAddCube);
            this.Name = "RayTracing";
            this.Text = "RayTracing";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPosY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl GLView;
        private System.Windows.Forms.TrackBar tbPosX;
        private System.Windows.Forms.TrackBar tbPosY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textCubePosZ;
        private System.Windows.Forms.TextBox textCubePosY;
        private System.Windows.Forms.TextBox textCubePosX;
        private System.Windows.Forms.Button buttonAddCube;
        private System.Windows.Forms.ComboBox comboSize;
        private System.Windows.Forms.ComboBox comboColor;
    }
}

