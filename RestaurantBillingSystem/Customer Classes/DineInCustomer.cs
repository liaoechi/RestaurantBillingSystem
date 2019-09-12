using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    public class DineInCustomer : RegularCustomer, ICustomer
    {
        private decimal tipAmount = 0.00m;

        public decimal TipAmount { get => tipAmount; set => tipAmount = value; }
        public DineInCustomer()
        {
            this.OrderList = new Order();
        }
        public override string PrintBill()
        {
            return "15% or higher tip recommended";
        }
    }
}
