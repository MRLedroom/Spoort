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

namespace Sportingh.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminLKPage.xaml
    /// </summary>
    public partial class AdminLKPage : Page
    {
        public AdminLKPage()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            FitnessListView.ItemsSource = Data.SportingZlatovRyaskixEntities.GetContext().Fitness.ToList();
            var manufactlist = Data.SportingZlatovRyaskixEntities.GetContext().Manufacturer.ToList();
            manufactlist.Insert(0, new Data.Manufacturer { ManufacturerName = "Все производители" });
            ManufacturerComboBox.ItemsSource = manufactlist;
            ManufacturerComboBox.SelectedIndex = 0;

            if (Classes.Manager.CurrentUser != null)
            {
                FIOLabel.Visibility = Visibility.Visible;
                FIOLabel.Content = $"{Classes.Manager.CurrentUser.Surname} " +
                    $"{Classes.Manager.CurrentUser.Lname} " +
                    $"{Classes.Manager.CurrentUser.Patronymic}";
            }
            else
            {
                FIOLabel.Visibility = Visibility.Hidden;
            }
            CountOfLabel.Content = $"{Data.SportingZlatovRyaskixEntities.GetContext().Fitness.Count()}/" +
                $"{Data.SportingZlatovRyaskixEntities.GetContext().Fitness.Count()}";
        }
        public List<Data.Fitness> _currentProduct = Data.SportingZlatovRyaskixEntities.GetContext().Fitness.ToList();

        public void Update()
        {
            try
            {
                _currentProduct = Data.SportingZlatovRyaskixEntities.GetContext().Fitness.ToList();

                _currentProduct = (from item in Data.SportingZlatovRyaskixEntities.GetContext().Fitness
                                   where item.Name.NameFClub.ToLower().Contains(SearchTextBox.Text) ||
                                   item.FitnessName.ToLower().Contains(SearchTextBox.Text) ||
                                   item.Manufacturer.ManufacturerName.ToLower().Contains(SearchTextBox.Text) ||
                                   item.Price.ToString().ToLower().Contains(SearchTextBox.Text) ||
                                   item.Count.ToString().ToLower().Contains(SearchTextBox.Text)
                                   select item).ToList();



                if (SortUpRadioButton.IsChecked == true)
                {
                    _currentProduct = _currentProduct.OrderBy(d => d.Price).ToList();
                }

                if (SortDownRadioButton.IsChecked == true)
                {
                    _currentProduct = _currentProduct.OrderByDescending(d => d.Price).ToList();
                }

                var selected = ManufacturerComboBox.SelectedItem as Data.Manufacturer;
                if (selected != null && selected.ManufacturerName != "Все производители")
                {
                    _currentProduct = _currentProduct.Where(d => d.IdManufacturer == selected.ManufacturerId).ToList();
                }

                CountOfLabel.Content = $"{_currentProduct.Count}/{Data.SportingZlatovRyaskixEntities.GetContext().Fitness.Count()}";

                FitnessListView.ItemsSource = _currentProduct;

            }
            catch (Exception)
            {

            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void SortUpRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void SortDownRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void ManufacturerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Classes.Manager.MainFrame.Navigate(new Pages.LoginPage());
        }
    }
}
