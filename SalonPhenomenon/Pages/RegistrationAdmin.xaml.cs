using SalonPhenomenon.Modules;
using SalonPhenomenon.Pages.Forms;
using SalonPhenomenon.Utils;
using SalonPhenomenon.Windows;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationAdmin.xaml
    /// </summary>
    public partial class RegistrationAdmin : Page
    {
        public RegistrationAdmin()
        {
            InitializeComponent();
            RecordsDataGrid.ItemsSource = SalonEntities.GetContext().Registrations.ToList();
        }
        private void LoadRegistrations()
        {
            using (var context = new SalonEntities())
            {
                RecordsDataGrid.ItemsSource = context.Registrations
                    .ToList();
            }
        }

        private void ExitBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AutorizationPage());
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditRegistration());
        }

        private void RecordEditBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditRegistration());
        }

        private void ServicesBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ServicesPage());
        }

        private void DetailBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RecordsDataGrid.SelectedItem is Registrations selectedRecord)
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
                MessageBox.Show("Выберите запись для просмотра деталей.",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Warning);
            }
        }

        private void MastersBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new MastersListPage());
        }

        private void RecordDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var recordobj = RecordsDataGrid.SelectedItem as Registrations;

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
    }
}
