using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Timers;

namespace PenkovNikitaKR
{
    public partial class IdlleManager : Form
    {
        private System.Timers.Timer idleTimer;
        private int idleTimeout;

        public IdlleManager()
        {
            InitializeComponent();
            InitializeIdleTimer();
        }

        private void InitializeIdleTimer()
        {
            // Получаем значение времени бездействия из конфигурации
            if (int.TryParse(ConfigurationManager.AppSettings["IdleTimeout"], out idleTimeout))
            {
                idleTimeout *= 1000; // Переводим в миллисекунды
            }
            else
            {
                idleTimeout = 30000; // По умолчанию 30 секунд
            }

            idleTimer = new System.Timers.Timer(idleTimeout);
            idleTimer.Elapsed += OnIdleTimeout;
            idleTimer.AutoReset = false; // Чтобы таймер не перезапускался автоматически
            idleTimer.Start();
        }

        public void UserActivityDetected()
        {
            // Сбрасываем таймер при активности пользователя
            idleTimer.Stop();
            idleTimer.Start();
        }

        private void OnIdleTimeout(object sender, ElapsedEventArgs e)
        {
            var loginForm = Application.OpenForms.OfType<Avtorizatia>().FirstOrDefault();
            loginForm = new Avtorizatia();
            loginForm.ShowDialog(); // Открываем форму авторизации
            // Блокируем систему и открываем форму авторизации
           
                // Сообщение о том, что пользователь не взаимодействовал
                MessageBox.Show("Вы не взаимодействовали с приложением более 30 секунд. Переход на форму авторизации.", "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Warning);        
            //
        }
    }
}