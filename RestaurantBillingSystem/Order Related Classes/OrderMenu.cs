using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestaurantBillingSystem
{
    //this class models a menu to order from
    [XmlRoot("OrderMenu")]
    public class OrderMenu : IEnumerable<Dish>
    {
        [XmlArray("DishList")]
        [XmlArrayItem("Dish")]
        List<Dish> dishes;

        public List<Dish> Dishes { get => dishes; set => dishes = value; }
        public int Count { get => dishes.Count(); }

        public OrderMenu()
        {
            dishes = new List<Dish>();
        }

        public void Add(Dish d)
        {
            dishes.Add(d);
        }

        public void Remove(Dish d)
        {
            dishes.Remove(d);
        }

        public void Clear()
        {
            dishes.Clear();
        }

        public override string ToString()
        {
            string printOut = string.Empty;

            foreach (Dish d in dishes)
            {
                printOut += "(" + d.DishType + ") " + d.ToString() + "\n";
            }

            return printOut;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Dish>)Dishes).GetEnumerator();
        }

        public IEnumerator<Dish> GetEnumerator()
        {
            return ((IEnumerable<Dish>)dishes).GetEnumerator();
        }
    }
}
