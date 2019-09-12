using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    //this class models an order to be used for a bill
    public class Order : OrderMenu
    {
        //need attribute for icustomer
        public decimal OrderTotal { get => GetOrderTotal(); }
        //public string CustomerName { get => customerName; set => customerName = value; }

        public Order()
        {
            Dishes = new List<Dish>();
        }

        public decimal GetOrderTotal()
        {
            decimal total = 0;
            foreach(Dish d in this.Dishes)
            {
                total += d.Cost;
            }
            return total;
        }
    }
}
