using SalonPhenomenon.Modules;
using SalonPhenomenon.Pages.Forms;
using SalonPhenomenon.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages
{
    /// <summary>
    /// Логика взаимодействия для MastersListPage.xaml
    /// </summary>
    public partial class MastersListPage : Page
    {
        public MastersListPage()
        {
            InitializeComponent();
            InitializeComponent();
            MastersDataGrid.ItemsSource = SalonEntities.GetContext().Masters.ToList();
        }
        private void LoadMasters()
        {
            using (var context = new SalonEntities())
            {
                MastersDataGrid.ItemsSource = context.Services
                    .ToList();
            }
        }

        private void MastGoBackBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RegistrationAdmin());
        }

        private void MasterServiceBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var master = MastersDataGrid.SelectedItem as Masters;

            if (master == null)
            {
                MessageBox.Show("Выберите мастера");
                return;
            }

            if (MessageBox.Show("Удалить?", "Подтвердите", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var context = new SalonEntities())
                {
                    var entry = context.Services.Find(master.MasterID);
                    if (entry != null)
                    {
                        context.Services.Remove(entry);
                        context.SaveChanges();
                        LoadMasters();
                    }
                }
            }
        }

        private void MasterAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditMaster());
        }

        private void MasterEditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditMaster());
        }
    }
}
