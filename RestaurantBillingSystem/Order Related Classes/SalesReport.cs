using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestaurantBillingSystem
{
    [XmlRoot("SalesReport")]
    [XmlInclude(typeof(DineInCustomer))]
    [XmlInclude(typeof(BuffetCustomer))]
    [XmlInclude(typeof(TakeOutCustomer))]
    public class SalesReport
    {
        [XmlArray("SalesHistory")]
        [XmlArrayItem("Bill")]
        private List<Bill> bills;
        private decimal cumulativeTipAmt = 0.00m;
        private decimal cumulativeSalesAmt = 0.00m;
        private decimal cumulativeTaxAmt = 0.00m;

        [XmlIgnore]
        private CalcDel calcAll;
        [XmlIgnore]
        public const decimal TaxRate = 1.13m;

        public List<Bill> Bills { get => bills; set => bills = value; }
        [XmlIgnore]
        public CalcDel CalcAll { get => calcAll; set => calcAll = value; }
        public decimal CumulativeTipAmt { get => cumulativeTipAmt; set => cumulativeTipAmt = value; }
        public decimal CumulativeSalesAmt { get => cumulativeSalesAmt; set => cumulativeSalesAmt = value; }
        public decimal CumulativeTaxAmt { get => cumulativeTaxAmt; set => cumulativeTaxAmt = value; }
        

        public SalesReport()
        {
            bills = new List<Bill>();
            calcAll = CalcTotalTips;
            calcAll += CalcTotalSales;
            calcAll += CalcTotalTaxes;
        }

        public void CalcTotalTips()
        {
            cumulativeTipAmt = 0;
            foreach(Bill b in Bills)
            {
                cumulativeTipAmt += b.TipAmount;
            }
        }

        public void CalcTotalSales()
        {
            cumulativeSalesAmt = 0;
            foreach (Bill b in Bills)
            {
                cumulativeSalesAmt += b.TotalAmt;
            }
        }

        public void CalcTotalTaxes()
        {
            cumulativeTaxAmt = 0;
            foreach (Bill b in Bills)
            {
                cumulativeTaxAmt += Math.Round((b.TotalAmt / TaxRate) * (TaxRate - 1), 2); //backwards calculation because TotalAmt includes tax
            }
        }

        public void SetAllCumulativeTotals()
        {
            calcAll();
        }
    }
}
