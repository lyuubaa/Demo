using ShoesShop.Models;
using System;
using System.Collections.Generic;
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

namespace ShoesShop.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrdersUserControl.xaml
    /// </summary>
    public partial class OrdersUserControl : UserControl
    {
        private Order cur_order;
        public OrdersUserControl(Order order)
        {
            InitializeComponent();
            DataContext = order;
            deliveryDate_txt.Text = $"{order.DeliveryDate.Day}.{order.DeliveryDate.Month}.{order.DeliveryDate.Year}";
            orderDate_txt.Text = $"{order.OrderDate.Day}.{order.OrderDate.Month}.{order.OrderDate.Year}";
            adr.Text = order.PickupPoint.DisplayAddress.ToString() ;
        }
    }
}
