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
    public partial class RedaktirovanieZakazov : Form
    {       
        private int orderId; // ID заказа для редактирования
        public RedaktirovanieZakazov(int orderId)
        {
            this.orderId = orderId; // Сохранение переданного orderId
            InitializeComponent();
            LoadOrderData();
            this.StartPosition = FormStartPosition.CenterScreen; // Установка позиции формы по центру экрана
            this.Resize += new EventHandler(ProsmotrYslyk_Resize);
            LoadServices();
            LoadOrderStatuses();
            LoadVIPStatuses();
            LoadEmployees();
            SetupAutoComplete();
            SetupComponentNameAutoComplete();
            LoadClientDeviceNames(); // Загрузка названий устройств
            textBoxComponentQuantity.TextChanged += textBoxComponentQuantity_TextChanged;
            comboBoxService.SelectedIndexChanged += comboBoxService_SelectedIndexChanged;
            comboBoxVIPStatus.SelectedIndexChanged += comboBoxVIPStatus_SelectedIndexChanged;
            comboBoxClientName.SelectedIndexChanged += comboBoxClientName_SelectedIndexChanged; // Добавлено
            DisableInputFields();
        }
        private void ProsmotrYslyk_Resize(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void LoadOrderData()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT * FROM orders WHERE NumberOrders = @orderId";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@orderId", orderId); // Используем переданный orderId

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Заполнение текстовых полей данными заказа
                        string name = reader["NameClient"].ToString();
                        string surname = reader["SurnameClient"].ToString();
                        string middleName = reader["MiddleNameClient"].ToString();
                        comboBoxClientName.Text = $"{name} {surname} {middleName}"; // Объединяем имя, фамилию и отчество

                        textBoxPhoneNumber.Text = reader["NumberPhoneClient"].ToString();
                        comboBoxVIPStatus.SelectedValue = reader["StatusVIPClient"].ToString();
                        comboBoxService.SelectedValue = reader["Services"].ToString();
                        comboBoxEmployee.SelectedValue = reader["FullNameEmployee"].ToString();

                        comboBoxComponentName.Text = reader["NameComponentReplaced"].ToString();
                        textBoxComponentQuantity.Text = reader["QuantityComponentReplaced"].ToString();
                        textBoxOrderDescription.Text = reader["OrderdDescription"].ToString();

                        comboBoxClientDeviceName.SelectedValue = reader["NameClientDevice"].ToString();
                        comboBoxOrderStatus.SelectedValue = reader["OrderStatus"].ToString();

                        // Заполнение полей стоимости
                        textBoxServiceCost.Text = reader["CostServices"].ToString();
                        textBoxComponentCost.Text = reader["CostComponentReplaced"].ToString();
                        textBoxTotalCost.Text = reader["TotalCost"].ToString();

                        // Установка выбранной даты в календаре
                        monthCalendar1.SetDate(Convert.ToDateTime(reader["StartDateOrder"]));
                        monthCalendar2.SetDate(Convert.ToDateTime(reader["EndDateOrder"]));
                    }
                    else
                    {
                        MessageBox.Show("Заказ не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void DisableInputFields()
        {
            // Делаем все текстбоксы только для чтения                             
            textBoxServiceCost.ReadOnly = true;
            textBoxComponentCost.ReadOnly = true;
            textBoxTotalCost.ReadOnly = true;
            textBoxPhoneNumber.ReadOnly = true;
            comboBoxVIPStatus.Enabled = false;
        }
        // Метод для загрузки названий устройств в comboBoxClientDeviceName
        private void LoadClientDeviceNames()
        {
            List<string> deviceNames = new List<string> { "ПК", "Ноутбук", "Смартфон", "Телевизор", "Монитор" };
            comboBoxClientDeviceName.DataSource = deviceNames;
        }
        private void LoadServices()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idservices, Name FROM services"; // Загружаем id и имена услуг
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable servicesTable = new DataTable();
                adapter.Fill(servicesTable);

                comboBoxService.DataSource = servicesTable;
                comboBoxService.DisplayMember = "Name"; // Отображаемое значение в комбобоксе
                comboBoxService.ValueMember = "Name"; // Сохраняем id услуги
            }
        }

        private void comboBoxService_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateServiceCost();
        }

        private void UpdateServiceCost()
        {
            if (comboBoxService.SelectedItem != null)
            {
                // Получаем id услуги
                int selectedServiceId = (int)((DataRowView)comboBoxService.SelectedItem)["idservices"];

                // Получаем стоимость услуги по id
                decimal serviceCost = GetServiceCost(selectedServiceId);

                // Получаем скидку из comboBoxVIPStatus
                decimal discount = GetVIPDiscount(comboBoxVIPStatus.SelectedValue?.ToString());

                // Применяем скидку
                decimal finalCost = serviceCost - (serviceCost * discount / 100);
                textBoxServiceCost.Text = finalCost.ToString("F2"); // Форматируем до 2 знаков после запятой
            }
            else
            {
                textBoxServiceCost.Clear();
            }
        }

        private decimal GetServiceCost(int serviceId)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT Cost FROM services WHERE idservices = @ServiceId"; // Используем id
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceId", serviceId);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }


        private void comboBoxVIPStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateServiceCost();
        }

        private void LoadVIPStatuses()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT StatusVIP FROM statusvip";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable vipTable = new DataTable();
                adapter.Fill(vipTable);

                comboBoxVIPStatus.DataSource = vipTable;
                comboBoxVIPStatus.DisplayMember = "StatusVIP";
                comboBoxVIPStatus.ValueMember = "StatusVIP"; // Убедитесь, что ValueMember установлен
            }
        }
        private void LoadEmployees()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idemployee, CONCAT(Surname, ' ', Name, ' ', MiddleName) AS FullName FROM employee WHERE Role = 'Менеджер'";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable employeeTable = new DataTable();
                adapter.Fill(employeeTable);

                comboBoxEmployee.DataSource = employeeTable;
                comboBoxEmployee.DisplayMember = "FullName";
                comboBoxEmployee.ValueMember = "FullName"; // Сохраняем id для использования позже
            }
        }
        private void SetupAutoComplete()
        {
            // Настройка автозаполнения для comboBoxClientName
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT CONCAT(Name, ' ', Surname, ' ', MiddleName) AS FullName FROM client";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                List<string> clientNames = new List<string>();
                while (reader.Read())
                {
                    clientNames.Add(reader["FullName"].ToString());
                }

                comboBoxClientName.DataSource = clientNames; // Заполнение комбобокса
                comboBoxClientName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBoxClientName.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
        }

        private decimal GetVIPDiscount(string statusVIP)
        {
            if (string.IsNullOrEmpty(statusVIP))
            {
                return 0; // Если статус VIP не выбран, скидка 0%
            }

            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT Discount FROM statusvip WHERE StatusVIP = @StatusVIP";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StatusVIP", statusVIP);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }
        private void SetupComponentNameAutoComplete()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT NameComponentDevice FROM componentdevice";
                MySqlCommand cmd = new MySqlCommand(query, con);
                MySqlDataReader reader = cmd.ExecuteReader();

                List<string> componentNames = new List<string>();
                while (reader.Read())
                {
                    componentNames.Add(reader["NameComponentDevice"].ToString());
                }

                comboBoxComponentName.DataSource = componentNames; // Заполнение комбобокса
                comboBoxComponentName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBoxComponentName.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
        }
        private void comboBoxComponentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxComponentName.Text))
            {
                string componentName = comboBoxComponentName.Text.Trim();
                decimal componentCost = GetComponentCost(componentName);
                textBoxComponentCost.Text = componentCost.ToString("F2"); // Форматируем до 2 знаков после запятой
                UpdateTotalCost();
            }
        }
        private decimal GetComponentCost(string componentName)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT CostComponentDevice FROM componentdevice WHERE NameComponentDevice = @ComponentName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ComponentName", componentName);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }
        private void LoadOrderStatuses()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT NameOrderStatus FROM orderstatus";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                DataTable statusTable = new DataTable();
                adapter.Fill(statusTable);

                comboBoxOrderStatus.DataSource = statusTable;
                comboBoxOrderStatus.DisplayMember = "NameOrderStatus"; // Отображаемое значение
                comboBoxOrderStatus.ValueMember = "NameOrderStatus"; // Значение, которое будет сохраняться
            }
        }
        private void textBoxComponentQuantity_TextChanged(object sender, EventArgs e)
        {
            UpdateComponentTotalCost();
            UpdateTotalCost();

            // Если текстбокс пустой, обновляем стоимость компонента
            if (string.IsNullOrEmpty(textBoxComponentQuantity.Text))
            {
                decimal componentCost = GetComponentCost(comboBoxComponentName.Text.Trim()); // Получаем стоимость компонента
                textBoxComponentCost.Text = componentCost.ToString("F2"); // Обновляем стоимость компонента без умножения на количество
            }
            UpdateTotalCost();
        }
        private void UpdateComponentTotalCost()
        {
            decimal componentCost = string.IsNullOrEmpty(textBoxComponentCost.Text) ? 0 : Convert.ToDecimal(textBoxComponentCost.Text);
            int quantity = string.IsNullOrEmpty(textBoxComponentQuantity.Text) ? 0 : Convert.ToInt32(textBoxComponentQuantity.Text);

            // общая стоимость компонента
            decimal totalComponentCost = componentCost * quantity;

            // Обновляем текстовое поле с общей стоимостью компонента
            textBoxComponentCost.Text = totalComponentCost.ToString("F2"); // Обновляем стоимость компонента
        }

        private void UpdateTotalCost()
        {
            decimal serviceCost = string.IsNullOrEmpty(textBoxServiceCost.Text) ? 0 : Convert.ToDecimal(textBoxServiceCost.Text);
            decimal componentCost = string.IsNullOrEmpty(textBoxComponentCost.Text) ? 0 : Convert.ToDecimal(textBoxComponentCost.Text);
            int quantity = string.IsNullOrEmpty(textBoxComponentQuantity.Text) ? 0 : Convert.ToInt32(textBoxComponentQuantity.Text);

            // Если quantity не указано, используем только стоимость услуги и компонента
            if (quantity == 0)
            {
                decimal totalCost = serviceCost + componentCost;
                textBoxTotalCost.Text = totalCost.ToString("F2"); // Форматируем до 2 знаков после запятой
            }
            else
            {
                // Вычисляем общую стоимость с учетом количества компонентов
                decimal totalComponentCost = componentCost * quantity;
                decimal totalCost = serviceCost + totalComponentCost;
                textBoxTotalCost.Text = totalCost.ToString("F2"); // Форматируем до 2 знаков после запятой
            }
        }
        private void comboBoxClientName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // Метод для получения ID клиента по имени
        private int GetClientId(string name, string surname, string middleName)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idClient FROM client WHERE Name = @Name AND Surname = @Surname AND MiddleName = @MiddleName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Surname", surname);
                cmd.Parameters.AddWithValue("@MiddleName", middleName);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private int GetOrderStatusId(string orderStatusName)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idOrderStatus FROM orderstatus WHERE NameOrderStatus = @OrderStatusName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@OrderStatusName", orderStatusName);

                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception("Статус заказа не найден."); // Или вернуть значение по умолчанию
                }
                return Convert.ToInt32(result);
            }
        }
        private int GetServiceIdByName(string servicesTable)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idservices FROM services WHERE Name = @ServiceName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ServiceName", servicesTable);

                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception("Услуга заказа не найден."); // Или вернуть значение по умолчанию
                }
                return Convert.ToInt32(result);
            }
        }
        // Метод для получения ID клиента по имени
        private int GetSotrydnikId(string surname1, string name1, string middleName1)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idemployee FROM employee WHERE Name = @Name AND Surname = @Surname AND MiddleName = @MiddleName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", name1);
                cmd.Parameters.AddWithValue("@Surname", surname1);
                cmd.Parameters.AddWithValue("@MiddleName", middleName1);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
        private bool ValidateFields()
        {
            // Проверка текстовых полей
            if (
                string.IsNullOrWhiteSpace(textBoxPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(textBoxOrderDescription.Text) ||
                string.IsNullOrWhiteSpace(comboBoxClientDeviceName.Text) ||
                comboBoxVIPStatus.SelectedItem == null ||
                comboBoxService.SelectedItem == null ||
                comboBoxEmployee.SelectedItem == null ||
                comboBoxClientName.SelectedItem == null ||
                comboBoxOrderStatus.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }



            // Если все проверки пройдены
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверяем поля перед добавлением заказа
            if (!ValidateFields())
            {
                return; // Выходим, если валидация не прошла
            }

            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                MySqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Получаем полное имя клиента из комбобокса
                    string selectedName = comboBoxClientName.SelectedItem?.ToString();
                    if (string.IsNullOrEmpty(selectedName))
                    {
                        MessageBox.Show("Пожалуйста, выберите клиента из списка.");
                        return;
                    }

                    string[] names = selectedName.Split(' ');
                    if (names.Length < 3)
                    {
                        MessageBox.Show("Пожалуйста, выберите полное имя клиента (Имя Фамилия Отчество).");
                        return;
                    }

                    string name = names[0];
                    string surname = names[1];
                    string middleName = names[2];

                    // Получение ID клиента из базы данных
                    int clientId = GetClientId(name, surname, middleName);
                    if (clientId == 0)
                    {
                        MessageBox.Show("Клиент не найден в базе данных.");
                        return;
                    }

                    // Получаем ID сотрудника по имени
                    string[] names1 = comboBoxEmployee.Text.Split(' ');
                    if (names1.Length < 3)
                    {
                        MessageBox.Show("Пожалуйста, введите полное имя сотрудника (Имя Фамилия Отчество).");
                        return;
                    }

                    string surname1 = names1[0];
                    string name1 = names1[1];
                    string middleName1 = names1[2];

                    // Получение ID сотрудника из базы данных
                    int sotrydnikId = GetSotrydnikId(surname1, name1, middleName1);
                    if (sotrydnikId == 0)
                    {
                        MessageBox.Show("Сотрудник не найден в базе данных.");
                        return;
                    }

                    // Получение стоимости услуги
                    int serviceCost = (int)Math.Ceiling(string.IsNullOrEmpty(textBoxServiceCost.Text) ? 0 : Convert.ToDecimal(textBoxServiceCost.Text));
                    // Получение стоимости компонента
                    int componentCost = (int)Math.Ceiling(string.IsNullOrEmpty(textBoxComponentCost.Text) ? 0 : Convert.ToDecimal(textBoxComponentCost.Text));
                    // Получение ID статуса заказа
                    int orderStatusId = GetOrderStatusId(comboBoxOrderStatus.Text);
                    // Получение ID статуса VIP
                    int statusVIPId = GetVIPStatusId(comboBoxVIPStatus.SelectedValue.ToString());
                    // Получение ID услуги заказа
                    int serviceId = GetServiceIdByName(comboBoxService.Text);
                    // Получаем количество компонентов из textBoxComponentQuantity, устанавливаем 0, если пусто
                    int quantity = string.IsNullOrEmpty(textBoxComponentQuantity.Text) ? 0 : Convert.ToInt32(textBoxComponentQuantity.Text);

                    // Получение номера заказа
                    int orderNumber = orderId;

                    // Получение общей стоимости
                    int totalCost = serviceCost + componentCost;


                    // Обновление заказа в таблице orders
                    string updateOrderQuery = "UPDATE orders SET " +
                                              "StartDateOrder = @StartDate, " +
                                              "EndDateOrder = @EndDate, " +
                                              "NameClient = @NameClient, " +
                                              "SurnameClient = @SurnameClient, " +
                                              "MiddleNameClient = @MiddleNameClient, " +
                                              "NumberPhoneClient = @NumberPhoneClient, " +
                                              "StatusVIPClient = @StatusVIPClient, " +
                                              "FullNameEmployee = @FullNameEmployee, " +
                                              "Services = @Service, " +
                                              "CostServices = @ServiceCost, " +
                                              "CostComponentReplaced = @CostComponentReplaced, " +
                                              "TotalCost = @TotalCost, " +
                                              "NameClientDevice = @ClientDeviceName, " +
                                              "OrderStatus = @OrderStatus, " +
                                              "client_idClient = @ClientId, " +
                                              "services_idservices = @ServiceId, " +
                                              "orderstatus_idorderstatus = @OrderStatusId, " +
                                              "statusvip_idstatusVIP = @StatusVIPId, " +
                                              "QuantityComponentReplaced = @QuantityComponentReplaced, " +
                                              "OrderdDescription = @Description, " +
                                              "NameComponentReplaced = @NameComponentReplaced, " +
                                              "employee_idemployee = @SotrydnikId " +
                                              "WHERE NumberOrders = @NumberOrders";
                    MySqlCommand cmdOrder = new MySqlCommand(updateOrderQuery, con, transaction);
                    cmdOrder.Parameters.AddWithValue("@StartDate", monthCalendar1.SelectionStart);
                    cmdOrder.Parameters.AddWithValue("@EndDate", monthCalendar2.SelectionEnd);
                    cmdOrder.Parameters.AddWithValue("@NameClient", name);
                    cmdOrder.Parameters.AddWithValue("@SurnameClient", surname);
                    cmdOrder.Parameters.AddWithValue("@MiddleNameClient", middleName);
                    cmdOrder.Parameters.AddWithValue("@NumberPhoneClient", textBoxPhoneNumber.Text);
                    cmdOrder.Parameters.AddWithValue("@FullNameEmployee", comboBoxEmployee.SelectedValue);
                    cmdOrder.Parameters.AddWithValue("@Service", comboBoxService.SelectedValue);
                    cmdOrder.Parameters.AddWithValue("@ServiceCost", serviceCost);
                    cmdOrder.Parameters.AddWithValue("@CostComponentReplaced", componentCost);
                    cmdOrder.Parameters.AddWithValue("@NameComponentReplaced", comboBoxComponentName.SelectedItem?.ToString() ?? string.Empty); // Используем комбобокс
                    cmdOrder.Parameters.AddWithValue("@QuantityComponentReplaced", quantity);
                    cmdOrder.Parameters.AddWithValue("@Description", textBoxOrderDescription.Text);
                    cmdOrder.Parameters.AddWithValue("@ClientDeviceName", comboBoxClientDeviceName.SelectedItem.ToString());
                    cmdOrder.Parameters.AddWithValue("@TotalCost", totalCost);
                    cmdOrder.Parameters.AddWithValue("@OrderStatus", comboBoxOrderStatus.SelectedValue);
                    cmdOrder.Parameters.AddWithValue("@ClientId", clientId);
                    cmdOrder.Parameters.AddWithValue("@ServiceId", serviceId); // Используем id услуги
                    cmdOrder.Parameters.AddWithValue("@OrderStatusId", orderStatusId);
                    cmdOrder.Parameters.AddWithValue("@StatusVIPId", statusVIPId);
                    cmdOrder.Parameters.AddWithValue("@NumberOrders", orderNumber); // Используем номер заказа
                    cmdOrder.Parameters.AddWithValue("@StatusVIPClient", comboBoxVIPStatus.SelectedValue);
                    cmdOrder.Parameters.AddWithValue("@SotrydnikId", sotrydnikId);


                    cmdOrder.ExecuteNonQuery();

                    // Подтверждение транзакции
                    transaction.Commit();
                    MessageBox.Show("Заказ успешно отредактирован!");
                    ClearFields();
                    this.Visible = false;
                    ProsmotrZakazov main = new ProsmotrZakazov();
                    main.ShowDialog();
                }
                catch (Exception ex)
                {
                    // Откат транзакции в случае ошибки
                    transaction.Rollback();
                    MessageBox.Show("Ошибка при редактировании заказа: " + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        // Метод для получения ID статуса VIP
        private int GetVIPStatusId(string statusVIP)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                string query = "SELECT idstatusVIP FROM statusvip WHERE StatusVIP = @StatusVIP";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StatusVIP", statusVIP);

                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
            if (textBox.Text.Length >= 2 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрет ввода
            }
        }
        private void TextBoxNameComponentDevice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Ограничение на количество символов
            if (textBoxOrderDescription.Text.Length >= 40 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Запрещаем ввод, если превышено количество символов
            }
        }
        private void ClearFields()
        {
            // Очистка всех полей формы
            comboBoxClientName.SelectedItem = null;
            textBoxPhoneNumber.Clear();
            comboBoxVIPStatus.SelectedItem = null;
            comboBoxService.SelectedItem = null;
            comboBoxEmployee.SelectedItem = null;
            comboBoxOrderStatus.SelectedItem = null;
            comboBoxClientDeviceName.SelectedItem = null;
            textBoxServiceCost.Clear();
            textBoxTotalCost.Clear();
            textBoxOrderDescription.Clear();
            monthCalendar1.SelectionStart = DateTime.Now;
            monthCalendar2.SelectionEnd = DateTime.Now;
            comboBoxComponentName.SelectedItem = null;
            textBoxComponentCost.Clear();
            textBoxComponentQuantity.Clear();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Visible = false;
            ProsmotrZakazov main = new ProsmotrZakazov();
            main.ShowDialog();
        }
    }
}
