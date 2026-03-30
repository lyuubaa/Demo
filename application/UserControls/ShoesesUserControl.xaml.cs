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
using static System.Net.Mime.MediaTypeNames;

namespace ShoesShop.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ShoesesUserControl.xaml
    /// </summary>
    public partial class ShoesesUserControl : UserControl
    {
        private Tovar cur_tovar;
        public ShoesesUserControl(Tovar tovar)
        {
            InitializeComponent();
            cur_tovar = tovar;
            DataContext = cur_tovar;

            img.Source = new BitmapImage(new Uri(cur_tovar.DisplayedImage, UriKind.Relative));

            if (cur_tovar.CurrentDiscount > 15)
                percent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));

            if(cur_tovar.QuantityInStock < 1)
                count_in_stock.Background = Brushes.CornflowerBlue;
            try
            {
                procents.Text = cur_tovar.CurrentDiscount.ToString().Substring(0, cur_tovar.CurrentDiscount.ToString().Length - 3) + " %";
            }
            catch { }
            if (cur_tovar.CurrentDiscount > 0)
            {
                decimal summ = cur_tovar.TovarPrice - (cur_tovar.TovarPrice * cur_tovar.CurrentDiscount / 100);
                price_with_discount.Text = summ.ToString("F2");
                price_with_discount.Visibility = Visibility.Visible;
                price.TextDecorations = TextDecorations.Strikethrough;
                price.Foreground = Brushes.Red;
            }
        }
    }
}
