namespace PenkovNikitaKR
{
    partial class RedaktirovanieZakazov
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
            this.comboBoxClientDeviceName = new System.Windows.Forms.ComboBox();
            this.comboBoxVIPStatus = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxOrderDescription = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxComponentQuantity = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxEmployee = new System.Windows.Forms.ComboBox();
            this.comboBoxService = new System.Windows.Forms.ComboBox();
            this.textBoxTotalCost = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxComponentCost = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxServiceCost = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxOrderStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPhoneNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.monthCalendar2 = new System.Windows.Forms.MonthCalendar();
            this.comboBoxClientName = new System.Windows.Forms.ComboBox();
            this.comboBoxComponentName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxClientDeviceName
            // 
            this.comboBoxClientDeviceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxClientDeviceName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClientDeviceName.FormattingEnabled = true;
            this.comboBoxClientDeviceName.Location = new System.Drawing.Point(712, 16);
            this.comboBoxClientDeviceName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxClientDeviceName.Name = "comboBoxClientDeviceName";
            this.comboBoxClientDeviceName.Size = new System.Drawing.Size(262, 32);
            this.comboBoxClientDeviceName.TabIndex = 206;
            this.comboBoxClientDeviceName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // comboBoxVIPStatus
            // 
            this.comboBoxVIPStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxVIPStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxVIPStatus.FormattingEnabled = true;
            this.comboBoxVIPStatus.Items.AddRange(new object[] {
            "Администратор",
            "Пользователь"});
            this.comboBoxVIPStatus.Location = new System.Drawing.Point(192, 168);
            this.comboBoxVIPStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxVIPStatus.Name = "comboBoxVIPStatus";
            this.comboBoxVIPStatus.Size = new System.Drawing.Size(183, 32);
            this.comboBoxVIPStatus.TabIndex = 198;
            this.comboBoxVIPStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(708, 309);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(240, 24);
            this.label14.TabIndex = 197;
            this.label14.Text = "Дата завершения работы";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label15.Location = new System.Drawing.Point(446, 309);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(191, 24);
            this.label15.TabIndex = 196;
            this.label15.Text = "Дата начала работы";
            // 
            // textBoxOrderDescription
            // 
            this.textBoxOrderDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOrderDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxOrderDescription.Location = new System.Drawing.Point(633, 176);
            this.textBoxOrderDescription.MaxLength = 45;
            this.textBoxOrderDescription.Multiline = true;
            this.textBoxOrderDescription.Name = "textBoxOrderDescription";
            this.textBoxOrderDescription.Size = new System.Drawing.Size(340, 104);
            this.textBoxOrderDescription.TabIndex = 195;
            this.textBoxOrderDescription.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.textBoxOrderDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxNameComponentDevice_KeyPress);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(460, 177);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(164, 24);
            this.label13.TabIndex = 194;
            this.label13.Text = "Описание заказа";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(459, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(254, 24);
            this.label12.TabIndex = 193;
            this.label12.Text = "Название девайса клиента";
            // 
            // textBoxComponentQuantity
            // 
            this.textBoxComponentQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComponentQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxComponentQuantity.Location = new System.Drawing.Point(711, 116);
            this.textBoxComponentQuantity.MaxLength = 45;
            this.textBoxComponentQuantity.Name = "textBoxComponentQuantity";
            this.textBoxComponentQuantity.Size = new System.Drawing.Size(263, 29);
            this.textBoxComponentQuantity.TabIndex = 192;
            this.textBoxComponentQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBoxPhoneNumber_KeyPress);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(459, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(242, 24);
            this.label11.TabIndex = 191;
            this.label11.Text = "Количество компонентов";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(459, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(210, 24);
            this.label9.TabIndex = 190;
            this.label9.Text = "Название компонента";
            // 
            // comboBoxEmployee
            // 
            this.comboBoxEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxEmployee.FormattingEnabled = true;
            this.comboBoxEmployee.Items.AddRange(new object[] {
            "Администратор",
            "Пользователь"});
            this.comboBoxEmployee.Location = new System.Drawing.Point(192, 14);
            this.comboBoxEmployee.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxEmployee.Name = "comboBoxEmployee";
            this.comboBoxEmployee.Size = new System.Drawing.Size(183, 32);
            this.comboBoxEmployee.TabIndex = 189;
            this.comboBoxEmployee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // comboBoxService
            // 
            this.comboBoxService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxService.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxService.FormattingEnabled = true;
            this.comboBoxService.Items.AddRange(new object[] {
            "Администратор",
            "Пользователь"});
            this.comboBoxService.Location = new System.Drawing.Point(192, 219);
            this.comboBoxService.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxService.Name = "comboBoxService";
            this.comboBoxService.Size = new System.Drawing.Size(183, 32);
            this.comboBoxService.TabIndex = 188;
            this.comboBoxService.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // textBoxTotalCost
            // 
            this.textBoxTotalCost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTotalCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxTotalCost.Location = new System.Drawing.Point(192, 372);
            this.textBoxTotalCost.MaxLength = 45;
            this.textBoxTotalCost.Name = "textBoxTotalCost";
            this.textBoxTotalCost.Size = new System.Drawing.Size(183, 29);
            this.textBoxTotalCost.TabIndex = 187;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(11, 370);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(186, 24);
            this.label10.TabIndex = 186;
            this.label10.Text = "Цена за всю работу";
            // 
            // textBoxComponentCost
            // 
            this.textBoxComponentCost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxComponentCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxComponentCost.Location = new System.Drawing.Point(192, 318);
            this.textBoxComponentCost.MaxLength = 45;
            this.textBoxComponentCost.Name = "textBoxComponentCost";
            this.textBoxComponentCost.Size = new System.Drawing.Size(183, 29);
            this.textBoxComponentCost.TabIndex = 185;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(11, 314);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(182, 24);
            this.label8.TabIndex = 184;
            this.label8.Text = "Цена за компонент";
            // 
            // textBoxServiceCost
            // 
            this.textBoxServiceCost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxServiceCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxServiceCost.Location = new System.Drawing.Point(192, 276);
            this.textBoxServiceCost.MaxLength = 45;
            this.textBoxServiceCost.Name = "textBoxServiceCost";
            this.textBoxServiceCost.Size = new System.Drawing.Size(183, 29);
            this.textBoxServiceCost.TabIndex = 183;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(11, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 24);
            this.label7.TabIndex = 182;
            this.label7.Text = "Цена услуги";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(11, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 24);
            this.label6.TabIndex = 181;
            this.label6.Text = "Название услуги";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(11, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 24);
            this.label4.TabIndex = 180;
            this.label4.Text = "Статус VIP";
            // 
            // comboBoxOrderStatus
            // 
            this.comboBoxOrderStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxOrderStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxOrderStatus.FormattingEnabled = true;
            this.comboBoxOrderStatus.Items.AddRange(new object[] {
            "Администратор",
            "Пользователь"});
            this.comboBoxOrderStatus.Location = new System.Drawing.Point(192, 418);
            this.comboBoxOrderStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxOrderStatus.Name = "comboBoxOrderStatus";
            this.comboBoxOrderStatus.Size = new System.Drawing.Size(183, 32);
            this.comboBoxOrderStatus.TabIndex = 179;
            this.comboBoxOrderStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(11, 420);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 24);
            this.label3.TabIndex = 178;
            this.label3.Text = "Статус заказа";
            // 
            // textBoxPhoneNumber
            // 
            this.textBoxPhoneNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxPhoneNumber.Location = new System.Drawing.Point(192, 120);
            this.textBoxPhoneNumber.MaxLength = 45;
            this.textBoxPhoneNumber.Name = "textBoxPhoneNumber";
            this.textBoxPhoneNumber.Size = new System.Drawing.Size(183, 29);
            this.textBoxPhoneNumber.TabIndex = 177;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(11, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 24);
            this.label5.TabIndex = 176;
            this.label5.Text = "Номер телефона";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(11, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 24);
            this.label2.TabIndex = 174;
            this.label2.Text = "ФИО Клиента";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(11, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 24);
            this.label1.TabIndex = 173;
            this.label1.Text = "ФИО Сотрудника";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(192, 462);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(182, 53);
            this.button2.TabIndex = 170;
            this.button2.Text = "Назад";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(10, 462);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 53);
            this.button1.TabIndex = 169;
            this.button1.Text = "Редактировать";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.monthCalendar1.Location = new System.Drawing.Point(464, 341);
            this.monthCalendar1.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.monthCalendar1.MinDate = new System.DateTime(2023, 1, 1, 0, 0, 0, 0);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 207;
            // 
            // monthCalendar2
            // 
            this.monthCalendar2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.monthCalendar2.Location = new System.Drawing.Point(736, 341);
            this.monthCalendar2.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.monthCalendar2.MinDate = new System.DateTime(2023, 1, 1, 0, 0, 0, 0);
            this.monthCalendar2.Name = "monthCalendar2";
            this.monthCalendar2.TabIndex = 208;
            // 
            // comboBoxClientName
            // 
            this.comboBoxClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxClientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxClientName.FormattingEnabled = true;
            this.comboBoxClientName.Items.AddRange(new object[] {
            "Администратор",
            "Пользователь"});
            this.comboBoxClientName.Location = new System.Drawing.Point(192, 67);
            this.comboBoxClientName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxClientName.Name = "comboBoxClientName";
            this.comboBoxClientName.Size = new System.Drawing.Size(183, 32);
            this.comboBoxClientName.TabIndex = 209;
            this.comboBoxClientName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // comboBoxComponentName
            // 
            this.comboBoxComponentName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComponentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxComponentName.FormattingEnabled = true;
            this.comboBoxComponentName.Items.AddRange(new object[] {
            "Администратор",
            "Пользователь"});
            this.comboBoxComponentName.Location = new System.Drawing.Point(712, 67);
            this.comboBoxComponentName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxComponentName.Name = "comboBoxComponentName";
            this.comboBoxComponentName.Size = new System.Drawing.Size(262, 32);
            this.comboBoxComponentName.TabIndex = 210;
            this.comboBoxComponentName.SelectedIndexChanged += new System.EventHandler(this.comboBoxComponentName_SelectedIndexChanged);
            this.comboBoxComponentName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBox_KeyPress);
            // 
            // RedaktirovanieZakazov
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(991, 525);
            this.Controls.Add(this.comboBoxComponentName);
            this.Controls.Add(this.comboBoxClientName);
            this.Controls.Add(this.monthCalendar2);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.comboBoxClientDeviceName);
            this.Controls.Add(this.comboBoxVIPStatus);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxOrderDescription);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxComponentQuantity);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBoxEmployee);
            this.Controls.Add(this.comboBoxService);
            this.Controls.Add(this.textBoxTotalCost);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxComponentCost);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxServiceCost);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxOrderStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxPhoneNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RedaktirovanieZakazov";
            this.Text = "Редактирование заказов";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxClientDeviceName;
        private System.Windows.Forms.ComboBox comboBoxVIPStatus;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxOrderDescription;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxComponentQuantity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxEmployee;
        private System.Windows.Forms.ComboBox comboBoxService;
        private System.Windows.Forms.TextBox textBoxTotalCost;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxComponentCost;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxServiceCost;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxOrderStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPhoneNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.MonthCalendar monthCalendar2;
        private System.Windows.Forms.ComboBox comboBoxClientName;
        private System.Windows.Forms.ComboBox comboBoxComponentName;
    }
}