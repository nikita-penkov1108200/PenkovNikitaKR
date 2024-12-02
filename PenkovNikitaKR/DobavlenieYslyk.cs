using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public partial class DobavlenieYslyk : Form
    {
        public DobavlenieYslyk()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            textBoxName.TextChanged += textBoxName_TextChanged;
            textBoxName.KeyPress += textBoxName_KeyPress;
            textBoxCost.KeyPress += textBoxCost_KeyPress;
            textBoxCost.TextChanged += textBoxCost_TextChanged;
            textBoxTime.KeyPress += textBoxTime_KeyPress;
            textBoxTime.TextChanged += textBoxTime_TextChanged;
            textBoxDescription.TextChanged += textBoxDescription_TextChanged;
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (textBoxName.Text.Length > 20)
            {
                textBoxName.Text = textBoxName.Text.Substring(0, 20);
                textBoxName.SelectionStart = textBoxName.Text.Length;
            }

            if (textBoxName.Text.Length > 0)
            {
                textBoxName.Text = char.ToUpper(textBoxName.Text[0]) + textBoxName.Text.Substring(1);
                textBoxName.SelectionStart = textBoxName.Text.Length;
            }
        }

        private void textBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
            {
                e.Handled = true;
            }
            if (textBoxName.Text.Length >= 20 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }
        }
        private void textBoxService_Leave(object sender, EventArgs e)
        {
            // Первая буква с заглавной
            if (!string.IsNullOrEmpty(textBoxName.Text))
                textBoxName.Text = char.ToUpper(textBoxName.Text[0]) + textBoxName.Text.Substring(1);
        }

        private bool IsCyrillic(char c)
        {
            return Regex.IsMatch(c.ToString(), @"[\u0400-\u04FF]");
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

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            int cost;
            DateTime time;
            string description = textBoxDescription.Text;

            // Проверка на корректность ввода
            if (string.IsNullOrWhiteSpace(name) || !int.TryParse(textBoxCost.Text, out cost) || !DateTime.TryParse(textBoxTime.Text, out time))
            {
                MessageBox.Show("Пожалуйста, введите корректные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();

                // Проверка на дублирование
                string checkQuery = "SELECT COUNT(*) FROM services WHERE Name = @Name";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@Name", name);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Услуга с таким именем уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Вставка данных
                string insertQuery = "INSERT INTO services (Name, Cost, Time, DescriptionServices) VALUES (@Name, @Cost, @Time, @DescriptionServices)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Cost", cost);
                    cmd.Parameters.AddWithValue("@Time", time);
                    cmd.Parameters.AddWithValue("@DescriptionServices", description);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Услуга успешно добавлена.", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
