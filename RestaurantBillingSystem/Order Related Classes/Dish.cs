using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBillingSystem
{
    public enum TypeOfDish
    {
        Main = 0,
        Appetizer = 1,
        Drink = 3,
        Dessert = 4
    }
    //this is the class to represent each dish on the menu
    public class Dish : IEquatable<Dish>
    {
        private string dishName;
        private TypeOfDish dishType;
        private decimal cost;

        public string DishName { get => dishName; set => dishName = value; }
        public TypeOfDish DishType { get => dishType; set => dishType = value; }
        public decimal Cost { get => cost; set => cost = value; }

        public bool Equals(Dish other)
        {
            return this.dishName.Equals(other.DishName);
        }

        public override string ToString()
        {
            return dishName + " " + cost;
        }
    }
}
