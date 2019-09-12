using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    public enum PaymentType
    {
        CreditCard = 0,
        Debit = 1,
        Cash = 2
    }
    public interface ICustomer
    {
        string Name { get; set; }
        PaymentType MethodOfPayment { get; set; }
        Order OrderList { get; set; }
        string CustomerType { get; }
        void OrderItem(Dish d);
        void RemoveItem(Dish d);
        string PrintBill();
        decimal CalcSubtotal();
    }
}
