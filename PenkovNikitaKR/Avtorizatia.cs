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

namespace PenkovNikitaKR
{
    public partial class Avtorizatia : Form
    {
        public int count = 0;
        public Avtorizatia()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            qpass.UseSystemPasswordChar = true;
            Image image = Resource1.view;
            pictureBox1.Image = image;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
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
            string login = qlogin.Text;
            string passwd = qpass.Text;

            // Хешируем введенный пароль
            string hashedPassword = HashPassword(passwd);

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
    }
}