using SalonPhenomenon.Modules;
using SalonPhenomenon.Utils;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages.PagesForUsers
{
    /// <summary>
    /// Логика взаимодействия для ServicesUser.xaml
    /// </summary>
    public partial class ServicesUser : Page
    {
        public ServicesUser()
        {
            InitializeComponent();
            ServicesUserDataGrid.ItemsSource = SalonPhenEntities.GetContext().Services.ToList();
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RecordsPage());
        }
    }
}
