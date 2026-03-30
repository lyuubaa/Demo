using Microsoft.EntityFrameworkCore;
using ShoesShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShoesShop.Views
{
    public partial class MainWindow : Window
    {
        private User cur_user;

        public MainWindow(User user)
        {
            InitializeComponent();
            if (user != null)
            {
                cur_user = user;
                fullName_txt.Text = $"{cur_user.UserRole}: {cur_user.UserSurname} {cur_user.UserFirstname} {cur_user.UserLastname}";
                CheckRole(cur_user.UserRole);
            }
            else
            {
                fullName_txt.Text = "Гость";
                CheckRole("Гость");
            }
            LoadSuppliers();
            LoadTovars();
        }

        private void CheckRole(string role)
        {
            switch (role)
            {
                case "Менеджер":
                    panel_find.Visibility = Visibility.Visible;
                    panel_sorted.Visibility = Visibility.Visible;
                    orders_bt.Visibility = Visibility.Visible;
                    break;
                case "Администратор":
                    tovar_menu.Visibility = Visibility.Visible;
                    panel_find.Visibility = Visibility.Visible;
                    panel_sorted.Visibility = Visibility.Visible;
                    add_tovar_bt.Visibility = Visibility.Visible;
                    orders_bt.Visibility = Visibility.Visible;
                    tovars_list.MouseDoubleClick += tovars_list_MouseDoubleClick;
                    break;
                default:
                    break;
            }
        }

        private void LoadTovars()
        {
            tovars_list.Items.Clear();
            not_found.Visibility = Visibility.Hidden;

            var tovars = DbShoesContext.Context.Tovars
                .Include(s => s.Manufacturer)
                .Include(s => s.Supplier)
                .Include(s => s.Category)
                .ToList();

            foreach (var tovar in tovars)
            {
                tovars_list.Items.Add(new UserControls.ShoesesUserControl(tovar));
            }
            if(tovars_list.Items.Count == 0)
            {
                not_found.Visibility = Visibility.Visible;
            }
        }

        private void back_bt_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

        private void sort_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tovars_list == null) return;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            tovars_list.Items.Clear();
            not_found.Visibility = Visibility.Hidden;

            // Получаем текст поиска
            string searchText = find_text.Text.ToLower().Trim();

            // Получаем тип сортировки по количеству
            string sortType = (stock_sort_cb.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "по умолчанию";

            // Получаем выбранного поставщика
            int selectedSupplierId = 0;
            if (supplier_sort_cb.SelectedItem is Supplier selectedSupplier)
            {
                selectedSupplierId = selectedSupplier.SupplierId;
            }

            // Если все фильтры по умолчанию - просто загружаем все товары
            if (sortType == "по умолчанию" &&
                string.IsNullOrWhiteSpace(searchText) &&
                selectedSupplierId == 0)
            {
                LoadTovars();
                return;
            }

            // Базовый запрос
            var tovarsQuery = DbShoesContext.Context.Tovars
                .Include(t => t.Manufacturer)
                .Include(t => t.Supplier)
                .Include(t => t.Category)
                .AsQueryable();

            // Применяем фильтр по поставщику (если выбран не "Все поставщики")
            if (selectedSupplierId > 0)
            {
                tovarsQuery = tovarsQuery.Where(t => t.SupplierId == selectedSupplierId);
            }

            // Применяем поиск, если есть текст
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                tovarsQuery = tovarsQuery.Where(t =>
                    t.TovarName.ToLower().Contains(searchText) ||
                    (t.TovarDescription != null && t.TovarDescription.ToLower().Contains(searchText)) ||
                    t.Category.CategoryName.ToLower().Contains(searchText) ||
                    t.Manufacturer.ManufacturerName.ToLower().Contains(searchText) ||
                    t.Supplier.SupplierName.ToLower().Contains(searchText) ||
                    (t.UnitOfMeasurement != null && t.UnitOfMeasurement.ToLower().Contains(searchText))
                );
            }

            // Применяем сортировку по количеству
            if (sortType == "по возрастанию")
            {
                tovarsQuery = tovarsQuery.OrderBy(t => t.QuantityInStock);
            }
            else if (sortType == "по убыванию")
            {
                tovarsQuery = tovarsQuery.OrderByDescending(t => t.QuantityInStock);
            }

            // Получаем результаты
            var tovars = tovarsQuery.ToList();

            // Заполняем список
            foreach (var tovar in tovars)
            {
                tovars_list.Items.Add(new UserControls.ShoesesUserControl(tovar));
            }

            if (tovars_list.Items.Count == 0)
            {
                not_found.Visibility = Visibility.Visible;
            }
        }

        private void find_text_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tovars_list == null) return;
            ApplyFilters();
        }

        private void supplier_sort_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tovars_list == null) return;
            ApplyFilters();
        }

        private void LoadSuppliers()
        {
            var suppliers = DbShoesContext.Context.Suppliers.ToList();

            var comboBoxItems = new List<Supplier> { new Supplier { SupplierId = 0, SupplierName = "Все поставщики" } };
            comboBoxItems.AddRange(suppliers);

            supplier_sort_cb.ItemsSource = comboBoxItems;
            supplier_sort_cb.DisplayMemberPath = "SupplierName";
            supplier_sort_cb.SelectedIndex = 0;
        }

        private void tovars_list_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = tovars_list.SelectedItem as UserControls.ShoesesUserControl;
            if (selectedItem != null)
            {
                var selectedTovar = selectedItem.DataContext as Tovar;
                var window = new AddingEditingWindow(selectedTovar);
                if (window.ShowDialog() == true)
                {
                    LoadTovars();
                }
            }
        }

        private async void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var selectedControl = tovars_list.SelectedItem as UserControls.ShoesesUserControl;
                if (selectedControl == null) return;

                var selectedTovar = selectedControl.DataContext as Tovar;
                if (selectedTovar == null) return;

                var result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить товар {selectedTovar.TovarName}?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var is_ordered = DbShoesContext.Context.OrderItems
                   .FirstOrDefault(o => o.TovarArticle == selectedTovar.TovarArticle);

                    if (is_ordered != null)
                    {
                        MessageBox.Show("Товар есть в заказах!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    DbShoesContext.Context.Tovars.Remove(selectedTovar);
                    DbShoesContext.Context.SaveChanges();

                    LoadTovars();

                    MessageBox.Show("Товар успешно удален!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void add_tovar_bt_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddingEditingWindow();
            if (window.ShowDialog() == true)
            {
                LoadTovars();
            }
        }

        private void orders_bt_Click(object sender, RoutedEventArgs e)
        {
            new OrdersWindow(cur_user).Show();
            Close();
        }
    }
}