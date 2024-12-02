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
    public partial class RedactirovanieClient : Form
    {
        public event Action<string, string, string, long, string> DataUpdated;
        private string _originalName;
        private string _originalSurname;
        private string _originalMiddleName;
        private long _originalPhoneNumber; 
        private string _originalStatusVIP;

        public RedactirovanieClient(string name, string surname, string middleName, long phoneNumber, string statusVIP)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            _originalName = name;
            _originalSurname = surname;
            _originalMiddleName = middleName;
            _originalPhoneNumber = phoneNumber;
            _originalStatusVIP = statusVIP;

            textBoxName.Text = name;
            textBoxSurname.Text = surname;
            textBoxMiddleName.Text = middleName;
            textBoxPhoneNumber.Text = phoneNumber.ToString();

            comboBoxStatusVIP.Items.Add("Постоянный");
            comboBoxStatusVIP.Items.Add("Элитный");
            comboBoxStatusVIP.Items.Add("Императорский");
            comboBoxStatusVIP.SelectedItem = statusVIP;

            // Подписка на события для текстовых полей
            textBoxName.KeyPress += TextBox_KeyPress;
            textBoxSurname.KeyPress += TextBox_KeyPress;
            textBoxMiddleName.KeyPress += TextBox_KeyPress;
            textBoxPhoneNumber.KeyPress += TextBoxPhoneNumber_KeyPress;

            // Подписка на события для изменения текста
            textBoxName.TextChanged += TextBox_TextChanged;
            textBoxSurname.TextChanged += TextBox_TextChanged;
            textBoxMiddleName.TextChanged += TextBox_TextChanged;

            // Запрет ввода в ComboBox
            comboBoxStatusVIP.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void TextBoxPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрет ввода символов, кроме цифр и Backspace
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрет ввода
            }

            // Проверка, если длина текста уже равна 11, запрещаем ввод новых символов
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 11 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрет ввода
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрет ввода символов, кроме русских букв, пробелов и Backspace
            if (!char.IsControl(e.KeyChar) &&
                !IsRussianLetter(e.KeyChar) &&
                e.KeyChar != ' ')
            {
                e.Handled = true; // Запрет ввода
            }
        }

        private bool IsRussianLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Ограничение на количество символов
            if (textBox == textBoxName || textBox == textBoxSurname || textBox == textBoxMiddleName)
            {
                if (textBox.Text.Length >= 20)
                {
                    textBox.Text = textBox.Text.Substring(0, 20); // Ограничение до 20 символов
                    textBox.SelectionStart = textBox.Text.Length; // Установка курсора в конец
                }
            }
            else if (textBox == textBoxPhoneNumber)
            {
                if (textBox.Text.Length >= 11)
                {
                    textBox.Text = textBox.Text.Substring(0, 11); // Ограничение до 11 символов
                    textBox.SelectionStart = textBox.Text.Length; // Установка курсора в конец
                }
            }

            // Автоматическая корректировка первой буквы на заглавную
            if (textBox.Text.Length > 0)
            {
                textBox.Text = char.ToUpper(textBox.Text[0]) + textBox.Text.Substring(1);
                textBox.SelectionStart = textBox.Text.Length; // Установка курсора в конец
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем обновленные значения из текстовых полей и ComboBox
            string updatedName = textBoxName.Text;
            string updatedSurname = textBoxSurname.Text;
            string updatedMiddleName = textBoxMiddleName.Text;

            // Изменяем тип на long для хранения больших чисел
            long updatedPhoneNumber;
            if (!long.TryParse(textBoxPhoneNumber.Text, out updatedPhoneNumber))
            {
                MessageBox.Show("Введите корректный номер телефона.");
                return; // Прерываем выполнение, если ввод некорректен
            }

            string updatedStatusVIP = comboBoxStatusVIP.SelectedItem.ToString();

            // Обновление данных в базе данных
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "UPDATE client SET Name = @Name, Surname = @Surname, MiddleName = @MiddleName, PhoneNumber = @PhoneNumber, StatusVIP = @StatusVIP WHERE Name = @OriginalName AND Surname = @OriginalSurname AND MiddleName = @OriginalMiddleName AND PhoneNumber = @OriginalPhoneNumber";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", updatedName);
                cmd.Parameters.AddWithValue("@Surname", updatedSurname);
                cmd.Parameters.AddWithValue("@MiddleName", updatedMiddleName);
                cmd.Parameters.AddWithValue("@PhoneNumber", updatedPhoneNumber);
                cmd.Parameters.AddWithValue("@StatusVIP", updatedStatusVIP);
                cmd.Parameters.AddWithValue("@OriginalName", _originalName);
                cmd.Parameters.AddWithValue("@OriginalSurname", _originalSurname);
                cmd.Parameters.AddWithValue("@OriginalMiddleName", _originalMiddleName);
                cmd.Parameters.AddWithValue("@OriginalPhoneNumber", _originalPhoneNumber);

                cmd.ExecuteNonQuery();
            }

            // Вызываем событие для передачи обновленных данных
            DataUpdated?.Invoke(updatedName, updatedSurname, updatedMiddleName, updatedPhoneNumber, updatedStatusVIP); 
            MessageBox.Show("Информация успешно обновлена.");
            this.Hide();
            if (CurrentUser.Role == "Администратор")
            {
                ProsmotrClient adminMenu = new ProsmotrClient(true);
                adminMenu.Show();
            }
            else if (CurrentUser.Role == "Менеджер")
            {
                ProsmotrClient userMenu = new ProsmotrClient(false);
                userMenu.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (CurrentUser.Role == "Администратор")
            {
                ProsmotrClient adminMenu = new ProsmotrClient(true);
                adminMenu.Show();
            }
            else if (CurrentUser.Role == "Менеджер")
            {
                ProsmotrClient userMenu = new ProsmotrClient(false);
                userMenu.Show();
            }
        }

       
    }
}
