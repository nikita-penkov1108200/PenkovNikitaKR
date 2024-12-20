﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace PenkovNikitaKR
{
    public partial class Avtorizatia : Form
    {
        public int count = 0;
        private string text = String.Empty;
        private bool isBlocked = false; // Переменная для отслеживания блокировки
        private Timer unblockTimer; // Таймер для разблокировки
        public Avtorizatia()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            qpass.UseSystemPasswordChar = true;
            Image image = Resource1.view;
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);

            // Инициализация таймера
            unblockTimer = new Timer();
            unblockTimer.Interval = 10000; // 10 секунд
            unblockTimer.Tick += UnblockTimer_Tick;
        }

        private void UnblockTimer_Tick(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            button1.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button2.Enabled = true;
            pictureBox1.Enabled = true;
            isBlocked = false; // Разблокируем
            unblockTimer.Stop(); // Останавливаем таймер
            MessageBox.Show("Вы можете снова ввести логин, пароль и капчу.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button1_Click(object sender, EventArgs e)
        {
           

            // Проверяем, включены ли текстовые поля для логина и пароля
            if (!qlogin.Enabled || !qpass.Enabled)
            {
                MessageBox.Show("Сначала пройдите капчу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string login = qlogin.Text;
            string passwd = qpass.Text;

            // Получаем логин и пароль из App.config
            string adminUsername = ConfigurationManager.AppSettings["AdminUsername"];
            string adminPassword = ConfigurationManager.AppSettings["AdminPassword"];

            // Хешируем введенный пароль
            string hashedPassword = HashPassword(passwd);

            // Проверка на логин и пароль из App.config
            if (login == adminUsername && hashedPassword == HashPassword(adminPassword))
            {
                // Если логин и пароль совпадают, переходим на форму Import
                Import importForm = new Import();
                importForm.Show();
                this.Hide();
                return;
            }

            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT idemployee, Surname, Name, MiddleName, login, password, Role FROM employee WHERE login = @login and password = @password;", con);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", hashedPassword); // Используем хешированный пароль

                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int UserID = reader.GetInt32("idemployee");
                    string Login = reader.GetString("login");
                    string UsPassword = reader.GetString("password");
                    string UsSername = reader.GetString("Surname");
                    string UsName = reader.GetString("Name");
                    string UsPatronymic = reader.GetString("MiddleName");
                    string UsRole = reader.GetString("Role");
                    CurrentUser.Id = UserID;
                    CurrentUser.Name = UsName;
                    CurrentUser.Surname = UsSername;
                    CurrentUser.Patronymic = UsPatronymic;
                    CurrentUser.Role = UsRole;

                    if (UsRole == "Администратор")
                    {
                        MessageBox.Show("Успешный вход");
                        MenuAdmin MA = new MenuAdmin();
                        MA.Show();
                        this.Hide();
                    }
                    else if (UsRole == "Менеджер")
                    {
                        MessageBox.Show("Успешный вход");
                        MenuPolzovatel MA = new MenuPolzovatel();
                        MA.Show();
                        this.Hide();
                    }
                    else if (UsRole == "Техник")
                    {
                        MessageBox.Show("Успешный вход");
                        HistoryOrder MA = new HistoryOrder();
                        MA.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Введен не правильный логин или пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    qlogin.Text = "";
                    qpass.Text = "";
                    this.Width = 700;

                    // Блокируем возможность ввода
                    qlogin.Enabled = false; // Отключаем текстовое поле для логина
                    qpass.Enabled = false; // Отключаем текстовое поле для пароля
                }
                reader.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Выводим сообщение с подтверждением выхода
            var result = MessageBox.Show("Вы действительно хотите выйти из приложения?",
                                           "Подтверждение выхода",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

            // Если пользователь нажал "Да", выходим из приложения
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Переключаем видимость пароля
            if (qpass.UseSystemPasswordChar)
            {
                qpass.UseSystemPasswordChar = false; // Показываем пароль
                pictureBox1.Image = Resource1.icons; // Изображение открытого глаза
            }
            else
            {
                qpass.UseSystemPasswordChar = true; // Скрываем пароль
                pictureBox1.Image = Resource1.view; // Изображение закрытого глаза
            }
        }

        private void textBoxlogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, не превышает ли длина текста 10 символов
            if (qlogin.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }
        }

        private void textBoxpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Проверяем, не превышает ли длина текста 10 символов
            if (qpass.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено максимальное количество символов
            }
        }
        public Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            Bitmap result = new Bitmap(Width, Height);

            
            int Xpos = Width > 100 ? rnd.Next(0, Width - 100) : 0; // Устанавливаем Xpos в 0, если Width < 100
            int Ypos = Height > 50 ? rnd.Next(15, Height - 50) : 15; // Устанавливаем Ypos в 15, если Height < 50

            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.DarkRed,
                     Brushes.Green };

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.Gray);

            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Arial", 28),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));

            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }
  

   

        private void Avtorizatia_Load(object sender, EventArgs e)
        {
            pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == this.text)
            {
                MessageBox.Show("Верно!");
                Class1.k = 4;
                this.Width = 360;

                // Разрешаем ввод логина и пароля
                qlogin.Enabled = true; // Включаем текстовое поле для логина
                qpass.Enabled = true; // Включаем текстовое поле для пароля
                textBox1.Text = "";
                pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
            }
            else
            {
                MessageBox.Show("Ошибка!");
                textBox1.Text = "";              
                MessageBox.Show("Вход заблокирован. Пожалуйста, подождите 10 секунд.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Enabled = false;
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button2.Enabled = false;
                pictureBox1.Enabled = false;
                isBlocked = true;
                unblockTimer.Start(); // Запускаем таймер
            }
        }
    }
    internal class Class1
    {
        public static int k = 0;
        public static string l = "";
    }
}
