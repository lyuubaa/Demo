using Microsoft.EntityFrameworkCore;
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
using ShoesShop.Models;

namespace ShoesShop.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

        }

        private void login_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(login_txt.Text) || string.IsNullOrEmpty(password_txt.Password))
                {
                    MessageBox.Show("Поля не должны оставаться пустыми!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var user = DbShoesContext.Context.Users
                    .FirstOrDefault(u => u.UserEmail == login_txt.Text && u.UserPassword == password_txt.Password);

                if (user != null)
                {
                    MessageBox.Show("Авторизация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    new MainWindow(user).Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void login_as_guest_bt_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(null).Show();
            Close();
        }
    }
}
