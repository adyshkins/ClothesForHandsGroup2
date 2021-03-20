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

using ClothesForHandsGroup2.EF; // Обращение к папке EF
using static ClothesForHandsGroup2.EF.AppData; // вызов методов из класса AppData напрямую

namespace ClothesForHandsGroup2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ListMaterialWindow : Window
    {
        private List<string> listSort = new List<string>() 
        {
            "Наименование (по возрастанию)",
            "Наименование (по убыванию)",
            "Остаток на складе (по возрастанию)",
            "Остаток на складе (по убыванию)",
            "Стоимость (по возрастанию)",
            "Стоимость (по убыванию)",
        };

        List<TypeMaterial> typeMaterials = new List<TypeMaterial>();

        List<Material> listMaterials = new List<Material>();


        private void Filter()
        {
            listMaterials = Context.Material.ToList();
            // Поиск
            listMaterials = listMaterials.
                Where(i => i.Name.ToLower().Contains(txtSearch.Text.ToLower())).
                ToList();

            // Сортировка
            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listMaterials = listMaterials.OrderBy(i => i.Name).ToList();
                    break;

                case 1:
                    listMaterials = listMaterials.OrderByDescending(i => i.Name).ToList();
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
            switch (cmbFilter.SelectedIndex)
            {
                case 0:
                    listMaterials = listMaterials.ToList();
                    break;
                case 1:
                    listMaterials = listMaterials.Where(i => i.TypeId == cmbFilter.SelectedIndex).ToList();
                    break;
                case 2:
                    listMaterials = listMaterials.Where(i => i.TypeId == cmbFilter.SelectedIndex).ToList();
                    break;
                case 3:
                    listMaterials = listMaterials.Where(i => i.TypeId == cmbFilter.SelectedIndex).ToList();
                    break;
                default:
                    break;
            }

            // Постраничный вывод

            //listMaterials = listMaterials.
            //    OrderBy(i => i.ID).
            //    Skip(0).
            //    Take(15).
            //    ToList();

            lvListMaterial.ItemsSource = listMaterials;
        }

        public ListMaterialWindow()
        {
            InitializeComponent();

            cmbSort.ItemsSource = listSort;
            cmbSort.SelectedIndex = 0;

            typeMaterials = Context.TypeMaterial.ToList();

            TypeMaterial newType = new TypeMaterial() {Name = "Все типы"};

            typeMaterials.Insert(0, newType);
            cmbFilter.ItemsSource = typeMaterials;
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
    }
}
