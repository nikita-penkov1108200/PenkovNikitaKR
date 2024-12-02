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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PenkovNikitaKR
{
    public partial class RedactirovanieYslyk : Form
    {
        private DataTable servicesTable = new DataTable();
        private int selectedServiceId;
        public RedactirovanieYslyk()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            LoadServices();
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            textBoxCost.KeyPress += textBoxCost_KeyPress;
            textBoxCost.TextChanged += textBoxCost_TextChanged;
            textBoxTime.KeyPress += textBoxTime_KeyPress;
            textBoxTime.TextChanged += textBoxTime_TextChanged;
            textBoxDescription.TextChanged += textBoxDescription_TextChanged;
        }

        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadServices()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idservices, Name FROM services";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                adapter.Fill(servicesTable);
            }
        }

        public void LoadServiceData(int serviceId)
        {
            selectedServiceId = serviceId;

            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT * FROM services WHERE idservices = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", selectedServiceId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxService.Text = reader["Name"].ToString(); // Заполняем текстбокс
                            textBoxCost.Text = reader["Cost"].ToString();
                            textBoxTime.Text = reader["Time"].ToString(); // Получаем время как строку
                            textBoxDescription.Text = reader["DescriptionServices"].ToString();
                        }
                    }
                }
            }
        }

        private void textBoxCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем Backspace
            if (char.IsControl(e.KeyChar)) return;

            if ((textBoxCost.Text.Length >= 5 && e.KeyChar != (char)Keys.Back) || !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxCost_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCost.Text.Length > 5)
            {
                textBoxCost.Text = textBoxCost.Text.Substring(0, 5);
                textBoxCost.SelectionStart = textBoxCost.Text.Length;
            }
        }

        private void textBoxTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем Backspace
            if (char.IsControl(e.KeyChar)) return;

            // Ограничение на ввод только цифр и двоеточий
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != ':')
            {
                e.Handled = true;
            }

            // Добавление двоеточий в нужные позиции
            if (textBoxTime.Text.Length == 2 && e.KeyChar != (char)Keys.Back)
            {
                textBoxTime.Text += ":";
                textBoxTime.SelectionStart = textBoxTime.Text.Length; // Установка курсора в конец
            }
            else if (textBoxTime.Text.Length == 5 && e.KeyChar != (char)Keys.Back)
            {
                textBoxTime.Text += ":";
                textBoxTime.SelectionStart = textBoxTime.Text.Length; // Установка курсора в конец
            }
        }

        private void textBoxTime_TextChanged(object sender, EventArgs e)
        {
            // Ограничение на 8 символов (HH:mm:ss)
            if (textBoxTime.Text.Length > 8)
            {
                textBoxTime.Text = textBoxTime.Text.Substring(0, 8);
                textBoxTime.SelectionStart = textBoxTime.Text.Length;
            }
        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {
            // Ограничение на 40 символов
            if (textBoxDescription.Text.Length > 40)
            {
                textBoxDescription.Text = textBoxDescription.Text.Substring(0, 40);
                textBoxDescription.SelectionStart = textBoxDescription.Text.Length; // Установка курсора в конец
            }
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


        private void textBoxService_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
            {
                e.Handled = true;
            }
            if (textBoxService.Text.Length >= 20 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }
        }
        private void textBoxService_Leave(object sender, EventArgs e)
        {
            // Первая буква с заглавной
            if (!string.IsNullOrEmpty(textBoxService.Text))
                textBoxService.Text = char.ToUpper(textBoxService.Text[0]) + textBoxService.Text.Substring(1);
        }

        // Метод для проверки, является ли символ кириллическим
        private bool IsCyrillic(char c)
        {
            return (c >= '\u0400' && c <= '\u04FF'); // Диапазон кириллических символов
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string updateQuery = "UPDATE services SET Name = @name, Cost = @cost, Time = @time, DescriptionServices = @description WHERE idservices = @id";
                using (MySqlCommand cmd = new MySqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@name", textBoxService.Text); 
                    cmd.Parameters.AddWithValue("@cost", textBoxCost.Text);
                    cmd.Parameters.AddWithValue("@time", textBoxTime.Text);
                    cmd.Parameters.AddWithValue("@description", textBoxDescription.Text);
                    cmd.Parameters.AddWithValue("@id", selectedServiceId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Visible = false;
            ProsmotrYslyk main = new ProsmotrYslyk();
            main.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrYslyk main = new ProsmotrYslyk();
            main.ShowDialog();
        }
    }
}
