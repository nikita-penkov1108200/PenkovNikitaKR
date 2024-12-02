using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public partial class DobavlenieComponentDevice : Form
    {
        public static Form PreviousForm1 { get; set; }
        public DobavlenieComponentDevice()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            textBoxNameComponentDevice.KeyPress += TextBoxNameComponentDevice_KeyPress;
            textBoxCostComponentDevice.KeyPress += TextBoxCostComponentDevice_KeyPress;
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
            if (textBox != null && textBox.Text.Length > 0)
            {
                // Преобразуем первую букву в заглавную
                string firstLetter = textBox.Text.Substring(0, 1).ToUpper();
                string restOfString = textBox.Text.Substring(1);
                textBox.Text = firstLetter + restOfString;

                // Устанавливаем курсор в конец текста
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string nameComponentDevice = textBoxNameComponentDevice.Text;
            string costComponentDeviceText = textBoxCostComponentDevice.Text;

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(nameComponentDevice) || string.IsNullOrWhiteSpace(costComponentDeviceText))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка, является ли стоимость числом
            if (!decimal.TryParse(costComponentDeviceText, out decimal costComponentDevice))
            {
                MessageBox.Show("Стоимость должна быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на дублирование данных в базе данных
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                try
                {
                    con.Open();
                    // Запрос для проверки существования компонента с таким же именем
                    string checkQuery = "SELECT COUNT(*) FROM componentdevice WHERE NameComponentDevice = @name";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@name", nameComponentDevice);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Компонент с таким именем уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавление данных в базу данных
                    string query = "INSERT INTO componentdevice (NameComponentDevice, CostComponentDevice) VALUES (@name, @cost)";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@name", nameComponentDevice);
                    cmd.Parameters.AddWithValue("@cost", costComponentDevice);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Данные успешно добавлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // Закрываем форму после успешного добавления
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при добавлении данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            this.Visible = false;
            if (PreviousForm1 is DobavlenieZakazov)
            {
                // Если предыдущая форма - это DobavlenieZakazov
                DobavlenieZakazov zakazovForm = new DobavlenieZakazov();
                zakazovForm.Show();
            }
            else
            {
                // Переход на ProsmotrClient в зависимости от роли
                if (CurrentUser.Role == "Администратор")
                {
                    ProsmotrComponentDevice client1 = new ProsmotrComponentDevice(true);
                    client1.Show();
                }
                else if (CurrentUser.Role == "Менеджер")
                {
                    ProsmotrComponentDevice client1 = new ProsmotrComponentDevice(false);
                    client1.Show();
                }
            }
        }
        private void TextBoxNameComponentDevice_KeyPress(object sender, KeyPressEventArgs e)
        {         
            // Ограничение на количество символов
            if (textBoxNameComponentDevice.Text.Length >= 30 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено количество символов
            }
        }

        private void TextBoxCostComponentDevice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли символ управляющим (например, Backspace) или цифрой
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
            }

            // Ограничение на количество символов
            if (textBoxCostComponentDevice.Text.Length >= 7 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено количество символов
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (PreviousForm1 is DobavlenieZakazov)
            {
                // Если предыдущая форма - это DobavlenieZakazov
                DobavlenieZakazov zakazovForm = new DobavlenieZakazov();
                zakazovForm.Show();
            }
            else
            {
                // Переход на ProsmotrClient в зависимости от роли
                if (CurrentUser.Role == "Администратор")
                {
                    ProsmotrComponentDevice client1 = new ProsmotrComponentDevice(true);
                    client1.Show();
                }
                else if (CurrentUser.Role == "Менеджер")
                {
                    ProsmotrComponentDevice client1 = new ProsmotrComponentDevice(false);
                    client1.Show();
                }
            }
        }
    }
}

