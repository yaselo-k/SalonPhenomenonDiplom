using SalonPhenomenon.Modules;
using SalonPhenomenon.Pages.Forms;
using SalonPhenomenon.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();
            ServicesDataGrid.ItemsSource = SalonEntities.GetContext().Services.ToList();
        }

        private void LoadServices()
        {
            using (var context = new SalonEntities())
            {
                ServicesDataGrid.ItemsSource = context.Services
                    .ToList();
            }
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditService());
        }

        private void EditServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditService());
        }

        private void DeleteServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            var service = ServicesDataGrid.SelectedItem as Services;

            if (service == null)
            {
                MessageBox.Show("Выберите услугу");
                return;
            }

            if (MessageBox.Show("Удалить?", "Подтвердите", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var context = new SalonEntities())
                {
                    var entry = context.Services.Find(service.ServiceID);
                    if (entry != null)
                    {
                        context.Services.Remove(entry);
                        context.SaveChanges();
                        LoadServices();
                    }
                }
            }
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RegistrationAdmin());
        }

        private void ServiceAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditService());
        }

        private void ServiceEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditService());
        }
    }
}
