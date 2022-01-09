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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using System.Data;
using LiveCharts;
using LiveCharts.Wpf;
using Excel = Microsoft.Office.Interop.Excel;

namespace MoneyControl
{

    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        private SQLiteDataReader reader;
        private SQLiteDataAdapter adapter;
        private DataSet dataSet;
        private DataTable dataTable;
        private string dbFileName = "MoneyStat.db";

        private double currentCurrency = 1.00;
        private string currentCurrencyMark = "₽";

        private DateTime dayAgo = DateTime.Now.AddDays(-1);
        private DateTime monthAgo = DateTime.Now.AddMonths(-1);
        private DateTime yearAgo = DateTime.Now.AddYears(-1);
        private DateTime allTime = DateTime.Today.AddYears(-(Convert.ToInt32(DateTime.Now.Year) - 1));
        private static DateTime choosedTime;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = $"Data Source={dbFileName};Version=3";
            connection = new SQLiteConnection(connectionString);
            GetCombo();
            GetPie();
            GetTotalMoney();
            helloLabel.Content = $"Личный кабинет пользователя {Auth.UserLogin}";
        }
        private void GetCombo()
        {
            try
            {
                connection.Open();
                adapter = new SQLiteDataAdapter("SELECT * FROM Category", connection);
                dataSet = new DataSet();
                adapter.Fill(dataSet, "Categories");
                categoryComboBox.ItemsSource = dataSet.Tables[0].DefaultView;
                categoryComboBox.DisplayMemberPath = dataSet.Tables["Categories"].Columns[1].ToString();
                categoryComboBox.SelectedValuePath = dataSet.Tables["Categories"].Columns[0].ToString();

                categoryComboBox2.ItemsSource = dataSet.Tables[0].DefaultView;
                categoryComboBox2.DisplayMemberPath = dataSet.Tables["Categories"].Columns[1].ToString();
                categoryComboBox2.SelectedValuePath = dataSet.Tables["Categories"].Columns[0].ToString();
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
        private void GetPie()
        {
            pieChart.Series.Clear();
            double totalCategoryPrice = 0;
            try
            {
                connection.Open();
                int comboItemCount = categoryComboBox.Items.Count;

                for (int i = 0; i < comboItemCount; i++)
                {
                    categoryComboBox.SelectedIndex = i;
                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = $"SELECT CostIncomePrice FROM CostIncome WHERE CostIncomeDate BETWEEN '{choosedTime.ToString("yyyy-MM-dd")}' AND '{DateTime.Now.ToString("yyyy-MM-dd")}' AND ID_Category = @ID_Category AND ID_User = @ID_User;";
                    command.Parameters.Add("@ID_Category", DbType.Int32).Value = categoryComboBox.SelectedValue;
                    command.Parameters.Add("@ID_User", DbType.Int32).Value = Auth.UserID;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        totalCategoryPrice += (Convert.ToDouble(reader["CostIncomePrice"].ToString()));
                    }

                    pieChart.Series.Add(new PieSeries { Title = $"{dataSet.Tables[0].Rows[i][1]}", StrokeThickness = 0, Values = new ChartValues<double> {Math.Round(totalCategoryPrice / currentCurrency, 2) } });
                    totalCategoryPrice = 0;
                }
                categoryComboBox.SelectedIndex = 0;
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
        public static double totalMoney;
        private void GetTotalMoney()
        {
            try
            {
                connection.Open();

                command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT TotalMoney FROM Users WHERE ID_User = '{Auth.UserID}'";
                totalMoney = (Convert.ToDouble(command.ExecuteScalar()));
                totalMoneyLabel.Content = Math.Round(totalMoney / currentCurrency, 2) + currentCurrencyMark;
                if (totalMoney < 1)
                {
                    totalMoneyLabel.Foreground = Brushes.Red;
                }
                else
                {
                    totalMoneyLabel.Foreground = Brushes.Green;
                }

                double monthCosts = 0;
                command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT CostIncomePrice FROM CostIncome WHERE CostIncomeDate BETWEEN '{monthAgo}' AND '{DateTime.Now.ToString("yyyy-MM-dd")}' AND ID_User = '{Auth.UserID}';";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    monthCosts += Math.Round((Convert.ToDouble(reader["CostIncomePrice"])), 2);
                }
                monthCostsLabel.Content = $"Трат в этом месяце: {Math.Round(monthCosts / currentCurrency, 2)} {currentCurrencyMark}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            currencyLabel1.Content = currentCurrencyMark;
            currencyLabel2.Content = currentCurrencyMark;
        }

        private void addCostButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(categoryComboBox.Text) && costTextBox.Text.Length > 0)
            {
                try
                {
                    connection.Open();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO CostIncome(CostIncomePrice, ID_Category, CostIncomeDate, ID_User)" +
                        "VALUES (@CostIncomePrice, @ID_Category, @CostIncomeDate, @ID_User)";
                    command.Parameters.Add("@CostIncomePrice", DbType.Double).Value = Math.Round((Convert.ToDouble(costTextBox.Text) * currentCurrency), 2);
                    command.Parameters.Add("@ID_Category", DbType.Int32).Value = Convert.ToInt32(categoryComboBox.SelectedValue);
                    command.Parameters.Add("@CostIncomeDate", DbType.String).Value = DateTime.Now.ToString("yyyy-MM-dd");
                    command.Parameters.Add("@ID_User", DbType.Int32).Value = Auth.UserID;
                    command.ExecuteNonQuery();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = $"UPDATE Users SET TotalMoney=@TotalMoney WHERE ID_User={Auth.UserID}";
                    totalMoney -= Convert.ToDouble(costTextBox.Text) * currentCurrency;
                    command.Parameters.Add("@TotalMoney", DbType.Double).Value = totalMoney;
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
                GetPie();
                GetTotalMoney();
            }
            else
            {
                MessageBox.Show("Неверно введены данные. Повторите попытку.");
            }
            costTextBox.Text = "";
        }

        private void updateTotalMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = $"UPDATE Users SET TotalMoney=@TotalMoney WHERE ID_User={Auth.UserID}";
                command.Parameters.Add("@TotalMoney", DbType.Double).Value = Math.Round(Convert.ToDouble(totalMoneyTextBox.Text) * currentCurrency, 2);
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
            GetTotalMoney();
            totalMoneyTextBox.Text = "";
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (newCategoryTextBox.Text.Length >= 2 && newCategoryTextBox.Text.Length < 20)
            {
                try
                {
                    connection.Open();

                    command = new SQLiteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Category(CategoryName)" +
                        "VALUES (@CategoryName);";
                    command.Parameters.Add("@CategoryName", DbType.String).Value = newCategoryTextBox.Text;
                    command.ExecuteNonQuery();
                    MessageBox.Show($"Категория \"{newCategoryTextBox.Text}\" была успешно добавлена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                GetCombo();
                GetPie();
            }
            else
            {
                MessageBox.Show("Недопустимое название категории");
            }
            newCategoryTextBox.Text = "";
        }

        private void deleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();

                command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM Category WHERE ID_Category = '{Convert.ToInt32(categoryComboBox2.SelectedValue)}';";
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
            GetCombo();
            GetPie();
        }


        public class CostsIncome
        {
            public string CategoryName { get; set; }
            public double CostIncomePrice { get; set; }
            public string CostIncomeDate { get; set; }
        }

        static void DisplayInExcelCostIncome(IEnumerable<CostsIncome> costs)
        {
            var excelApp = new Excel.Application();
            excelApp.Visible = true;

            excelApp.Workbooks.Add();

            Excel._Worksheet worksheet = (Excel.Worksheet)excelApp.ActiveSheet;

            worksheet.Cells[1, "A"] = "Категория";
            worksheet.Cells[1, "B"] = "Размер траты";
            worksheet.Cells[1, "C"] = "Дата траты";

            var row = 1;
            foreach (var cost in costs)
            {
                row++;
                worksheet.Cells[row, "A"] = cost.CategoryName;
                worksheet.Cells[row, "B"] = cost.CostIncomePrice;
                worksheet.Cells[row, "C"] = cost.CostIncomeDate;
            }

            worksheet.Columns[1].AutoFit();
            worksheet.Columns[2].AutoFit();
            worksheet.Columns[3].AutoFit();

        }
        private void toExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var CostIncomeList = new List<CostsIncome>();
            try
            {
                connection.Open();

                command = new SQLiteCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM CostIncome " +
                    $"INNER JOIN Category ON CostIncome.ID_Category = Category.ID_Category  WHERE ID_User={Auth.UserID} ;";
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CostIncomeList.Add(new CostsIncome
                    {
                        CategoryName = reader["CategoryName"].ToString(),
                        CostIncomePrice = Math.Round(Convert.ToDouble(reader["CostIncomePrice"].ToString()) / currentCurrency, 2),
                        CostIncomeDate = reader["CostIncomeDate"].ToString(),
                    });
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
            DisplayInExcelCostIncome(CostIncomeList);
        }
        private class Currency
        {
            public static double RUB = 1.00000;
            public static double USD = 74.1373;
            public static double EUR = 89.506;
            public static double JPY = 0.6793;
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(RubCurrenceRadioButton.IsChecked == true)
            {
                currencyComboBox.Text = "Рубль | RUB | ₽";
                currentCurrency = Currency.RUB;
                currentCurrencyMark = "₽";
            }
            else if(UsdCurrenceRadioButton.IsChecked == true)
            {
                currencyComboBox.Text = "Доллар | USD | $";
                currentCurrency = Currency.USD;
                currentCurrencyMark = "$";
            }
            else if(EurCurrenceRadioButton.IsChecked == true)
            {
                currencyComboBox.Text = "Евро | EUR | €";
                currentCurrency = Currency.EUR;
                currentCurrencyMark = "€";
            }
            else if(JpyCurrenceRadioButton.IsChecked == true)
            {
                currencyComboBox.Text = "Йена | JPY | ¥";
                currentCurrency = Currency.JPY;
                currentCurrencyMark = "¥";
            }
            GetTotalMoney();
            GetPie();
        }
        private static bool isExtaExit = true;
        private void exitAccountButton_Click(object sender, RoutedEventArgs e)
        {
            isExtaExit = false;
            Auth auth = new Auth();
            auth.Show();
            this.Close();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (isExtaExit == true)
            {
                Application.Current.Shutdown();
            }
            isExtaExit = true;
        }

        private void ChooseTimeDay_Click(object sender, RoutedEventArgs e)
        {
            choosedTime = dayAgo;
            GetPie();
        }

        private void ChooseTimeMonth_Click(object sender, RoutedEventArgs e)
        {
            choosedTime = monthAgo;
            GetPie();
        }

        private void ChooseTimeYear_Click(object sender, RoutedEventArgs e)
        {
            choosedTime = yearAgo;
            GetPie();
        }

        private void ChooseTimeAll_Click(object sender, RoutedEventArgs e)
        {
            choosedTime = new DateTime();
            GetPie();
        }
    }
}
