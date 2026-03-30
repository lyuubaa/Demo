using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace ShoesShop.Views
{
    /// <summary>
    /// Логика взаимодействия для OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private User cur_user;
        public OrdersWindow(User user)
        {
            InitializeComponent();
            cur_user = user;
            if (cur_user.UserRole == "Администратор")
            {
                order_menu.Visibility = Visibility.Visible;
                orders_list.MouseDoubleClick += orders_list_MouseDoubleClick;
                add_order_bt.Visibility = Visibility.Visible;
            }
            LoadOrders();
        }
        private void LoadOrders()
        {
            orders_list.Items.Clear();

            var orders = DbShoesContext.Context.Orders
                .Include(s => s.PickupPoint)
                 .OrderByDescending(o => o.OrderDate)
                .ToList();

            foreach (var order in orders)
            {
                orders_list.Items.Add(new UserControls.OrdersUserControl(order));
            }
        }

        private void orders_list_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = orders_list.SelectedItem as UserControls.OrdersUserControl;
            if (selectedItem != null)
            {
                var selectedOrder = selectedItem.DataContext as Order;
                var window = new OrderAddingEditingWindow(cur_user, selectedOrder);
                if (window.ShowDialog() == true)
                {
                    LoadOrders();
                }
            }
        }

        private async void statusMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedControl = orders_list.SelectedItem as UserControls.OrdersUserControl;
                if (selectedControl == null) return;

                var selectedOrder = selectedControl.DataContext as Order;
                if (selectedOrder == null) return;

                string newStatus = selectedOrder.OrderStatus == "Новый" ? "Завершен" : "Новый";

                var result = MessageBox.Show(
                    $"Изменить статус заказа №{selectedOrder.OrderId}?",
                    "Подтверждение изменения",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    selectedOrder.OrderStatus = newStatus;
                    DbShoesContext.Context.SaveChanges();
                    LoadOrders();

                    MessageBox.Show($"Статус успешно изменен на {newStatus}!",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении статуса: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back_bt_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(cur_user).Show();
            Close();
        }

        private void add_order_bt_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderAddingEditingWindow(cur_user);
            if (window.ShowDialog() == true)
            {
                LoadOrders();
            }
        }
    }
}
