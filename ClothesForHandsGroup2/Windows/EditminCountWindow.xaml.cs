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
using ClothesForHandsGroup2.ClassHelper;

namespace ClothesForHandsGroup2.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditminCountWindow.xaml
    /// </summary>
    public partial class EditminCountWindow : Window
    {
        public EditminCountWindow()
        {
            InitializeComponent();

            txtMinCount.Text = MinCountMaterial.EditMinCount.ToString();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Подтвердите изменение", "Изменение минимального кличества",MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int val;
                if (int.TryParse(txtMinCount.Text, out val))
                {
                    MinCountMaterial.EditMinCount = val;
                    Close();
                }
                else
                {
                    MessageBox.Show("Недопустимое значение");
                }
            }
            
        }
    }
}
