using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace PenkovNikitaKR
{
    public partial class detalprosmotr : Form
    {
        private string orderNumber;

        public detalprosmotr(string orderNumber)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            this.orderNumber = orderNumber;
            LoadOrderDetails();
            LoadOrderStatus(); // Загружаем статусы заказов
            DisableInputFields();
        }

        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void DisableInputFields()
        {
            // Делаем все текстбоксы только для чтения
            textBoxStartDateOrder.ReadOnly = true;
            textBoxEndDateOrder.ReadOnly = true;
            textBoxFullNameClient.ReadOnly = true;
            textBoxFullNameEmployee.ReadOnly = true;
            textBoxServices.ReadOnly = true;
            textBoxCostServices.ReadOnly = true;
            textBoxCostComponentReplaced.ReadOnly = true;
            textBoxTotalCost.ReadOnly = true;
            textBoxNameClientDevice.ReadOnly = true;
            textBoxNameComponentReplaced.ReadOnly = true;
            textBoxQuantityComponentReplaced.ReadOnly = true;
            textBoxOrderDescription.ReadOnly = true;
            textBoxNumberPhoneClient.ReadOnly = true;
            textBoxStatusVIPClient.ReadOnly = true;
        }
        private void LoadOrderDetails()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT NumberOrders, StartDateOrder, EndDateOrder, NameClient, SurnameClient, MiddleNameClient, FullNameEmployee, Services, CostServices, CostComponentReplaced, TotalCost, NameClientDevice, OrderStatus, NameComponentReplaced, QuantityComponentReplaced, OrderdDescription, NumberPhoneClient, StatusVIPClient FROM orders WHERE NumberOrders = @NumberOrders";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NumberOrders", orderNumber);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBoxStartDateOrder.Text = reader["StartDateOrder"].ToString();
                        textBoxEndDateOrder.Text = reader["EndDateOrder"].ToString();

                        // Объединяю имя, фамилию и отчество в один текстбокс
                        textBoxFullNameClient.Text = $"{reader["NameClient"]} {reader["SurnameClient"]} {reader["MiddleNameClient"]}";

                        textBoxFullNameEmployee.Text = reader["FullNameEmployee"].ToString();
                        textBoxServices.Text = reader["Services"].ToString();
                        textBoxCostServices.Text = reader["CostServices"].ToString();
                        textBoxCostComponentReplaced.Text = reader["CostComponentReplaced"].ToString();
                        textBoxTotalCost.Text = reader["TotalCost"].ToString();
                        textBoxNameClientDevice.Text = reader["NameClientDevice"].ToString();
                        comboBoxOrderStatus.SelectedValue = reader["OrderStatus"].ToString();
                        textBoxNameComponentReplaced.Text = reader["NameComponentReplaced"].ToString();
                        textBoxQuantityComponentReplaced.Text = reader["QuantityComponentReplaced"].ToString();
                        textBoxOrderDescription.Text = reader["OrderdDescription"].ToString();
                        textBoxNumberPhoneClient.Text = reader["NumberPhoneClient"].ToString();
                        textBoxStatusVIPClient.Text = reader["StatusVIPClient"].ToString();
                    }
                }
            }
        }

        private void LoadOrderStatus()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT NameOrderStatus FROM orderstatus"; // Загружаем статусы заказов
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable orderStatusTable = new DataTable();
                adapter.Fill(orderStatusTable);

                comboBoxOrderStatus.DataSource = orderStatusTable;
                comboBoxOrderStatus.DisplayMember = "NameOrderStatus"; // Отображаемое имя
                comboBoxOrderStatus.ValueMember = "NameOrderStatus"; // Значение
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "UPDATE orders SET OrderStatus = @OrderStatus WHERE NumberOrders = @NumberOrders";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OrderStatus", comboBoxOrderStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@NumberOrders", orderNumber);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Статус заказа успешно обновлен.");
            }
            this.Visible = false;
            HistoryOrder main = new HistoryOrder();
            main.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            HistoryOrder main = new HistoryOrder();
            main.ShowDialog();
        }
    }
}