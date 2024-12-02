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
using System.Xml.Linq;

namespace PenkovNikitaKR
{
    public partial class RedactirovanieComponentDevice : Form
    {
        public event Action<string, int> DataUpdated;
        private string _originalnameComponentDevice;
        private int _originalcostComponentDevice;
        public RedactirovanieComponentDevice(string nameComponentDevice, int costComponentDevice)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            _originalnameComponentDevice = nameComponentDevice;
            _originalcostComponentDevice = costComponentDevice;

            textBox4.Text = nameComponentDevice;
            textBox2.Text = costComponentDevice.ToString();
            textBox4.KeyPress += TextBoxNameComponentDevice_KeyPress;
            textBox2.KeyPress += TextBoxCostComponentDevice_KeyPress;
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void TextBoxNameComponentDevice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрет ввода знаков
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод
            }
        }
        private void TextBoxCostComponentDevice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Ограничение на ввод только цифр и BackSpace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Отменяем ввод
            }
        }
        private void TextBoxCostComponentDevice_TextChanged(object sender, EventArgs e)
        {
            // Ограничение на 6 символов
            if (textBox2.Text.Length > 6)
            {
                textBox2.Text = textBox2.Text.Substring(0, 6);
                textBox2.SelectionStart = textBox2.Text.Length; // Установка курсора в конец
            }
        }

        private void TextBoxNameComponentDevice_TextChanged(object sender, EventArgs e)
        {
            // Ограничение на 20 символов
            if (textBox4.Text.Length > 20)
            {
                textBox4.Text = textBox4.Text.Substring(0, 20);
                textBox4.SelectionStart = textBox4.Text.Length; // Установка курсора в конец
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
            // Получаем обновленные значения из текстовых полей и ComboBox
            string updatednameComponentDevice = textBox4.Text;

            int updatedcostComponentDevice;
            if (!int.TryParse(textBox2.Text, out updatedcostComponentDevice))
            {
                MessageBox.Show("Введите корректную стоимость компонента.");
                return; // Прерываем выполнение, если ввод некорректен
            }

            // Обновление данных в базе данных
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "UPDATE componentdevice SET NameComponentDevice = @NameComponentDevice, CostComponentDevice = @CostComponentDevice WHERE NameComponentDevice = @OriginalnameComponentDevice AND CostComponentDevice = @OriginalcostComponentDevice";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NameComponentDevice", updatednameComponentDevice);
                cmd.Parameters.AddWithValue("@CostComponentDevice", updatedcostComponentDevice);

                cmd.Parameters.AddWithValue("@OriginalnameComponentDevice", _originalnameComponentDevice);
                cmd.Parameters.AddWithValue("@OriginalcostComponentDevice", _originalcostComponentDevice);

                cmd.ExecuteNonQuery();
            }

            // Вызываем событие для передачи обновленных данных
            DataUpdated?.Invoke(updatednameComponentDevice, updatedcostComponentDevice);
            MessageBox.Show("Информация успешно обновлена.");
            this.Hide();
            if (CurrentUser.Role == "Администратор")
            {
                ProsmotrComponentDevice adminMenu = new ProsmotrComponentDevice(true);
                adminMenu.Show();
            }
            else if (CurrentUser.Role == "Менеджер")
            {
                ProsmotrComponentDevice userMenu = new ProsmotrComponentDevice(false);
                userMenu.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (CurrentUser.Role == "Администратор")
            {
                ProsmotrComponentDevice adminMenu = new ProsmotrComponentDevice(true);
                adminMenu.Show();
            }
            else if (CurrentUser.Role == "Менеджер")
            {
                ProsmotrComponentDevice userMenu = new ProsmotrComponentDevice(false);
                userMenu.Show();
            }
        }

        private void RedactirovanieComponentDevice_Load(object sender, EventArgs e)
        {

        }
    }
}
