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

namespace RestaurantBillingSystem
{
    /// <summary>
    /// Interaction logic for ReportMenu.xaml
    /// </summary>
    public partial class ReportMenu : Page
    {
        SalesReport salesHistory;
        public const decimal TaxRate = 1.13m;

        public SalesReport SalesHistory { get => salesHistory; set => salesHistory = value; }

        public ReportMenu(SalesReport salesHistory)
        {
            InitializeComponent();
            this.SalesHistory = salesHistory;
            SalesHistory.SetAllCumulativeTotals();
            DataContext = this.SalesHistory;
        }

        private void GenerateTotals()
        {
            decimal salesTotal = 0;
            decimal taxTotal = 0;
            decimal tipsTotal = 0;

            if (SalesReportGrid.Items.Count > 0)
            {
                foreach (Bill row in SalesReportGrid.ItemsSource)
                {
                    salesTotal += row.TotalAmt;
                    tipsTotal += row.TipAmount;
                }
                taxTotal = Math.Round((salesTotal/TaxRate) * (TaxRate - 1), 2);
                CurrentSalesOutput.Text = $"${salesTotal}";
                CurrentTipsOutput.Text= $"${tipsTotal}";
                CurrentTaxOutput.Text = $"${taxTotal}";
            }
            else
            {
                CurrentSalesOutput.Text = string.Empty;
                CurrentTipsOutput.Text = string.Empty;
                CurrentTaxOutput.Text = string.Empty;
            }
        }

        private void SalesReportGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            GenerateTotals();
        }

        private void SalesReportGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            GenerateTotals();
        }

        private void StartDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            RunDateQuery();
        }

        private void EndDatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            RunDateQuery();
        }

        public void RunDateQuery()
        {
            IEnumerable<Bill> datequery = null;

       
            //inclusive date query for date range. We use .Date field from DateTime to only compare the short date and time.
            if (StartDatePicker.SelectedDate != null)
            {

                datequery = from bill in SalesHistory.Bills
                            where DateTime.Compare(bill.Date.Date, StartDatePicker.SelectedDate.Value.Date) >= 0
                            select bill;

                if (EndDatePicker.SelectedDate != null)
                {
                    datequery = from bill in datequery
                                where DateTime.Compare(bill.Date.Date, EndDatePicker.SelectedDate.Value.Date) <= 0
                                select bill;
                }
            }
            else
            {
                if (EndDatePicker.SelectedDate != null)
                {
                    datequery = from bill in SalesHistory.Bills
                                where DateTime.Compare(bill.Date.Date, EndDatePicker.SelectedDate.Value.Date) <= 0
                                select bill;
                }
            }

            if(datequery != null)
            {
                SalesReportGrid.ItemsSource = datequery;
            }
        }

        private void ClearFilter_Button(object sender, RoutedEventArgs e)
        {
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            SalesReportGrid.ItemsSource = SalesHistory.Bills;
        }
    }
}
