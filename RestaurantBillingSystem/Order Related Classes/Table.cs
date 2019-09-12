using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    //this class models the dinning tables in the restaurant
    //the table acts like a container for customers
    public class Table
    {
        private string tableName;
        private int capacity;
        private List<ICustomer> group;

        public string TableName { get => tableName; set => tableName = value; }
        public int Capacity { get => capacity; set => capacity = value; }
        public List<ICustomer> Group { get => group; set => group = value; }

        public Table()
        {
            group = new List<ICustomer>();
        }

        public override string ToString()
        {
            return TableName + " - " + Capacity.ToString();
        }
    }
}
