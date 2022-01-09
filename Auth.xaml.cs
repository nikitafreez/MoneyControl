using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;

namespace MoneyControl
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader reader;
        private string dbFileName = "MoneyStat.db";

        public Auth()
        {
            InitializeComponent();

            string connectionString = $"Data Source={dbFileName};Version=3";
            connection = new SQLiteConnection(connectionString);

            GetDB();
        }

        private void GetDB()
        {
            if (!File.Exists(dbFileName))
            {
                try
                {
                    connection.Open();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Users(" +
                        "ID_User INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "Login VARCHAR," +
                        "Password VARCHAR," +
                        "TotalMoney INTEGER);";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Category(" +
                        "ID_Category INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "CategoryName VARCHAR);";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "CREATE TABLE IF NOT EXISTS CostIncome(" +
                        "ID_CostIncome INTEGER PRIMARY KEY AUTOINCREMENT," +
                        "CostIncomePrice DOUBLE," +
                        "ID_Category INTEGER REFERENCES Category(ID_Category)," +
                        "CostIncomeDate VARCHAR," +
                        "ID_User INTEGER REFERENCES Users(ID_User));";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category(CategoryName) " +
                        "VALUES (@CategoryName);";
                    command.Parameters.Add("@CategoryName", System.Data.DbType.String).Value = "Транспорт";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category(CategoryName) " +
                        "VALUES (@CategoryName);";
                    command.Parameters.Add("@CategoryName", System.Data.DbType.String).Value = "Еда";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category(CategoryName) " +
                        "VALUES (@CategoryName);";
                    command.Parameters.Add("@CategoryName", System.Data.DbType.String).Value = "Подарки";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category(CategoryName) " +
                          "VALUES (@CategoryName);";
                    command.Parameters.Add("@CategoryName", System.Data.DbType.String).Value = "Здоровье";
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category(CategoryName) " +
                         "VALUES (@CategoryName);";
                    command.Parameters.Add("@CategoryName", System.Data.DbType.String).Value = "Другое";
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public static int UserID;
        public static string UserLogin;
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = "SELECT ID_User FROM Users WHERE Login=@Login AND Password=@Password";
                command.Parameters.Add("@Login", System.Data.DbType.String).Value = loginTextBox.Text;
                command.Parameters.Add("@Password", System.Data.DbType.String).Value = passwordTextBox.Text;
                UserID = Convert.ToInt32(command.ExecuteScalar());
                UserLogin = loginTextBox.Text;
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Некорректно введён логин или пароль. Повторите попытку");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Reg reg = new Reg();
            reg.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}