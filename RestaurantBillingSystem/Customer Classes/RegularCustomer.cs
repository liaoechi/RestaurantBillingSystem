using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    public abstract class RegularCustomer : ICustomer
    {
        private string name;
        private PaymentType methodOfPayment;
        private Order orderList;

        public string Name { get => name; set => name = value; }
        public PaymentType MethodOfPayment { get => methodOfPayment; set => methodOfPayment = value; }
        public Order OrderList { get => orderList; set => orderList = value; }
        public string CustomerType { get => GetCustomerType(); }

        public virtual void OrderItem(Dish d)
        {
            orderList.Add(d);
        }

        public virtual void RemoveItem(Dish d)
        {
            orderList.Remove(d);
        }

        public virtual string GetCustomerType()
        {
            string remove = "Customer";
            string[] getTypeArray = this.GetType().ToString().Split('.');
            return getTypeArray[1].Replace(remove, string.Empty);
        }

        //inheriting class needs to implement this
        public abstract string PrintBill();

        public virtual decimal CalcSubtotal()
        {
            return OrderList.GetOrderTotal();
        }
    }
}
