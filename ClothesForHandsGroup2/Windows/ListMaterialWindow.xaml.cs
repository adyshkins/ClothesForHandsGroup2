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

using ClothesForHandsGroup2.Windows;
using ClothesForHandsGroup2.ClassHelper;
using ClothesForHandsGroup2.EF; // Обращение к папке EF
using static ClothesForHandsGroup2.EF.AppData; // вызов методов из класса AppData напрямую


namespace ClothesForHandsGroup2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ListMaterialWindow : Window
    {
        private List<string> listSort = new List<string>() // Формарование листа для сортировки
        {
            "Наименование (по возрастанию)",
            "Наименование (по убыванию)",
            "Остаток на складе (по возрастанию)",
            "Остаток на складе (по убыванию)",
            "Стоимость (по возрастанию)",
            "Стоимость (по убыванию)"
        };

        List<TypeMaterial> typeMaterials = new List<TypeMaterial>(); // список типов материалов

        List<Material> listMaterials = new List<Material>(); // список для выгрузки на окно

        List<Material> selectMaterial = new List<Material>(); // список для выбранных материалов


        int numberPage = 0;
        int countMaterial;

        private void Filter() // Поиск, ФИЛЬТРАЦИЯ, сортировка
        {
            listMaterials = Context.Material.ToList(); // получиния всех материалов из БД
            
            // Поиск
            listMaterials = listMaterials.
                Where(i => i.Name.ToLower().Contains(txtSearch.Text.ToLower())).
                ToList();

            // Сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listMaterials = listMaterials.OrderBy(i => i.Name).ToList(); // сортировка по возрастанию
                    break;

                case 1:
                    listMaterials = listMaterials.OrderByDescending(i => i.Name).ToList(); // сортировка по убыванию
                    break;

                case 2:
                    listMaterials = listMaterials.OrderBy(i => i.Count).ToList();
                    break;

                case 3:
                    listMaterials = listMaterials.OrderByDescending(i => i.Count).ToList();
                    break;

                case 4:
                    listMaterials = listMaterials.OrderBy(i => i.Price).ToList();
                    break;

                case 5:
                    listMaterials = listMaterials.OrderByDescending(i => i.Price).ToList();
                    break;

                default:
                    listMaterials = listMaterials.OrderBy(i => i.Name).ToList();
                    break;
            }


            // Фильтер

            if (cmbFilter.SelectedIndex != 0)
            {
                listMaterials = listMaterials.Where(i => i.TypeId == cmbFilter.SelectedIndex).ToList();
            }

            countMaterial = listMaterials.Count;

            // Постраничный вывод

            listMaterials = listMaterials.
                Skip(numberPage * 15).
                Take(15).
                ToList();

            // издеваемся над кнопками

            if (Convert.ToInt32(btn2.Content) > (((countMaterial / 15)) + 1))
            {
                btn2.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn2.Visibility = Visibility.Visible;
            }

            if (Convert.ToInt32(btn3.Content) > (((countMaterial / 15)) + 1))
            {
                btn3.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn3.Visibility = Visibility.Visible;
            }

            lvListMaterial.ItemsSource = listMaterials;
            tbCountMaterial.Text = countMaterial.ToString();
            tbCountMaterialOnPage.Text = listMaterials.Count.ToString();
            
        }

        public ListMaterialWindow()
        {
            InitializeComponent();

            btnEditMinCount.Visibility = Visibility.Collapsed;

            cmbSort.ItemsSource = listSort; // заполнеие ComboBox для сортировки
            cmbSort.SelectedIndex = 0;

            typeMaterials = Context.TypeMaterial.ToList();            

            typeMaterials.Insert(0, new TypeMaterial { Name = "Все типы" }); // добавление в список элемента "ВСЕ ТИПЫ"
            cmbFilter.ItemsSource = typeMaterials; // заполнеие ComboBox для фильтрации
            cmbFilter.DisplayMemberPath = "Name";
            cmbFilter.SelectedIndex = 0;

            Filter();
            
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (numberPage > 0)
            {
                numberPage--;
                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
                Filter();
            }
            
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            
            if (((countMaterial / 15)) > numberPage )
            {
                numberPage++;

                btn1.Content = (numberPage + 1).ToString();
                btn2.Content = (numberPage + 2).ToString();
                btn3.Content = (numberPage + 3).ToString();
             
                Filter();
            }
           
        }

        private void lvListMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListMaterial.SelectedIndex != -1) // если выбран элемент то показать кнопку
            {
                btnEditMinCount.Visibility = Visibility.Visible;
            }
            else
            {
                btnEditMinCount.Visibility = Visibility.Collapsed; // если НЕ выбран элемент то спрятать кнопку
            }
            
        }

        private void btnEditMinCount_Click(object sender, RoutedEventArgs e)
        {
            foreach (var material in lvListMaterial.SelectedItems)
            {
                selectMaterial.Add(material as Material);
            }

            MinCountMaterial.EditMinCount = selectMaterial.Max(i => i.MinCount);

            EditminCountWindow editminCountWindow = new EditminCountWindow();
            editminCountWindow.ShowDialog();

            foreach (var item in selectMaterial)
            {
                item.MinCount = MinCountMaterial.EditMinCount;
            }
            Context.SaveChanges();

            Filter();
        }
    }
}
