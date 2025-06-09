using SalonPhenomenon.Modules;
using SalonPhenomenon.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages.PagesForUsers
{
    /// <summary>
    /// Логика взаимодействия для MastersUser.xaml
    /// </summary>
    public partial class MastersUser : Page
    {
        public MastersUser()
        {
            InitializeComponent();
            MastersUserDataGrid.ItemsSource = SalonEntities.GetContext().Masters.ToList();
        }
        private void LoadMasters()
        {
            using (var context = new SalonEntities())
            {
                MastersUserDataGrid.ItemsSource = context.Services
                    .ToList();
            }
        }

        private void MastGoBackBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RecordsPage());
        }

        private void MasterServiceBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var master = MastersUserDataGrid.SelectedItem as Masters;

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
    }
}
