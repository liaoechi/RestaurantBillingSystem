using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantBillingSystem
{
    public class TakeOutCustomer : RegularCustomer, ICustomer
    {
       
        private DateTime pickupTime; //format is TimeSpan(h, m, 00); hour must be between 11 and 22, minute must be between 0 and 60
        public DateTime PickupTime { get => pickupTime; set => pickupTime = value; }

        public TakeOutCustomer()
        {
            this.OrderList = new Order();
        }

        public override string PrintBill()
        {
            if (MethodOfPayment == PaymentType.Cash)
            {
                return "10% discount for cash payment";
            }
            else
                return "10% discount if paid by cash";
        }

        public override decimal CalcSubtotal()
        {
            decimal subtotal = 0;
            foreach (Dish d in OrderList)
            {
                subtotal += d.Cost;
            }
            return MethodOfPayment == PaymentType.Cash ? (Math.Round(subtotal * 0.90m,2)) : subtotal;
        }
    }
}
