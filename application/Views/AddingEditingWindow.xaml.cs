using Microsoft.Win32;
using ShoesShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShoesShop.Views
{
    public partial class AddingEditingWindow : Window
    {
        private Tovar _currentTovar;
        private string _newImagePath;

        public AddingEditingWindow(Tovar tovar = null)
        {
            InitializeComponent();
            LoadComboBoxes();

            if (tovar != null)
            {
                _currentTovar = tovar;
                LoadTovarData();
                artic.Text = $"(арт.: {_currentTovar.TovarArticle})";
                Title = "Редактирование товара";
            }
            else
            {
                _currentTovar = new Tovar();
                Title = "Добавление товара";
                window_name_txt.Text = "ДОБАВЛЕНИЕ";
            }
        }

        private void LoadComboBoxes()
        {
            category_cb.ItemsSource = DbShoesContext.Context.Categories.ToList();
            manufacturer_cb.ItemsSource = DbShoesContext.Context.Manufactures.ToList();
            supplier_cb.ItemsSource = DbShoesContext.Context.Suppliers.ToList();
        }

        private void LoadTovarData()
        {
            if (_currentTovar == null) return;

            tovarName_txt.Text = _currentTovar.TovarName;
            description_txt.Text = _currentTovar.TovarDescription;
            price_txt.Text = _currentTovar.TovarPrice.ToString("F2");
            unit_txt.Text = _currentTovar.UnitOfMeasurement;

            quantity_txt.Text = _currentTovar.QuantityInStock.ToString();
            discount_txt.Text = _currentTovar.CurrentDiscount.ToString("F2");

            category_cb.SelectedItem = _currentTovar.Category;
            manufacturer_cb.SelectedItem = _currentTovar.Manufacturer;
            supplier_cb.SelectedItem = _currentTovar.Supplier;

            LoadImage(_currentTovar.Photo);
        }

        private void LoadImage(string photoPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(photoPath))
                {
                    imageItem.Source = new BitmapImage(new Uri($"/Resources/{photoPath}", UriKind.Relative));
                }
                else
                {
                    imageItem.Source = new BitmapImage(new Uri("/Resources/picture.png", UriKind.Relative));
                }
            }
            catch
            {
                imageItem.Source = new BitmapImage(new Uri("/Resources/picture.png", UriKind.Relative));
            }
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string resourcesPath = @"C:\Users\Любовь\source\repos\ShoesShop\ShoesShop\Resources\";

            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
            }

            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Выбор изображения товара";
            fileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            fileDialog.InitialDirectory = resourcesPath;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    // Проверяем размер
                    using (var fileStream = new FileStream(fileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = fileStream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        if (bitmap.PixelWidth > 300 || bitmap.PixelHeight > 200)
                        {
                            MessageBox.Show($"Фотография должна быть не более 300x200 пикселей!\nТекущий размер: {bitmap.PixelWidth}x{bitmap.PixelHeight}",
                                          "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    // Проверяем что файл из папки Resources
                    string selectedDir = Path.GetDirectoryName(fileDialog.FileName) + @"\";
                    if (!selectedDir.Equals(resourcesPath, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Можно выбирать только файлы из папки Resources!\n\nПапка Resources: {resourcesPath}",
                                      "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string fileName = Path.GetFileName(fileDialog.FileName);
                    _newImagePath = fileName; 

                    imageItem.Source = new BitmapImage(new Uri($"/Resources/{fileName}", UriKind.Relative));

                    MessageBox.Show("Изображение успешно выбрано!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GenerateUniqueArticle()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            string article;

            do
            {
                article = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (DbShoesContext.Context.Tovars.Any(t => t.TovarArticle == article));

            return article;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполнения обязательных полей
                if (string.IsNullOrWhiteSpace(tovarName_txt.Text))
                {
                    MessageBox.Show("Введите наименование товара!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (category_cb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите категорию товара!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (manufacturer_cb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите производителя!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (supplier_cb.SelectedItem == null)
                {
                    MessageBox.Show("Выберите поставщика!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка цены
                if (!decimal.TryParse(price_txt.Text.Replace('.', ','), out decimal price) || price <= 0)
                {
                    MessageBox.Show("Цена должна быть больше нуля (можно использовать копейки)!",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка количества
                if (!int.TryParse(quantity_txt.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Количество на складе должно быть больше нуля!",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка скидки
                if (!decimal.TryParse(discount_txt.Text.Replace('.', ','), out decimal discount) ||
                    discount < 0 || discount > 99)
                {
                    MessageBox.Show("Скидка должна быть числом от 0 до 99!",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Заполняем данные товара
                _currentTovar.TovarName = tovarName_txt.Text.Trim();
                _currentTovar.TovarDescription = description_txt.Text?.Trim() ?? "";
                _currentTovar.TovarPrice = Math.Round(price, 2);
                _currentTovar.UnitOfMeasurement = unit_txt.Text?.Trim() ?? "шт.";
                _currentTovar.QuantityInStock = quantity;
                _currentTovar.CurrentDiscount = discount;

                _currentTovar.CategoryId = ((Category)category_cb.SelectedItem).CategoryId;
                _currentTovar.Category = (Category)category_cb.SelectedItem;

                _currentTovar.ManufacturerId = ((Manufacture)manufacturer_cb.SelectedItem).ManufacturerId;
                _currentTovar.Manufacturer = (Manufacture)manufacturer_cb.SelectedItem;

                _currentTovar.SupplierId = ((Supplier)supplier_cb.SelectedItem).SupplierId;
                _currentTovar.Supplier = (Supplier)supplier_cb.SelectedItem;

                if (!string.IsNullOrEmpty(_newImagePath))
                {
                    _currentTovar.Photo = _newImagePath;
                }

                if (string.IsNullOrEmpty(_currentTovar.TovarArticle))
                {
                    _currentTovar.TovarArticle = GenerateUniqueArticle();
                }

                var existingTovar = DbShoesContext.Context.Tovars
                    .FirstOrDefault(t => t.TovarArticle == _currentTovar.TovarArticle);

                if (existingTovar == null)
                {
                    DbShoesContext.Context.Tovars.Add(_currentTovar);
                }
                else
                {
                    DbShoesContext.Context.Tovars.Update(_currentTovar);
                }

                DbShoesContext.Context.SaveChanges();

                MessageBox.Show("Данные успешно сохранены!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}