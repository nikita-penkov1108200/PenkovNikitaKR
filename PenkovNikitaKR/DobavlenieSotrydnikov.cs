using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace PenkovNikitaKR
{
    public partial class DobavlenieSotrydnikov : Form
    {
            private string connect = ConnectionString.connectionString(); // Строка подключения
            private string dataPath = ConnectionString.path; // Путь к данным
            private string photoName = "";

            public DobavlenieSotrydnikov()
            {
                InitializeComponent();
                this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
                this.Resize += new EventHandler(ProsmotrYslyk_Resize);
                LoadRoles(); // Загрузка ролей
                LoadGenders(); // Загрузка полов
                SetDefaultImage(); // Установка изображения-заглушки
            }

            private void ProsmotrYslyk_Resize(object sender, EventArgs e)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2")); // Преобразуем байты в шестнадцатеричный формат
                }
                return builder.ToString(); // Возвращаем хешированный пароль
            }
        }

        private void LoadRoles()
            {
                comboBoxRole.Items.Clear();
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT RoleName FROM role", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBoxRole.Items.Add(reader["RoleName"].ToString());
                    }
                }
            }

            private void LoadGenders()
            {
                comboBoxGender.Items.Clear();
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT NameGender FROM gender", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBoxGender.Items.Add(reader["NameGender"].ToString());
                    }
                }
            }

            private void SetDefaultImage()
            {
                // Устанавливаем изображение-заглушку
                pictureBox1.Image = Resource1.DefaultImage; // Убедитесь, что у вас есть изображение-заглушка в ресурсах
            }

            private void pictureBox1_Click(object sender, EventArgs e)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";
                    openFileDialog.Title = "Выберите изображение";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                        if ((fileInfo.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                             fileInfo.Extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                             fileInfo.Extension.Equals(".png", StringComparison.OrdinalIgnoreCase)) &&
                            fileInfo.Length <= 2 * 1024 * 1024) // Проверка размера файла
                        {
                            // Используем using для освобождения ресурсов
                            using (Image img = Image.FromFile(openFileDialog.FileName))
                            {
                                pictureBox1.Image = ResizeImage(img, pictureBox1.Size); // Уменьшаем изображение под размеры PictureBox
                            }
                            photoName = openFileDialog.FileName; // Сохраняем полный путь к файлу
                        }
                        else
                        {
                            MessageBox.Show("Выберите файл JPG или PNG размером не более 2 Мб.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            private Image ResizeImage(Image img, Size size)
            {
                Bitmap bmp = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, 0, 0, size.Width, size.Height);
                }
                return bmp;
            }

        // Обработчик нажатия кнопки для добавления сотрудника
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем, что все поля заполнены
            if (string.IsNullOrEmpty(textBoxSurname.Text) || string.IsNullOrEmpty(textBoxName.Text) ||
                string.IsNullOrEmpty(textBoxMiddleName.Text) || string.IsNullOrEmpty(textBoxAddress.Text) ||
                string.IsNullOrEmpty(textBoxAge.Text) || string.IsNullOrEmpty(textBoxPhoneNumber.Text) ||
                string.IsNullOrEmpty(textBoxLogin.Text) || string.IsNullOrEmpty(textBoxPassword.Text) ||
                comboBoxGender.SelectedItem == null || comboBoxRole.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на уникальность логина
            if (IsLoginDuplicate(textBoxLogin.Text))
            {
                MessageBox.Show("Логин уже существует. Пожалуйста, выберите другой логин.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Если логин дублируется, выходим из метода
            }

            // Если фотография не добавлена, спрашиваем пользователя
            if (string.IsNullOrEmpty(photoName))
            {
                DialogResult result = MessageBox.Show("Вы не добавили фотографию. Продолжить без фотографии?", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return; // Если пользователь выбрал "Нет", выходим
                }
            }

            // Получаем данные из текстовых полей и комбобоксов
            string surname = textBoxSurname.Text;
            string name = textBoxName.Text;
            string middleName = textBoxMiddleName.Text;
            string address = textBoxAddress.Text;
            int age = Convert.ToInt32(textBoxAge.Text);
            string phoneNumber = textBoxPhoneNumber.Text;
            string gender = comboBoxGender.SelectedItem.ToString();
            string role = comboBoxRole.SelectedItem.ToString();
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            // Хешируем пароль
            string hashedPassword = HashPassword(password);

            // Генерация ID сотрудника (например, максимальный ID + 1)
            int employeeId;
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT IFNULL(MAX(idemployee), 0) + 1 FROM employee", con);
                employeeId = Convert.ToInt32(cmd.ExecuteScalar()); // Автоинкремент ID
            }

            // Получаем ID роли
            int roleId;
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT idrole FROM role WHERE RoleName = @RoleName", con);
                cmd.Parameters.AddWithValue("@RoleName", role);
                roleId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // Получаем ID пола
            int genderId;
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT idgender FROM gender WHERE NameGender = @GenderName", con);
                cmd.Parameters.AddWithValue("@GenderName", gender);
                genderId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // Сохранение файла фотографии
            if (!string.IsNullOrEmpty(photoName))
            {
                string destinationPath = Path.Combine(dataPath, Path.GetFileName(photoName)); // Путь для сохранения файла
                try
                {
                    File.Copy(photoName, destinationPath, true); // Копируем файл в папку данных
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // SQL-запрос для добавления нового сотрудника
            string sqlQuery = @"INSERT INTO employee (idemployee, Surname, Name, MiddleName, Address, Age, PhoneNumber, Photo, Gender_idgender, Gender, Role, Role_idrole, login, password) 
    VALUES (@idemployee, @Surname, @Name, @MiddleName, @Address, @Age, @PhoneNumber, @Photo, @Gender_idgender, @Gender, @Role, @Role_idrole, @Login, @Password)";

            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@idemployee", employeeId);
                cmd.Parameters.AddWithValue("@Surname", surname);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@MiddleName", middleName);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@Photo", Path.GetFileName(photoName)); // Сохраняем только имя файла
                cmd.Parameters.AddWithValue("@Gender_idgender", genderId); // ID пола
                cmd.Parameters.AddWithValue("@Role_idrole", roleId); // ID роли
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", hashedPassword); // Сохраняем хешированный пароль

                int res = cmd.ExecuteNonQuery();

                if (res == 1)
                {
                    MessageBox.Show("Данные успешно добавлены.");
                    ClearFields(); // Очищаем поля после добавления
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении данных.");
                }
            }
        }

        // Метод для проверки дублирования логина
        private bool IsLoginDuplicate(string login)
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM employee WHERE login = @Login", con);
                cmd.Parameters.AddWithValue("@Login", login);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0; // Если счетчик больше 0, логин дублируется
            }
        }

            private void ClearFields()
            {
                textBoxSurname.Text = string.Empty;
                textBoxName.Text = string.Empty;
                textBoxMiddleName.Text = string.Empty;
                textBoxAddress.Text = string.Empty;
                textBoxAge.Text = string.Empty;
                textBoxPhoneNumber.Text = string.Empty;
                textBoxLogin.Text = string.Empty;
                textBoxPassword.Text = string.Empty;
                comboBoxGender.SelectedItem = null;
                comboBoxRole.SelectedItem = null;
                SetDefaultImage(); // Устанавливаем изображение-заглушку
                photoName = ""; // Сбрасываем имя файла
            }

            private void button2_Click(object sender, EventArgs e)
            {
                this.Visible = false;
                ProsmotrSotrudnikov main = new ProsmotrSotrudnikov();
                main.ShowDialog();
            }

            // Ограничение на ввод в текстовые поля
            private void textBoxAge_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
                if (textBoxAge.Text.Length >= 2 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
                }
            }

            private void textBoxPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
                if (textBoxPhoneNumber.Text.Length >= 11 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
                }
            }

        private void textBoxLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, не превышает ли длина текста 7 символов
            if (textBoxLogin.Text.Length >= 7 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }

            // Проверяем, является ли символ английской буквой или Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если символ не является буквой или Backspace
            }
        }

            private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (textBoxPassword.Text.Length >= 7 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
                }
            }

            private void textBoxSurname_Leave(object sender, EventArgs e)
            {
                // Первая буква с заглавной
                if (!string.IsNullOrEmpty(textBoxSurname.Text))
                    textBoxSurname.Text = char.ToUpper(textBoxSurname.Text[0]) + textBoxSurname.Text.Substring(1);
            }

            private void textBoxName_Leave(object sender, EventArgs e)
            {
                // Первая буква с заглавной
                if (!string.IsNullOrEmpty(textBoxName.Text))
                    textBoxName.Text = char.ToUpper(textBoxName.Text[0]) + textBoxName.Text.Substring(1);
            }

            private void textBoxMiddleName_Leave(object sender, EventArgs e)
            {
                // Первая буква с заглавной
                if (!string.IsNullOrEmpty(textBoxMiddleName.Text))
                    textBoxMiddleName.Text = char.ToUpper(textBoxMiddleName.Text[0]) + textBoxMiddleName.Text.Substring(1);
            }

            private void textBoxSurname_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxSurname.Text.Length > 20)
                    textBoxSurname.Text = textBoxSurname.Text.Substring(0, 20);
            }

            private void textBoxName_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxName.Text.Length > 20)
                    textBoxName.Text = textBoxName.Text.Substring(0, 20);
            }

            private void textBoxMiddleName_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxMiddleName.Text.Length > 20)
                    textBoxMiddleName.Text = textBoxMiddleName.Text.Substring(0, 20);
            }

            private void textBoxAddress_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxAddress.Text.Length > 25)
                    textBoxAddress.Text = textBoxAddress.Text.Substring(0, 25);
            }

            private void textBoxAge_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxAge.Text.Length > 2)
                    textBoxAge.Text = textBoxAge.Text.Substring(0, 2);
            }

            private void textBoxPhoneNumber_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxPhoneNumber.Text.Length > 11)
                    textBoxPhoneNumber.Text = textBoxPhoneNumber.Text.Substring(0, 11);
            }

            private void textBoxLogin_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxLogin.Text.Length > 7)
                    textBoxLogin.Text = textBoxLogin.Text.Substring(0, 7);
            }

            private void textBoxPassword_TextChanged(object sender, EventArgs e)
            {
                // Ограничение на количество символов
                if (textBoxPassword.Text.Length > 7)
                    textBoxPassword.Text = textBoxPassword.Text.Substring(0, 7);
            }

            // Метод для ограничения ввода в текстовые поля
            private void textBoxSurname_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
                {
                    e.Handled = true;
                }
                if (textBoxSurname.Text.Length >= 20 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
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

            private void textBoxMiddleName_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
                {
                    e.Handled = true;
                }
                if (textBoxMiddleName.Text.Length >= 20 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
                }
            }

            // Метод для проверки, является ли символ кириллическим
            private bool IsCyrillic(char c)
            {
                return (c >= '\u0400' && c <= '\u04FF'); // Диапазон кириллических символов
            }

            private void textBoxAddress_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    e.KeyChar != ' ' && e.KeyChar != '.' && e.KeyChar != ',' && e.KeyChar != '-' && e.KeyChar != '/')
                {
                    e.Handled = true; // Запрещаем ввод
                }
                if (textBoxAddress.Text.Length >= 25 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
                }
            }
        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрещаем ввод любых символов
            e.Handled = true;
        }

    }
}
