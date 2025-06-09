using SalonPhenomenon.Modules;
using System.Windows;

namespace SalonPhenomenon.Windows
{
    /// <summary>
    /// Логика взаимодействия для DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(Registrations record)
        {
            InitializeComponent();
            DataContext = record;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
