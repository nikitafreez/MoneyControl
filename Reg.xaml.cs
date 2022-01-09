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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader reader;
        private string dbFileName = "MoneyStat.db";

        public Reg()
        {
            InitializeComponent();
            string connectionString = $"Data Source={dbFileName};Version=3";
            connection = new SQLiteConnection(connectionString);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isLoginOK = false;
            bool isPasswordOK = false;
            bool isPasswordSimilarOK = false;

            if (!string.IsNullOrEmpty(loginTextBox.Text) && loginTextBox.Text.Length > 3 && loginTextBox.Text.Length < 13)
            {
                isLoginOK = true;
            }
            if (!string.IsNullOrEmpty(passwordTextBox.Text) && passwordTextBox.Text.Length > 5 && passwordTextBox.Text.Length < 26 && passwordTextBox.Text.Any(c => !char.IsLetterOrDigit(c)))
            {
                isPasswordOK = true;
            }
            if (passwordTextBox.Text == passwordTextBoxCheck.Text)
            {
                isPasswordSimilarOK = true;
            }

            if (isLoginOK && isPasswordOK && isPasswordSimilarOK)
            {
                try
                {
                    connection.Open();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Users(Login, Password, TotalMoney)" +
                        "VALUES (@Login, @Password, @TotalMoney);";
                    command.Parameters.Add("@Login", System.Data.DbType.String).Value = loginTextBox.Text;
                    command.Parameters.Add("@Password", System.Data.DbType.String).Value = passwordTextBox.Text;
                    command.Parameters.Add("@TotalMoney", System.Data.DbType.Int32).Value = 0;
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
                this.Close();
            }
            else if (isLoginOK == false)
            {
                MessageBox.Show("Логин должен содержать от 4 до 12 символов. Повторите попытку.");
            }
            else if (isPasswordOK == false)
            {
                MessageBox.Show("Пароль должен содержать от 6 до 25 символов, а также содержать минимум один спец. символ: !@#$%^&*№?");
            }
            else if (isPasswordSimilarOK == false)
            {
                MessageBox.Show("Пароли не совпадают");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Auth auth = new Auth();
            auth.Show();
        }
    }
}
