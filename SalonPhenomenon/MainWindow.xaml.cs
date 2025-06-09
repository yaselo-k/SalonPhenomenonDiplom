using SalonPhenomenon.Pages;
using SalonPhenomenon.Utils;
using System.Windows;

namespace SalonPhenomenon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            Manager.MainFrame.Navigate(new AutorizationPage());
        }
    }
}
