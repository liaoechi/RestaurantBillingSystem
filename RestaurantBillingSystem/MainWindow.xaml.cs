using System;
using System.Collections.Generic;
using System.IO;
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
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace RestaurantBillingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public enum CustomerType
    {
        DineIn = 0,
        Buffet = 1,
        Takeout = 2
    }

    public delegate void CalcDel();

    public partial class MainWindow : Window
    {
        //private List<Table> tables;
        private ObservableCollection<Table> tables = new ObservableCollection<Table>();
        private ObservableCollection<Dish> orderMenu;
        private SalesReport salesHistory = new SalesReport();
        private string pathDishes = @"Data\OrderMenuList.xml";
        private string pathSales = @"Data\SalesHistory.xml";
        private const decimal DiscountRate = 0.90m;
        private const decimal TaxRate = 1.13m;
        private string newCustomerName = string.Empty;
        private decimal newTipInput = 0.00m;

        public ObservableCollection<Table> Tables { get => tables; set => tables = value; }
        public ObservableCollection<Dish> OrderMenu { get => orderMenu; set => orderMenu = value; }
        public SalesReport SalesHistory { get => salesHistory; set => salesHistory = value; }
        public string PathDishes { get => pathDishes; set => pathDishes = value; }
        public string PathSales { get => pathSales; set => pathSales = value; }
        public string NewCustomerName { get => newCustomerName; set => newCustomerName = value; }
        public decimal NewTipInput { get => newTipInput; set => newTipInput = value; }


        //public List<Table> Tables { get => tables; set => tables = value; }

        public MainWindow()
        {
            InitializeComponent();
            string dir = @"Data";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            orderMenu = new ObservableCollection<Dish>();
            LoadOrderMenuFromXML();
            SetUpTables();
            SetupDropDownsAndInputs();
            LoadSalesHistoryFromXML();
            DataContext = this;
        }

        public void SetupDropDownsAndInputs()
        {
            string[] typeOfPaymentList = Enum.GetNames(typeof(PaymentType));
            foreach (string paymentType in typeOfPaymentList)
            {
                PaymentTypeInput.Items.Add(paymentType);
            }

            string[] custTypeList = Enum.GetNames(typeof(CustomerType));
            foreach (string custType in custTypeList)
            {
                CustTypeInput.Items.Add(custType);
            }

            TipAmountInput.Text = "0.00";
        }

        public void SetUpTables()
        {
            Tables.Add(new Table() { TableName = "A2", Capacity = 2 });
            Tables.Add(new Table() { TableName = "A3", Capacity = 3 });
            Tables.Add(new Table() { TableName = "B4", Capacity = 4 });
            Tables.Add(new Table() { TableName = "B5", Capacity = 5 });
            Tables.Add(new Table() { TableName = "B10", Capacity = 10 });
            Tables.Add(new Table() { TableName = "Takeout", Capacity = 100});
        }

        public void LoadOrderMenuFromXML()
        {

            if (File.Exists(PathDishes))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OrderMenu));
                StreamReader reader = new StreamReader(PathDishes);
                OrderMenu masterMenuList = (OrderMenu)serializer.Deserialize(reader);
                reader.Close();
                orderMenu.Clear();
                foreach (Dish d in masterMenuList)
                {
                    orderMenu.Add(d);
                }
            }
        }

        private void MainButton(object sender, RoutedEventArgs e)
        {
            Main.Content = null;
            MainGrid.Visibility = Visibility.Visible;
            LoadOrderMenuFromXML();
        }

        private void DishButton(object sender, RoutedEventArgs e)
        {
            DishMenu dishMenu = new DishMenu();
            Main.Content = dishMenu;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void ReportButton(object sender, RoutedEventArgs e)
        {
            ReportMenu reportMenu = new ReportMenu(SalesHistory);
            Main.Content = reportMenu;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void AddNewCustomer(object sender, RoutedEventArgs e)
        {
            //add new customer to currently selected table
            int i = TableSelection.SelectedIndex;
            ICustomer newCustomer = null;
            CustomerType currentType = (CustomerType)CustTypeInput.SelectedIndex;

            //disable button if validation error occurs
            if(i == -1 || CustNameInput.Text == string.Empty || CustNameInput.ToolTip != null || PaymentTypeInput.SelectedIndex == -1 || CustTypeInput.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter valid a customer profile.");
            }
            else
            {
                //check if table is full
                if (Tables[i].Group.Count < Tables[i].Capacity)
                {
                    switch (currentType)
                    {
                        case CustomerType.DineIn:
                            newCustomer = new DineInCustomer() { Name = CustNameInput.Text, MethodOfPayment = (PaymentType)PaymentTypeInput.SelectedIndex };
                            break;
                        case CustomerType.Buffet:
                            newCustomer = new BuffetCustomer() { Name = CustNameInput.Text, MethodOfPayment = (PaymentType)PaymentTypeInput.SelectedIndex };
                            break;
                        case CustomerType.Takeout:
                            newCustomer = new TakeOutCustomer() { Name = CustNameInput.Text, MethodOfPayment = (PaymentType)PaymentTypeInput.SelectedIndex };
                            break;
                        default:
                            break;
                    }

                    if (currentType == CustomerType.Takeout && Tables[i].TableName != "Takeout")
                    {
                        MessageBox.Show("Takeout customers can only be placed in Takeout table.");
                    }
                    else if (currentType != CustomerType.Takeout && Tables[i].TableName == "Takeout")
                    {
                        MessageBox.Show("Takeout table can only accept takeout customer.");
                    }
                    else
                    {
                        Tables[i].Group.Add(newCustomer);
                        CustNameInput.Text = string.Empty;
                        PaymentTypeInput.SelectedIndex = -1;
                        CustTypeInput.SelectedIndex = -1;
                        CustomerSelection.Items.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show($"Table {Tables[i].TableName} is full, try another table.");
                }
            }
        }

        private void OrderButton(object sender, RoutedEventArgs e)
        {
            int selectedTable = TableSelection.SelectedIndex;
            int selectedPerson = CustomerSelection.SelectedIndex;

            if(selectedPerson == -1)
            {
                MessageBox.Show("Please select or highlight a customer first before ordering.");
            }

            if (SearchDishesGrid.SelectedItem != null && SearchDishesGrid.SelectedItem is Dish)
            {
                Dish currentDish = (Dish)SearchDishesGrid.SelectedItem;
                //shallow copy
                Dish d = new Dish() { DishName = currentDish.DishName, DishType = currentDish.DishType, Cost = currentDish.Cost };
                //need to check index here
                if(selectedTable != -1 && selectedPerson != -1)
                {
                    Tables[selectedTable].Group[selectedPerson].OrderItem(d);
                    CustOrderedGrid.Items.Refresh();
                }
            }
        }

        private void RemoveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedTable = TableSelection.SelectedIndex;
            int selectedPerson = CustomerSelection.SelectedIndex;
            int selectedOrder= CustOrderedGrid.SelectedIndex;

            //MessageBox.Show($"{selectedTable}, {selectedPerson}, {selectedOrder}");

            if (selectedPerson != -1 && SearchDishesGrid.SelectedItem is Dish)
            {
                Dish currentDish = (Dish)CustOrderedGrid.SelectedItem;
                Tables[selectedTable].Group[selectedPerson].RemoveItem(currentDish);
                CustOrderedGrid.Items.Refresh();
                //RecalculateSubTotal();
            }
        }

        private void RemoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            int selectedTable = TableSelection.SelectedIndex;
            int selectedPerson = CustomerSelection.SelectedIndex;

            if(selectedPerson != -1)
            {
                ICustomer currentCustomer = (ICustomer)CustomerSelection.SelectedItem;
                Tables[selectedTable].Group[selectedPerson].OrderList.Clear();
                Tables[selectedTable].Group.Remove(currentCustomer);
                CustomerSelection.Items.Refresh();
                CustOrderedGrid.Items.Refresh();
            }
        }

        private void RecalculateSubTotal()
        {
            decimal subtotal = 0;

            if (CustOrderedGrid.Items.Count > 0)
            {
                foreach (Dish row in CustOrderedGrid.ItemsSource)
                {
                    subtotal += row.Cost;
                }
                SubtotalDisplay.Content = $"${subtotal}";
                TotalDisplay.Content = $"$ {Math.Round(subtotal * TaxRate,2)}";
            }
            else
            {
                SubtotalDisplay.Content = string.Empty; ;
                TotalDisplay.Content = string.Empty;
            }
        }

        private void UpdateInvoice()
        {
            int selectedTable = TableSelection.SelectedIndex;
            int selectedPerson = CustomerSelection.SelectedIndex;
            string msg = "";

            //CustOrderedGrid.Items.Refresh();
            if (selectedTable != -1 && selectedPerson != -1 && selectedPerson < Tables[selectedTable].Group.Count)
            {
                //use of LINQ to group by Dish Name to get count of each dish
                //need to check for index out of bound
                var lineItem = from dish in Tables[selectedTable].Group[selectedPerson].OrderList
                               group dish by new { dish.DishName, dish.Cost } into duplicateDish
                               select new
                               {
                                   DishName = duplicateDish.Key.DishName,
                                   Price = duplicateDish.Key.Cost,
                                   Qty = duplicateDish.Count()
                               };

                foreach(var line in lineItem)
                {
                    msg += $"{line.DishName} {line.Qty} @ ${line.Price} = ${Math.Round(line.Qty * line.Price,2)}\n";
                }
            }

            LineItemsDisplay.Text = msg;
        }

        private void CustOrderedGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            RecalculateSubTotal();
            UpdateInvoice();
        }

        private void CustOrderedGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            RecalculateSubTotal();
            UpdateInvoice();
        }

        private void SaveBill_ButtonClick(object sender, RoutedEventArgs e)
        {
            //need to catch no customer selected
            if( TableSelection.SelectedIndex > -1 && CustomerSelection.SelectedIndex > -1 && 
                CustOrderedGrid.Items.Count > 0 &&
                DatePicker.SelectedDate != null && TipAmountInput.ToolTip == null)
            {
                Table currentTable = Tables[TableSelection.SelectedIndex];
                ICustomer currentCust = Tables[TableSelection.SelectedIndex].Group[CustomerSelection.SelectedIndex];
                //already sorted by invoice number descending order, index 0 should have most recent invoice number
                int newInvoiceNum = SalesHistory.Bills.Count == 0 ? 0 : (SalesHistory.Bills[0].InvoiceNum + 1);
                Bill newBill = new Bill()
                {
                    Date = DatePicker.SelectedDate.Value,
                    InvoiceNum = newInvoiceNum,
                    TableName = currentTable.TableName,
                    CustName = currentCust.Name,
                    CustType = currentCust.CustomerType,
                    PaymentType = currentCust.MethodOfPayment,
                    TotalAmt = Math.Round(currentCust.CalcSubtotal() * TaxRate, 2),
                    TipAmount = decimal.Parse(TipAmountInput.Text)
                };

                //Timestamp selected date with current time
                DateTime now = DateTime.Now;
                DateTime timeStampedDate = newBill.Date.Add(new TimeSpan(now.Hour, now.Minute, now.Second));
                newBill.Date = timeStampedDate;

                //Sort bills by descending order based on invoice number
                SalesHistory.Bills.Add(newBill);
                SalesHistory.Bills.Sort();
                SalesHistoryWriteToXML();
                currentCust.OrderList.Clear();
                currentTable.Group.Remove(currentCust);
                CustomerSelection.Items.Refresh();
                CustOrderedGrid.Items.Refresh();
                TipAmountInput.Text = "0.00";
            }
            else
            {
                MessageBox.Show("A valid date is required and order must be greater than 0.");
            }
        }

        private void PrintBill_ButtonClick(object sender, RoutedEventArgs e)
        {
            if(DatePicker.SelectedDate == null)
            {
                MessageBox.Show("The date must be selected for the bill.");
            }
            else if (CustomerSelection.SelectedIndex == -1)
            {
                MessageBox.Show("There is no customer selected.");
            }
            else if (CustOrderedGrid.Items.Count < 1)
            {
                MessageBox.Show("The customer has ordered nothing.");
            }
            else if(TipAmountInput.ToolTip != null)
            {
                MessageBox.Show("Invalid tip input.");
            }
            else
            {
                ICustomer currentCust = Tables[TableSelection.SelectedIndex].Group[CustomerSelection.SelectedIndex];
                CommentOutput.Text = currentCust.PrintBill();
                MessageBox.Show("Bill printed.");
            }
        }

        private void SalesHistoryWriteToXML()
        {
            if (SalesHistory.Bills.Count > 0)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SalesReport));
                TextWriter writer = new StreamWriter(PathSales);
                serializer.Serialize(writer, SalesHistory);
                writer.Close();
            }
        }

        private void LoadSalesHistoryFromXML()
        {
            if (File.Exists(PathSales))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SalesReport));
                StreamReader reader = new StreamReader(PathSales);
                SalesHistory = (SalesReport)serializer.Deserialize(reader);
                reader.Close();
            }
        }

        private void CustomerSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommentOutput.Text = string.Empty;
        }
    }
}
