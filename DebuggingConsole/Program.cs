using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RestaurantBillingSystem;

namespace DebuggingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Go();
            Console.ReadKey();
        }

        public void Go()
        {
            OrderMenu newMenu = null;

            //OrderMenu newMenu = new OrderMenu();
            //newMenu.AddDish(new Dish { DishName = "Tuna Sandwich", DishType = "Entrée", Cost = 5.99m});
            //newMenu.AddDish(new Dish { DishName = "BBQ Sandwich", DishType = "Entrée", Cost = 7.99m });
            //newMenu.AddDish(new Dish { DishName = "Steak Sandwich", DishType = "Entrée", Cost = 9.99m });

            //Console.Write(newMenu.ToString());
            //Console.WriteLine(newMenu.Count);

            //create a folder for the data
            string dir = @"Data";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            ////generate a xml data file for dishes
            //XmlSerializer serializer = new XmlSerializer(typeof(OrderMenu));
            //TextWriter writer = new StreamWriter(@"Data\OrderMenuList.xml");
            //serializer.Serialize(writer, newMenu);
            //writer.Close();

            //read from xml file
            string path = @"Data\OrderMenuList.xml";

            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OrderMenu));
                StreamReader reader = new StreamReader(path);
                newMenu = (OrderMenu)serializer.Deserialize(reader);
                reader.Close();
            }

            Console.Write(newMenu.ToString());
            Console.WriteLine(newMenu.Count);
        }
    }
}
