using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace PenkovNikitaKR
{
    public partial class SozdanieCheka : Form
    {
        //private readonly string FileName = @".\word\cheque.docx";
        private readonly string FileName = @"C:\Users\student251\Desktop\textbox\textbox\textbox\sdelano\sdelano\Kyrsovoi (2)\Kyrsovoi (2)\Kyrsovoi\проект\dok\cheque.docx";
        private DataTable ordersTable = new DataTable();
        private int selectedRow = -1;
        MySqlDataAdapter da;
        public SozdanieCheka()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadData();
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void LoadData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                try
                {
                    con.Open();
                    string query = "SELECT NumberOrders, StartDateOrder, EndDateOrder, NameClient, SurnameClient, MiddleNameClient, FullNameEmployee, Services, CostServices, CostComponentReplaced, TotalCost, NameClientDevice, OrderStatus, NameComponentReplaced, QuantityComponentReplaced, OrderdDescription, NumberPhoneClient, StatusVIPClient FROM orders";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    ordersTable.Clear();
                    adapter.Fill(ordersTable);
                    dataGridView1.DataSource = ordersTable;

                                    // Запретить добавление пустой строки
                    dataGridView1.AllowUserToAddRows = false;

                    // Установка режима заполнения для всех столбцов
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Устанавливаем режим заполнения
                    }

                    // Изменение заголовков столбцов на русский
                    dataGridView1.Columns["NumberOrders"].HeaderText = "Номер заказа";
                    dataGridView1.Columns["StartDateOrder"].HeaderText = "Дата начала заказа";
                    dataGridView1.Columns["EndDateOrder"].HeaderText = "Дата окончания заказа";
                    dataGridView1.Columns["NameClient"].HeaderText = "Имя клиента";
                    dataGridView1.Columns["SurnameClient"].HeaderText = "Фамилия клиента";
                    dataGridView1.Columns["MiddleNameClient"].HeaderText = "Отчество клиента";
                    dataGridView1.Columns["FullNameEmployee"].HeaderText = "ФИО сотрудника";
                    dataGridView1.Columns["Services"].HeaderText = "Услуги";
                    dataGridView1.Columns["CostServices"].HeaderText = "Стоимость услуг";
                    dataGridView1.Columns["CostComponentReplaced"].HeaderText = "Стоимость заменяемых компонентов";
                    dataGridView1.Columns["TotalCost"].HeaderText = "Общая стоимость";
                    dataGridView1.Columns["NameClientDevice"].HeaderText = "Устройство клиента";
                    dataGridView1.Columns["OrderStatus"].HeaderText = "Статус заказа";
                    dataGridView1.Columns["NameComponentReplaced"].HeaderText = "Название заменяемого компонента";
                    dataGridView1.Columns["QuantityComponentReplaced"].HeaderText = "Количество заменяемых компонентов";
                    dataGridView1.Columns["OrderdDescription"].HeaderText = "Описание заказа";
                    dataGridView1.Columns["NumberPhoneClient"].HeaderText = "Номер телефона клиента";
                    dataGridView1.Columns["StatusVIPClient"].HeaderText = "Статус VIP клиента";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrZakazov main = new ProsmotrZakazov();
            main.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedRow != -1)
            {
                DataRow row = ordersTable.Rows[selectedRow];

                string NumberOrders = row["NumberOrders"].ToString();
                string EndDateOrder = row["EndDateOrder"].ToString();
                string Services = row["Services"].ToString();
                string CostServices = row["CostServices"].ToString();
                string NameComponentReplaced = row["NameComponentReplaced"].ToString();
                string QuantityComponentReplaced = row["QuantityComponentReplaced"].ToString();
                string CostComponentReplaced = row["CostComponentReplaced"].ToString();
                string TotalCost = row["TotalCost"].ToString();
                string FullNameEmployee = row["FullNameEmployee"].ToString();

                // Создаем полное имя клиента
                string clientName = $"{row["NameClient"]} {row["SurnameClient"]} {row["MiddleNameClient"]}";

                var wordApp = new Word.Application();
                wordApp.Visible = false;
                try
                {
                    var wordDocument = wordApp.Documents.Open(FileName);

                    // Заменяем шаблоны в документе
                    ReplaceWordStub("{NumberOrders}", NumberOrders, wordDocument);
                    ReplaceWordStub("{EndDateOrder}", EndDateOrder, wordDocument);
                    ReplaceWordStub("{Services}", Services, wordDocument);
                    ReplaceWordStub("{CostServices}", CostServices, wordDocument);
                    ReplaceWordStub("{NameComponentReplaced}", NameComponentReplaced, wordDocument);
                    ReplaceWordStub("{QuantityComponentReplaced}", QuantityComponentReplaced, wordDocument);
                    ReplaceWordStub("{CostComponentReplaced}", CostComponentReplaced, wordDocument);
                    ReplaceWordStub("{TotalCost}", TotalCost, wordDocument);
                    ReplaceWordStub("{FullNameEmployee}", FullNameEmployee, wordDocument);
                    ReplaceWordStub("{ClientFullName}", clientName, wordDocument); 
                    ReplaceWordStub("{ClientFullName}", clientName, wordDocument);
                    ReplaceWordStub("{TotalCost}", TotalCost, wordDocument);

                    wordApp.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Отчет об ошибке при создании");
                }
                finally
                {
                    wordApp.Quit();
                    Marshal.FinalReleaseComObject(wordApp);
                }
            }
        }
        private void ReplaceWordStub(string stubToReplace, string text, Word.Document wordDocument)
        {
            var range = wordDocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                selectedRow = e.RowIndex;
            }
        }
    }
}
