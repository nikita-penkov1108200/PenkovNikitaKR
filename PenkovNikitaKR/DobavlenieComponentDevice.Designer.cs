namespace PenkovNikitaKR
{
    partial class DobavlenieComponentDevice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxNameComponentDevice = new System.Windows.Forms.TextBox();
            this.textBoxCostComponentDevice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.button2.Location = new System.Drawing.Point(360, 126);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(226, 55);
            this.button2.TabIndex = 22;
            this.button2.Text = "Назад";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.button1.Location = new System.Drawing.Point(11, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(226, 55);
            this.button1.TabIndex = 21;
            this.button1.Text = "Добавить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxNameComponentDevice
            // 
            this.textBoxNameComponentDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNameComponentDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.8F);
            this.textBoxNameComponentDevice.Location = new System.Drawing.Point(285, 12);
            this.textBoxNameComponentDevice.Name = "textBoxNameComponentDevice";
            this.textBoxNameComponentDevice.Size = new System.Drawing.Size(301, 35);
            this.textBoxNameComponentDevice.TabIndex = 20;
            this.textBoxNameComponentDevice.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // textBoxCostComponentDevice
            // 
            this.textBoxCostComponentDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCostComponentDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.8F);
            this.textBoxCostComponentDevice.Location = new System.Drawing.Point(285, 59);
            this.textBoxCostComponentDevice.Name = "textBoxCostComponentDevice";
            this.textBoxCostComponentDevice.Size = new System.Drawing.Size(301, 35);
            this.textBoxCostComponentDevice.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 29);
            this.label2.TabIndex = 18;
            this.label2.Text = "Цена компонента";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 29);
            this.label1.TabIndex = 17;
            this.label1.Text = "Название компонента";
            // 
            // DobavlenieComponentDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(598, 193);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxNameComponentDevice);
            this.Controls.Add(this.textBoxCostComponentDevice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DobavlenieComponentDevice";
            this.Text = "Добавление компонентов девайса";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxNameComponentDevice;
        private System.Windows.Forms.TextBox textBoxCostComponentDevice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}