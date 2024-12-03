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

namespace PenkovNikitaKR
{
    public partial class Import : Form
    {
        //
        public Import()
        {
            InitializeComponent();
            LoadTableNames();
        }
        
        private void LoadTableNames()
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                var cmd = new MySqlCommand("SHOW TABLES;", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxTables.Items.Add(reader[0].ToString());
                    }
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBoxTables.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите таблицу из списка.");
                return;
            }

            string selectedTable = comboBoxTables.SelectedItem.ToString();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    ImportCsvToDatabase(filePath, selectedTable);
                }
            }
        }
        private void ImportCsvToDatabase(string filePath, string tableName)
        {
            int importedRecordsCount = 0;

            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
                {
                    con.Open();

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        // Skip header
                        reader.ReadLine();

                        // Get column count from table
                        int columnCount = GetColumnCount(con, tableName);

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            // Check for parameter count mismatch
                            if (values.Length != columnCount)
                            {
                                MessageBox.Show($"Parameter count mismatch: expected {columnCount}, but got {values.Length}.");
                                return;
                            }

                            // Create SQL insert command
                            var insertCommand = new MySqlCommand($"INSERT INTO {tableName} VALUES ({string.Join(",", values.Select(v => $"'{v}'"))});", con);
                            insertCommand.ExecuteNonQuery();
                            importedRecordsCount++;
                        }
                    }
                }

                MessageBox.Show($"Successfully imported {importedRecordsCount} records.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing data: {ex.Message}");
            }
        }
        private int GetColumnCount(MySqlConnection con, string tableName)
        {
            var getColumnCountCommand = new MySqlCommand($"DESCRIBE {tableName}", con);
            int columnCount = 0;
            using (var reader = getColumnCountCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnCount++;
                }
            }
            return columnCount;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(ConnectionString.connectionString()))
            {
                con.Open();
                try
                {
                    string createSchemaScript = @"
                DROP TABLE IF EXISTS `componentdevice_has_orders`;
                DROP TABLE IF EXISTS `gender`;
                DROP TABLE IF EXISTS `employee`;
                DROP TABLE IF EXISTS `client`;
                DROP TABLE IF EXISTS `componentdevice`;
                DROP TABLE IF EXISTS `orders`;
                DROP TABLE IF EXISTS `orderstatus`;
                DROP TABLE IF EXISTS `role`;
                DROP TABLE IF EXISTS `services`;
                DROP TABLE IF EXISTS `statusvip`;

                CREATE TABLE `gender` (
                    `idgender` int NOT NULL AUTO_INCREMENT,
                    `NameGender` varchar(255) NOT NULL,
                    PRIMARY KEY (`idgender`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `role` (
                    `idrole` int NOT NULL AUTO_INCREMENT,
                    `RoleName` varchar(255) NOT NULL,
                    PRIMARY KEY (`idrole`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `statusvip` (
                    `idstatusVIP` int NOT NULL AUTO_INCREMENT,
                    `StatusVIP` varchar(255) NOT NULL,
                    `Discount` int DEFAULT NULL,
                    PRIMARY KEY (`idstatusVIP`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `services` (
                    `idservices` int NOT NULL AUTO_INCREMENT,
                    `Name` varchar(255) NOT NULL,
                    `Cost` int NOT NULL,
                    `Time` time NOT NULL,
                    `DescriptionServices` varchar(255) NOT NULL,
                    PRIMARY KEY (`idservices`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `client` (
                    `idClient` int NOT NULL AUTO_INCREMENT,
                    `Name` varchar(255) NOT NULL,
                    `Surname` varchar(255) NOT NULL,
                    `MiddleName` varchar(255) NOT NULL,
                    `PhoneNumber` bigint NOT NULL,
                    `StatusVIP` varchar(255) NOT NULL,
                    `statusvip_idstatusVIP` int NOT NULL,
                    PRIMARY KEY (`idClient`),
                    KEY `fk_client_statusvip1_idx` (`statusvip_idstatusVIP`),
                    CONSTRAINT `fk_client_statusvip1` FOREIGN KEY (`statusvip_idstatusVIP`) REFERENCES `statusvip` (`idstatusVIP`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `employee` (
                    `idemployee` int NOT NULL AUTO_INCREMENT,
                    `Surname` varchar(255) NOT NULL,
                    `Name` varchar(255) NOT NULL,
                    `MiddleName` varchar(255) NOT NULL,
                    `Address` varchar(255) NOT NULL,
                    `Age` int NOT NULL,
                    `PhoneNumber` bigint NOT NULL,
                    `Photo` varchar(255) DEFAULT NULL,
                    `Gender` varchar(255) NOT NULL,
                    `Role` varchar(255) NOT NULL,
                    `login` varchar(255) NOT NULL,
                    `password` varchar(255) NOT NULL,
                    `role_idrole` int NOT NULL,
                    `gender_idgender` int NOT NULL,
                    PRIMARY KEY (`idemployee`),
                    KEY `fk_employee_role1_idx` (`role_idrole`),
                    KEY `fk_employee_gender1_idx` (`gender_idgender`),
                    CONSTRAINT `fk_employee_gender1` FOREIGN KEY (`gender_idgender`) REFERENCES `gender` (`idgender`),
                    CONSTRAINT `fk_employee_role1` FOREIGN KEY (`role_idrole`) REFERENCES `role` (`idrole`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `componentdevice` (
                    `idcomponentdevice` int NOT NULL AUTO_INCREMENT,
                    `NameComponentDevice` varchar(255) NOT NULL,
                    `CostComponentDevice` int NOT NULL,
                    PRIMARY KEY (`idcomponentdevice`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `orderstatus` (
                    `idorderstatus` int NOT NULL AUTO_INCREMENT,
                    `NameOrderStatus` varchar(255) NOT NULL,
                    PRIMARY KEY (`idorderstatus`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `orders` (
                    `NumberOrders` int NOT NULL AUTO_INCREMENT,
                    `StartDateOrder` date NOT NULL,
                    `EndDateOrder` date NOT NULL,
                    `NameClient` varchar(255) NOT NULL,
                    `SurnameClient` varchar(255) NOT NULL,
                    `MiddleNameClient` varchar(255) NOT NULL,
                    `FullNameEmployee` varchar(255) NOT NULL,
                    `Services` varchar(255) NOT NULL,
                    `CostServices` int NOT NULL,
                    `CostComponentReplaced` int DEFAULT NULL,
                    `TotalCost` int NOT NULL,
                    `NameClientDevice` varchar(255) DEFAULT NULL,
                    `OrderStatus` varchar(255) NOT NULL,
                    `NameComponentReplaced` varchar(255) DEFAULT NULL,
                    `QuantityComponentReplaced` int DEFAULT NULL,
                    `OrderdDescription` varchar(255) NOT NULL,
                    `client_idClient` int NOT NULL,
                    `services_idservices` int NOT NULL,
                    `employee_idemployee` int NOT NULL,
                    `orderstatus_idorderstatus` int NOT NULL,
                    `NumberPhoneClient` bigint NOT NULL,
                    `StatusVIPClient` varchar(255) NOT NULL,
                    `statusvip_idstatusVIP` int NOT NULL,
                    PRIMARY KEY (`NumberOrders`),
                    KEY `fk_orders_statusvip` (`statusvip_idstatusVIP`),
                    KEY `fk_orders_client1_idx` (`client_idClient`),
                    KEY `fk_orders_services1_idx` (`services_idservices`),
                    KEY `fk_orders_employee1_idx` (`employee_idemployee`),
                    KEY `fk_orders_orderstatus1_idx` (`orderstatus_idorderstatus`),
                    CONSTRAINT `fk_orders_client` FOREIGN KEY (`client_idClient`) REFERENCES `client` (`idClient`),
                    CONSTRAINT `fk_orders_employee` FOREIGN KEY (`employee_idemployee`) REFERENCES `employee` (`idemployee`),
                    CONSTRAINT `fk_orders_orderstatus` FOREIGN KEY (`orderstatus_idorderstatus`) REFERENCES `orderstatus` (`idorderstatus`),
                    CONSTRAINT `fk_orders_services` FOREIGN KEY (`services_idservices`) REFERENCES `services` (`idservices`),
                    CONSTRAINT `fk_orders_statusvip` FOREIGN KEY (`statusvip_idstatusVIP`) REFERENCES `statusvip` (`idstatusVIP`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

               

                CREATE TABLE `componentdevice_has_orders` (
                    `componentdevice_idcomponentdevice` int NOT NULL,
                    `orders_NumberOrders` int NOT NULL,
                    PRIMARY KEY (`componentdevice_idcomponentdevice`,`orders_NumberOrders`),
                    KEY `fk_componentdevice_has_orders_orders1_idx` (`orders_NumberOrders`),
                    KEY `fk_componentdevice_has_orders_componentdevice_idx` (`componentdevice_idcomponentdevice`),
                    CONSTRAINT `fk_componentdevice_has_orders_componentdevice` FOREIGN KEY (`componentdevice_idcomponentdevice`) REFERENCES `componentdevice` (`idcomponentdevice`),
                    CONSTRAINT `fk_componentdevice_has_orders_orders1` FOREIGN KEY (`orders_NumberOrders`) REFERENCES `orders` (`NumberOrders`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
            ";

                    using (MySqlCommand command = new MySqlCommand(createSchemaScript, con))
                    {
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Структура базы данных восстановлена успешно.");
                    LoadTableNames();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка восстановления структуры базы данных: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MenuAdmin main = new MenuAdmin();
            main.ShowDialog();
        }
    }
}
