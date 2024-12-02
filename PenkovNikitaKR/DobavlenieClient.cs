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
    public partial class DobavlenieClient : Form
    {
        public static Form PreviousForm { get; set; }
        public DobavlenieClient()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            textBoxName.MaxLength = 20;
            textBoxSurname.MaxLength = 20;
            textBoxMiddleName.MaxLength = 20;
            textBoxPhoneNumber.MaxLength = 11;

            textBoxName.KeyPress += TextBox_KeyPress_Cyrillic;
            textBoxSurname.KeyPress += TextBox_KeyPress_Cyrillic;
            textBoxMiddleName.KeyPress += TextBox_KeyPress_Cyrillic;
            textBoxPhoneNumber.KeyPress += TextBox_KeyPress_Numbers;

            textBoxName.TextChanged += TextBox_TextChanged;
            textBoxSurname.TextChanged += TextBox_TextChanged;
            textBoxMiddleName.TextChanged += TextBox_TextChanged;

            LoadStatusVIPData(); // Загрузка данных в комбобокс
            comboBoxStatusVIP.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void LoadStatusVIPData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT StatusVIP FROM statusvip"; // Запрос для получения статусов
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем значения в комбобокс
                            comboBoxStatusVIP.Items.Add(reader["StatusVIP"].ToString());
                        }
                    }
                }
            }
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
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (PreviousForm is DobavlenieZakazov)
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
                    ProsmotrClient client1 = new ProsmotrClient(true);
                    client1.Show();
                }
                else if (CurrentUser.Role == "Менеджер")
                {
                    ProsmotrClient client1 = new ProsmotrClient(false);
                    client1.Show();
                }
            }
        }
        private void TextBox_KeyPress_Cyrillic(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только кириллических символов и BackSpace
            if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }

        private void TextBox_KeyPress_Numbers(object sender, KeyPressEventArgs e)
        {
            // Разрешаем ввод только цифр и BackSpace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }

        // Метод для проверки, является ли символ кириллическим
        private bool IsCyrillic(char c)
        {
            return (c >= '\u0400' && c <= '\u04FF'); // Проверка на диапазон кириллических символов
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string surname = textBoxSurname.Text;
            string middleName = textBoxMiddleName.Text;
            string phoneNumber = textBoxPhoneNumber.Text;
            string statusVIP = comboBoxStatusVIP.SelectedItem?.ToString();

            // Проверка на пустые значения
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                string.IsNullOrWhiteSpace(middleName) || string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(statusVIP))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получение ID статуса VIP
            int statusVIPId;
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string statusQuery = "SELECT idstatusVIP FROM statusvip WHERE StatusVIP = @StatusVIP";
                using (MySqlCommand statusCmd = new MySqlCommand(statusQuery, con))
                {
                    statusCmd.Parameters.AddWithValue("@StatusVIP", statusVIP);
                    object result = statusCmd.ExecuteScalar();

                    // Проверка, найден ли статус
                    if (result == null)
                    {
                        MessageBox.Show("Выбранный статус VIP не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    statusVIPId = Convert.ToInt32(result);
                }

                // Проверка на дублирование
                string checkQuery = "SELECT COUNT(*) FROM client WHERE Name = @Name AND Surname = @Surname AND MiddleName = @MiddleName";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.AddWithValue("@Name", name);
                    checkCmd.Parameters.AddWithValue("@Surname", surname);
                    checkCmd.Parameters.AddWithValue("@MiddleName", middleName);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Клиент с такими данными уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Вставка данных
                string insertQuery = "INSERT INTO client (Name, Surname, MiddleName, PhoneNumber, StatusVIP, statusvip_idstatusVIP) VALUES (@Name, @Surname, @MiddleName, @PhoneNumber, @StatusVIP, @StatusVIPId)";
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@MiddleName", middleName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@StatusVIP", statusVIP);
                    cmd.Parameters.AddWithValue("@StatusVIPId", statusVIPId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Клиент успешно добавлен.", "Добавление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();

            if (PreviousForm is DobavlenieZakazov)
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
                    ProsmotrClient client1 = new ProsmotrClient(true);
                    client1.Show();
                }
                else if (CurrentUser.Role == "Менеджер")
                {
                    ProsmotrClient client1 = new ProsmotrClient(false);
                    client1.Show();
                }
            }
        }
    }
    
}
