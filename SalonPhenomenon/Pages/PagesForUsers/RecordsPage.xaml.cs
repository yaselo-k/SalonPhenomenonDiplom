using SalonPhenomenon.Modules;
using SalonPhenomenon.Utils;
using SalonPhenomenon.Windows;
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

namespace SalonPhenomenon.Pages.PagesForUsers
{
    /// <summary>
    /// Логика взаимодействия для RecordsPage.xaml
    /// </summary>
    public partial class RecordsPage : Page
    {
        public RecordsPage()
        {
            InitializeComponent();
            RecordsDataGridUser.ItemsSource = SalonEntities.GetContext().Registrations.ToList();
        }
        private void LoadRegistrations()
        {
            using (var context = new SalonEntities())
            {
                RecordsDataGridUser.ItemsSource = context.Registrations
                    .ToList();
            }
        }

        private void RecordDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var recordobj = RecordsDataGridUser.SelectedItem as Registrations;

            if (recordobj == null)
            {
                MessageBox.Show("Выберите запись");
                return;
            }

            if (MessageBox.Show("Удалить?", "Подтвердите", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var context = new SalonEntities())
                {
                    var entry = context.Registrations.Find(recordobj.RegistrationID);
                    if (entry != null)
                    {
                        context.Registrations.Remove(entry);
                        context.SaveChanges();
                        LoadRegistrations();
                    }
                }
            }
        }

        private void DetailRowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RecordsDataGridUser.SelectedItem is Registrations selectedRecord)
            {
                var detailsWindow = new DetailsWindow(selectedRecord)
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите запись для просмотра деталей.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void MastersBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new MastersUser());
        }

        private void ServicesBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ServicesUser());
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AutorizationPage());
        }
    }
}
