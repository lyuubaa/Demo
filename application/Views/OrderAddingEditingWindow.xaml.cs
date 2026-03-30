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
    /// Логика взаимодействия для OrderAddingEditingWindow.xaml
    /// </summary>
    public partial class OrderAddingEditingWindow : Window
    {
        private Order currentOrder;
        private User cur_user;
        public OrderAddingEditingWindow(User user, Order order = null)
        {
            InitializeComponent();
            cur_user = user;
            LoadPickupPoints();

            if (order != null)
            {
                currentOrder = order;
                LoadOrderData();
                Title = "Редактирование заказа";
            }
            else
            {
                currentOrder = new Order();
                Title = "Добавление заказа";
                window_name_txt.Text = "ДОБАВЛЕНИЕ";
                orderNumber_tb.Visibility = Visibility.Collapsed;
                orderNumber_txt.Visibility = Visibility.Collapsed;
                orderDate_dp.SelectedDate = DateTime.Today;
                orderDate_dp.DisplayDateStart = DateTime.Today;
                orderDate_dp.DisplayDateEnd = DateTime.Today;

                issueDate_dp.DisplayDateStart = DateTime.Today;
            }
        }

        private void LoadPickupPoints()
        {
            pickup_points_cb.ItemsSource = DbShoesContext.Context.PickupPoints.ToList();
            pickup_points_cb.DisplayMemberPath = "DisplayAddress";
        }

        private void LoadOrderData()
        {
            if (currentOrder == null) return;

            orderNumber_txt.Text = currentOrder.OrderId.ToString();
            orderNumber_txt.IsEnabled = false;

            foreach (ComboBoxItem item in status_cb.Items)
            {
                if (item.Content.ToString() == currentOrder.OrderStatus)
                {
                    status_cb.SelectedItem = item;
                    break;
                }
            }

            pickup_points_cb.SelectedItem = currentOrder.PickupPoint;

            orderDate_dp.SelectedDate = currentOrder.OrderDate.ToDateTime(TimeOnly.MinValue);
            issueDate_dp.SelectedDate = currentOrder.DeliveryDate.ToDateTime(TimeOnly.MinValue);

            orderDate_dp.IsEnabled = false;
            issueDate_dp.IsEnabled = false;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pickup_points_cb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите пункт выдачи!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (status_cb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите статус заказа!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (orderDate_dp.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату заказа!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (issueDate_dp.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату выдачи!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (currentOrder.OrderId == 0)
                {
                    currentOrder.ClientId = cur_user.UserId;
                    currentOrder.CodeToReceive = currentOrder.CodeToReceive = new Random().Next(0, 1000).ToString("D3");

                }

                currentOrder.OrderStatus = ((ComboBoxItem)status_cb.SelectedItem).Content.ToString();
                currentOrder.PickupPointId = ((PickupPoint)pickup_points_cb.SelectedItem).PickupPointId;
                currentOrder.OrderDate = DateOnly.FromDateTime(orderDate_dp.SelectedDate.Value);
                currentOrder.DeliveryDate = DateOnly.FromDateTime(issueDate_dp.SelectedDate.Value);

                if (currentOrder.OrderId == 0)
                    DbShoesContext.Context.Orders.Add(currentOrder);
                else
                    DbShoesContext.Context.Orders.Update(currentOrder);

                DbShoesContext.Context.SaveChanges();

                MessageBox.Show("Заказ успешно сохранен!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
