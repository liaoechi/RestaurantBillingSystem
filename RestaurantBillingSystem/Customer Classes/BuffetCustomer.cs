using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    public class BuffetCustomer : RegularCustomer, ICustomer
    {
        private decimal tipAmount = 0.00m;
        private TimeSpan allowedTime; //format is TimeSpan(h, m, 00) to set max allowed time of buffet.

        public decimal TipAmount { get => tipAmount; set => tipAmount = value; }
        public TimeSpan AllowedTime { get => allowedTime; set => allowedTime = value; }

        public BuffetCustomer()
        {
            this.OrderList = new Order();
        }

        public override string PrintBill()
        {
            return "10% tip recommended";
        }
    }
}
