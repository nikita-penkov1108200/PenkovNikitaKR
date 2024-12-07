using Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PenkovNikitaKR
{
    public partial class ProsmotrSotrudnikov : Form
    {
        private System.Data.DataTable dataTable = new System.Data.DataTable();
        private string photoName = "";
        private string fullPath = "";
        private string connect = ConnectionString.connectionString(); // Предполагается, что у вас есть строка подключения
        private string dataPath = ConnectionString.path; // Предполагается, что у вас есть путь к данным

        public ProsmotrSotrudnikov()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual; // Установка позиции формы в ручном режиме
            this.Location = new System.Drawing.Point(0, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadData();
            LoadRoles(); // Загрузка ролей в комбобокс
            LoadGenders(); // Загрузка полов в комбобокс
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.ReadOnly = true; // Блокировка редактирования
                                          // Установка максимальной длины для textBoxSearch
            textBoxSearch.MaxLength = 15; // Максимально 15 символов

            // Привязка обработчика событий для textBoxSearch
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            button2.Visible = false;
            SetDefaultImage(); // Установка изображения-заглушки
        }
        private void SetDefaultImage()
        {
            // Устанавливаем изображение-заглушку
            pictureBox1.Image = Resource1.DefaultImage; // Убедитесь, что у вас есть изображение-заглушка в ресурсах
        }
        private void TextBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, является ли нажатая клавиша не буквенной (не кириллицей) или является ли это специальным символом
            if (!char.IsControl(e.KeyChar) && !IsCyrillic(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод
            }
        }
        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Запрещаем ввод любых символов
            e.Handled = true;
        }

        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadData()
        {
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear(); // Очищаем существующие столбцы
            dataGridView.Columns.Add("idemployee", "ID сотрудника");
            dataGridView.Columns.Add("Surname", "Фамилия");
            dataGridView.Columns.Add("Name", "Имя");
            dataGridView.Columns.Add("MiddleName", "Отчество");
            dataGridView.Columns.Add("Address", "Адрес");
            dataGridView.Columns.Add("Age", "Возраст");
            dataGridView.Columns.Add("PhoneNumber", "Телефон");

            // Изменяем тип столбца Photo на DataGridViewImageColumn
            DataGridViewImageColumn photoColumn = new DataGridViewImageColumn();
            photoColumn.Name = "Photo";
            photoColumn.HeaderText = "Фото";
            dataGridView.Columns.Add(photoColumn);

            dataGridView.Columns.Add("Gender", "Пол");
            dataGridView.Columns.Add("Role", "Роль");
            dataGridView.Columns.Add("login", "Логин");
            dataGridView.Columns.Add("password", "Пароль");
            dataGridView.Columns.Add("role_idrole", "ID роли");
            dataGridView.Columns.Add("gender_idgender", "ID пола");

            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT idemployee, Surname, Name, MiddleName, Address, Age, PhoneNumber, Photo, Gender, Role, login, password, role_idrole, gender_idgender
                                          FROM employee", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable(); // Укажите полное имя пространства имен
                da.Fill(dt);
                dataTable = dt; // Сохраняем данные в dataTable для фильтрации

                // Проверяем, что есть данные
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow row in dt.Rows) // Укажите полное имя пространства имен
                    {
                        int rowIndex = dataGridView.Rows.Add(); // Добавляем строку и получаем индекс
                        DataGridViewRow dataGridViewRow = dataGridView.Rows[rowIndex];

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j != 7) // Пропускаем столбец Photo
                            {
                                dataGridViewRow.Cells[j].Value = row[j].ToString();
                            }
                            else // Обработка изображения
                            {
                                string photoName = row[j].ToString();
                                if (!string.IsNullOrEmpty(photoName))
                                {
                                    try
                                    {
                                        // Предполагается, что путь к изображениям корректен
                                        string imagePath = Path.Combine(dataPath, photoName);
                                        if (File.Exists(imagePath))
                                        {
                                            using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                                            {
                                                using (Image img = Image.FromStream(stream))
                                                {
                                                    // Изменяем размер изображения перед установкой
                                                    Bitmap resizedImg = (Bitmap)ResizeImage(img, dataGridViewRow.Cells[j].Size); // Создаем новое изображение
                                                    dataGridViewRow.Cells[j].Value = resizedImg; // Устанавливаем измененное изображение в ячейку
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dataGridViewRow.Cells[j].Value = null; // Если файл не найден, оставляем пустым
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    dataGridViewRow.Cells[j].Value = null; // Если нет изображения, оставляем пустым
                                }
                            }
                        }
                    }
                }
            }

            // Скрываем ненужные столбцы, если они есть
            if (dataGridView.Columns.Contains("role_idrole"))
                dataGridView.Columns["role_idrole"].Visible = false;
            if (dataGridView.Columns.Contains("gender_idgender"))
                dataGridView.Columns["gender_idgender"].Visible = false;
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

        private Image ResizeImage(Image img, Size size)
        {
            // Создаем новый объект изображения с указанными размерами
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Устанавливаем интерполяцию для более качественного изменения размера
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                // Рисуем изображение на новом объекте с измененными размерами
                g.DrawImage(img, 0, 0, size.Width, size.Height);
            }
            return bmp;
        }

        private void viewTable() // Обновление таблицы
        {
            dataGridView.Rows.Clear();

            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();

                // Обновленный SQL-запрос для получения данных сотрудников
                MySqlCommand cmd = new MySqlCommand(@"SELECT idemployee, Surname, Name, MiddleName, Address, Age, PhoneNumber, Photo, Gender, Role, login, password 
                                              FROM employee WHERE Surname LIKE @search OR Name LIKE @search OR MiddleName LIKE @search", con);

                // Добавляем параметр для предотвращения SQL-инъекций
                cmd.Parameters.AddWithValue("@search", $"%{textBoxSearch.Text}%");

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                System.Data.DataTable dt = new System.Data.DataTable(); // Укажите полное имя пространства имен
                da.Fill(dt);

                // Заполняем DataGridView данными
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView.Rows.Add();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j != 7) // Пропускаем столбец Photo
                        {
                            dataGridView.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString();
                        }
                        else // Обработка изображения
                        {
                            string photo = dt.Rows[i][j].ToString();
                            try
                            {
                                if (string.IsNullOrEmpty(photo))
                                {
                                    photo = "picture.png"; // Путь к изображению по умолчанию
                                }
                                string imagePath = Path.Combine(dataPath, photo);
                                if (File.Exists(imagePath))
                                {
                                    using (Image sketch = new Bitmap(imagePath))
                                    {
                                        Bitmap resizedSketch = (Bitmap)ResizeImage(sketch, dataGridView.Rows[i].Cells[j].Size); // Изменяем размер изображения под ячейку
                                        dataGridView.Rows[i].Cells[j].Value = resizedSketch; // Присваиваем изображение в ячейку
                                    }
                                }
                                else
                                {
                                    dataGridView.Rows[i].Cells[j].Value = null; // Если файл не найден, оставляем пустым
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                                dataGridView.Rows[i].Cells[j].Value = null; // Если произошла ошибка, оставляем пустым
                            }
                        }
                    }
                }
            }
            photoName = "";
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string filter = textBoxSearch.Text.ToLower();

            // Проверяем, является ли текстовое поле пустым
            if (string.IsNullOrEmpty(filter))
            {
                // Если текстовое поле пустое, показываем все строки
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Visible = true; // Показываем строку
                    }
                }
            }
            else
            {
                // Перебираем все строки в DataGridView и скрываем те, которые не соответствуют фильтру
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    // Проверяем, что строка не является новой строкой (для добавления)
                    if (!row.IsNewRow)
                    {
                        // Если фамилия содержит фильтр, показываем строку, иначе скрываем
                        if (row.Cells["Surname"].Value != null && row.Cells["Surname"].Value.ToString().ToLower().Contains(filter))
                        {
                            row.Visible = true; // Показываем строку
                        }
                        else
                        {
                            row.Visible = false; // Скрываем строку
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MenuAdmin main = new MenuAdmin();
            main.ShowDialog();
        }
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            button7.Enabled = dataGridView.SelectedRows.Count > 0;

            if (dataGridView.SelectedRows.Count > 0)
            {
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбран сотрудник
            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Прерываем выполнение, если ничего не выбрано
            }

            // Если строка выбрана, продолжаем выполнение
            this.Width = 1750;
            if (!string.IsNullOrEmpty(textBoxSearch.Text))
            {
                button5.Enabled = true; // Активируем кнопку, если есть текст в поиске
            }
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
        private void button5_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбран сотрудник
            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
                return;
            }

            int rowIndex = dataGridView.CurrentCell.RowIndex;
            string employeeId = dataGridView.Rows[rowIndex].Cells["idemployee"].Value.ToString(); // Получаем ID сотрудника

            // Получаем данные из текстовых полей и комбобоксов
            string surname = textBoxSurname.Text;
            string name = textBoxName.Text;
            string middleName = textBoxMiddleName.Text;
            string address = textBoxAddress.Text;
            int age = Convert.ToInt32(textBoxAge.Text);
            string phoneNumber = textBoxPhoneNumber.Text;
            string gender = comboBoxGender.SelectedItem?.ToString();
            string role = comboBoxRole.SelectedItem?.ToString();
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;

            string photo = photoName; // Имя файла фотографии

            // Если новое фото не выбрано, получаем текущее имя фото из базы данных
            if (string.IsNullOrEmpty(photo))
            {
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    MySqlCommand cmd = new MySqlCommand($"SELECT Photo FROM employee WHERE idemployee = @idemployee", con);
                    cmd.Parameters.AddWithValue("@idemployee", employeeId);
                    con.Open();
                    photo = Convert.ToString(cmd.ExecuteScalar());
                }
            }

            // Хешируем пароль только при редактировании
            string hashedPassword = HashPassword(password);

            // Обновляем данные сотрудника в базе данных
            string sqlQuery = $@"UPDATE employee 
                     SET Surname = @Surname, Name = @Name, MiddleName = @MiddleName, 
                         Address = @Address, Age = @Age, PhoneNumber = @PhoneNumber, 
                         Photo = @Photo, Gender = @Gender, Role = @Role, 
                         login = @Login, password = @Password 
                     WHERE idemployee = @idemployee";

            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@Surname", surname);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@MiddleName", middleName);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                cmd.Parameters.AddWithValue("@Photo", photo);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", hashedPassword); // Хешируем пароль перед сохранением
                cmd.Parameters.AddWithValue("@idemployee", employeeId);

                int res = cmd.ExecuteNonQuery();

                if (res == 1)
                {
                    MessageBox.Show("Данные обновлены");
                    LoadData(); // Перезагружаем данные в DataGridView
                }
                else
                {
                    MessageBox.Show("Данные не обновлены");
                }
            }

            // Очистка полей после обновления
            ClearFields();
            viewTable();
            this.Width = 1100;
        }

        private void ClearFields()
        {
            textBoxSurname.Text = string.Empty;
            textBoxName.Text = string.Empty;
            textBoxMiddleName.Text = string.Empty;
            textBoxAddress.Text = string.Empty;
            textBoxAge.Text = string.Empty;
            textBoxPhoneNumber.Text = string.Empty;
            comboBoxGender.SelectedItem = null;
            comboBoxRole.SelectedItem = null;
            textBoxLogin.Text = string.Empty;
            textBoxPassword.Text = string.Empty;
            pictureBox1.Image = null; // Очищаем изображение
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Width = 1100;
        }
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Получаем имя столбца
                string columnName = dataGridView.Columns[e.ColumnIndex].Name;

                // Получаем значение ячейки
                var cellValue = dataGridView.Rows[e.RowIndex].Cells[columnName].Value;

                // Проверяем, что значение не null
                if (cellValue != null)
                {
                    switch (columnName)
                    {
                        case "Name":
                            var nameValue = cellValue.ToString();
                            if (!string.IsNullOrEmpty(nameValue))
                            {
                                e.Value = nameValue.Substring(0, 1) + new string('*', nameValue.Length - 1);
                            }
                            break;

                        case "MiddleName":
                            var middleNameValue = cellValue.ToString();
                            if (!string.IsNullOrEmpty(middleNameValue))
                            {
                                e.Value = middleNameValue.Substring(0, 1) + new string('*', middleNameValue.Length - 1);
                            }
                            break;

                        case "Address":
                            var addressValue = cellValue.ToString();
                            if (!string.IsNullOrEmpty(addressValue))
                            {
                                var firstWord = addressValue.Split(' ')[0]; // Получаем первое слово
                                e.Value = firstWord; // Устанавливаем только первое слово
                            }
                            break;

                        case "PhoneNumber":
                            var phoneNumberValue = cellValue.ToString();
                            if (!string.IsNullOrEmpty(phoneNumberValue) && phoneNumberValue.Length > 2)
                            {
                                e.Value = new string('*', phoneNumberValue.Length - 2) + phoneNumberValue.Substring(phoneNumberValue.Length - 2);
                            }
                            break;

                        case "login":
                            e.Value = new string('*', 7); // или e.Value = "******"; для фиксированной длины
                            break;

                        case "password":
                            e.Value = new string('*', 7); // или e.Value = "******"; для фиксированной длины
                            break;

                        case "Surname":
                            // Не скрываем, оставляем как есть
                            break;

                        default:
                            // Установка цвета фона для других столбцов, если нужно
                            break;
                    }

                    // Указываем, что форматирование было выполнено
                    e.FormattingApplied = true;
                }
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0) // Проверяем, что строка выбрана
            {
                DataGridViewRow row = dataGridView.Rows[rowIndex];
                string employeeId = row.Cells["idemployee"].Value.ToString(); // Получаем ID сотрудника

                // SQL-запрос для получения данных сотрудника
                string sqlQuery = "SELECT Surname, Name, MiddleName, Address, Age, PhoneNumber, Photo, Gender, Role, login, password " +
                                  "FROM employee WHERE idemployee = @idemployee;";

                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                    cmd.Parameters.AddWithValue("@idemployee", employeeId); // Используем параметр для предотвращения SQL-инъекций
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        textBoxSurname.Text = rdr["Surname"].ToString();
                        textBoxName.Text = rdr["Name"].ToString();
                        textBoxMiddleName.Text = rdr["MiddleName"].ToString();
                        textBoxAddress.Text = rdr["Address"].ToString();
                        textBoxAge.Text = rdr["Age"].ToString();
                        textBoxPhoneNumber.Text = rdr["PhoneNumber"].ToString(); // Обновляем поле PhoneNumber

                        string photo = rdr["Photo"].ToString();
                        // Загрузка фотографии в pictureBox
                        if (!string.IsNullOrEmpty(photo))
                        {
                            string imagePath = Path.Combine(dataPath, photo);
                            if (File.Exists(imagePath))
                            {
                                pictureBox1.Image = Image.FromFile(imagePath);
                                pictureBox1.Image = ResizeImage(pictureBox1.Image, pictureBox1.Size); // Уменьшаем изображение под границы PictureBox
                            }
                            else
                            {
                                pictureBox1.Image = null; // Если файл не найден, оставляем пустым
                            }
                        }

                        // Установка значений в комбобоксах
                        comboBoxGender.SelectedItem = rdr["Gender"].ToString();
                        comboBoxRole.SelectedItem = rdr["Role"].ToString();

                        // Установка логина и пароля, если нужно
                        textBoxLogin.Text = rdr["login"].ToString();
                        textBoxPassword.Text = rdr["password"].ToString();
                    }
                }
            }
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
                        fileInfo.Length <= 2 * 1024 * 1024)
                    {
                        using (Image image = Image.FromFile(openFileDialog.FileName))
                        {
                            pictureBox1.Image = ResizeImage(image, pictureBox1.Size); // Уменьшаем изображение под границы PictureBox
                        }
                        photoName = fileInfo.Name;
                        fullPath = openFileDialog.FileName;
                    }
                    else
                    {
                        MessageBox.Show("Выберите файл JPG или PNG размером не более 2 Мб.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DobavlenieSotrydnikov main = new DobavlenieSotrydnikov();
            main.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбран сотрудник
            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления.");
                return;
            }

            // Получаем ID выбранного сотрудника
            int rowIndex = dataGridView.CurrentCell.RowIndex;
            string employeeId = dataGridView.Rows[rowIndex].Cells["idemployee"].Value.ToString();

            // Подтверждение удаления
            var confirmationResult = MessageBox.Show("Вы действительно хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmationResult == DialogResult.Yes)
            {
                using (MySqlConnection con = new MySqlConnection(connect))
                {
                    con.Open();
                    // SQL-запрос для удаления записи
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM employee WHERE idemployee = @idemployee", con);
                    cmd.Parameters.AddWithValue("@idemployee", employeeId);

                    int res = cmd.ExecuteNonQuery();
                    if (res == 1)
                    {
                        MessageBox.Show("Запись успешно удалена.");
                        LoadData(); // Перезагружаем данные в DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении записи.");
                    }
                }
            }
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


        private void button7_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбран сотрудник
            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Выберите сотрудника для просмотра.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Прерываем выполнение, если ничего не выбрано
            }

            int rowIndex = dataGridView.CurrentCell.RowIndex;
            string employeeId = dataGridView.Rows[rowIndex].Cells["idemployee"].Value.ToString(); // Получаем ID сотрудника

            // SQL-запрос для получения данных выбранного сотрудника
            string sqlQuery = "SELECT Surname, Name, MiddleName, Address, Age, PhoneNumber, Photo, Gender, Role, login, password " +
                              "FROM employee WHERE idemployee = @idemployee;";

            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@idemployee", employeeId); // Используем параметр для предотвращения SQL-инъекций
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    // Получаем данные
                    string surname = rdr["Surname"].ToString();
                    string name = rdr["Name"].ToString();
                    string middleName = rdr["MiddleName"].ToString();
                    string address = rdr["Address"].ToString();
                    int age = Convert.ToInt32(rdr["Age"]);
                    string phoneNumber = rdr["PhoneNumber"].ToString();
                    string login = rdr["login"].ToString();
                    string photo = rdr["Photo"].ToString();
                    string password = rdr["Password"].ToString();
                    string gender = rdr["Gender"].ToString();
                    string role = rdr["Role"].ToString();

                    // Формируем полный путь к фотографии
                    string photoPath = Path.Combine(dataPath, photo);

                    // Создаем экземпляр формы ProsmotrSotrPoln
                    ProsmotrSotrPoln detailsForm = new ProsmotrSotrPoln();
                    detailsForm.DisplayData(surname, name, middleName, address, age, phoneNumber, login, photoPath, password, gender, role);
                    detailsForm.ShowDialog(); // Показываем форму
                }
            }
        }
    }
}