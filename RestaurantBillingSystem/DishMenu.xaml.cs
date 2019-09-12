using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace RestaurantBillingSystem
{
    /// <summary>
    /// Interaction logic for DishMenu.xaml
    /// </summary>
    ///

    public partial class DishMenu : Page
    {
        private string path = @"Data\OrderMenuList.xml";
        private ObservableCollection<Dish> displayMenuList;
        private OrderMenu masterMenuList;
        private decimal newCost;
        private string newDishName = string.Empty;

        public ObservableCollection<Dish> DisplayMenuList { get => displayMenuList; set => displayMenuList = value; }
        public OrderMenu MasterMenuList { get => masterMenuList; set => masterMenuList = value; }
        public string Path { get => path; set => path = value; }
        public decimal NewCost { get => newCost; set => newCost = value; }
        public string NewDishName { get => newDishName; set => newDishName = value; }

        public DishMenu()
        {
            InitializeComponent();
            DisplayMenuList = new ObservableCollection<Dish>();
            DataContext = this;
            ReadFromXML();
            SetupDishTypeList();
            //Validation.AddErrorHandler(this.NewDishCostInput, Display_ValidationError);
            //Validation.AddErrorHandler(this.NewDishNameInput, Display_ValidationError);
        }

        //protected void Display_ValidationError(object sender,
        //ValidationErrorEventArgs e)
        //{
        //    MessageBox.Show((string)e.Error.ErrorContent);
        //}

        public void SetupDishTypeList()
        {
            string[] dishTypeList = Enum.GetNames(typeof(TypeOfDish));
            foreach (string dishType in dishTypeList)
            {
                NewDishTypeInput.Items.Add(dishType);
            }
        }

        public void ReadFromXML()
        {

            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OrderMenu));
                StreamReader reader = new StreamReader(path);
                MasterMenuList = (OrderMenu)serializer.Deserialize(reader);
                reader.Close();
                foreach (Dish d in MasterMenuList)
                {
                    DisplayMenuList.Add(d);
                }
                //MessageBox.Show(DisplayMenuList.Count.ToString());
            }
            else
            {
                MasterMenuList = new OrderMenu();
            }
        }

        public void WriteToXML()
        {
            //write ClientList into an Xml file
            if (DisplayMenuList.Count > 0)
            {
                MasterMenuList.Clear();
                foreach (Dish d in DisplayMenuList)
                {
                    MasterMenuList.Add(d);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(OrderMenu));
                TextWriter writer = new StreamWriter(Path);
                serializer.Serialize(writer, MasterMenuList);
                writer.Close();
                ClearInputs();
            }
        }

        private void AddDishButton(object sender, RoutedEventArgs e)
        {
            //if no tooltip errors, add new dish
            if (NewDishNameInput.ToolTip == null && NewDishCostInput.ToolTip == null && NewDishTypeInput.SelectedIndex != -1)
            {
                DisplayMenuList.Add(new Dish { DishName = NewDishName, DishType = (TypeOfDish)Enum.Parse(typeof(TypeOfDish), NewDishTypeInput.Text), Cost = NewCost });
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Please provide valid inputs.");
            }
        }

        private void ClearInputs()
        {
            NewDishNameInput.Text = string.Empty;
            NewDishTypeInput.SelectedIndex = -1;
            NewDishCostInput.Text = string.Empty;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            WriteToXML();
            MessageBox.Show("Save successful");
        }
    }
}
