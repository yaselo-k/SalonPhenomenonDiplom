using SalonPhenomenon.Modules;
using SalonPhenomenon.Utils;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private readonly SalonPhenEntities _context;
        private Services _currentService;
        public ServicesPage()
        {
            InitializeComponent();
            _context = SalonPhenEntities.GetContext();
            LoadServices();
        }

        private void LoadServices()
        {
            using (var context = new SalonPhenEntities())
            {
                ServicesDataGrid.ItemsSource = _context.Services.ToList();
                ServiceEditBtn.IsEnabled = false;
                ServiceAddBtn.IsEnabled = false;
            }
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EditServiceBtn_Click(object sender, RoutedEventArgs e)
        { }
        

        private void DeleteServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            var service = ServicesDataGrid.SelectedItem as Services;

            if (service == null)
            {
                MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var context = new SalonPhenEntities())
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
            _currentService = null;
            ClearEditFields();
            EditServicePanel.Visibility = Visibility.Visible;
        }

        private void ServiceEditBtn_Click(object sender, RoutedEventArgs e)
        {
            EditServicePanel.Visibility = Visibility.Visible;

            if (ServicesDataGrid.SelectedItem is Services sel)
            {
                _currentService = _context.Services.Find(sel.ServiceID);
                if (_currentService != null)
                {
                    ServiceTB.Text = _currentService.ServiceName;
                    DurInMinTB.Text = _currentService.ServiceDurationInMin.ToString();
                    CostTB.Text = _currentService.ServiceCost.ToString();
                    ServicesDataGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void ServicesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = ServicesDataGrid.SelectedItem != null;
            ServiceEditBtn.IsEnabled = sel;
            ServiceAddBtn.IsEnabled = sel;
        }
        private void ClearEditFields()
        {
            ServiceTB.Clear();
            DurInMinTB.Clear();
            CostTB.Clear();
            _currentService = null;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            EditServicePanel.Visibility = Visibility.Collapsed;
            ClearEditFields();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();

            // Вспомогательные методы
            bool ContainsDigit(string input) =>
                !string.IsNullOrEmpty(input) && input.Any(char.IsDigit);

            bool ContainsSpecialChars(string input)
            {
                if (string.IsNullOrEmpty(input)) return false;
                char[] specialChars = "!@#$%^&*{}[]|\\\"'<>?,./~`+=_-".ToCharArray();
                return input.Any(c => specialChars.Contains(c));
            }

            bool HasValidLength(string input) =>
                input?.Trim().Length >= 2;

            // Проверка: Название услуги
            if (string.IsNullOrWhiteSpace(ServiceTB.Text))
            {
                sb.AppendLine("• Введите наименование услуги.");
            }
            else
            {
                if (!HasValidLength(ServiceTB.Text))
                    sb.AppendLine("• Название должно содержать минимум 2 символа.");
                if (ContainsDigit(ServiceTB.Text))
                    sb.AppendLine("• Название не должно содержать цифры.");
                if (ContainsSpecialChars(ServiceTB.Text))
                    sb.AppendLine("• Название содержит недопустимые символы.");
            }

            // Проверка: Длительность в минутах
            if (!int.TryParse(DurInMinTB.Text, out var dur))
            {
                sb.AppendLine("• Укажите длительность в минутах.");
            }
            else if (dur.ToString().Length >= 4)
            {
                sb.AppendLine("• Длительность не должна быть четырёхзначной.");
            }

            // Проверка: Стоимость
            if (!decimal.TryParse(CostTB.Text, out var sum))
            {
                sb.AppendLine("• Укажите стоимость.");
            }
            else if (sum <= 0)
            {
                sb.AppendLine("• Стоимость должна быть больше нуля.");
            }

            // Если есть ошибки — показываем их
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString().TrimEnd(), "Ошибка ввода",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_currentService == null)
            {
                _currentService = new Services();
                _context.Services.Add(_currentService);
            }

            _currentService.ServiceName = ServiceTB.Text.Trim();
            _currentService.ServiceDurationInMin = dur;  // Теперь сохраняем int
            _currentService.ServiceCost = sum;          // Теперь сохраняем decimal

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message,
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadServices();
            EditServicePanel.Visibility = Visibility.Collapsed;
            ClearEditFields();
        }
    }
}
