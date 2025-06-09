using SalonPhenomenon.Modules;
using System.Linq;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages.Forms
{
    /// <summary>
    /// Логика взаимодействия для AddEditRegistration.xaml
    /// </summary>
    public partial class AddEditRegistration : Page
    {
        public AddEditRegistration()
        {
            InitializeComponent();
            CBService.ItemsSource = SalonEntities.GetContext().Services.ToList();
            CBStatus.ItemsSource = SalonEntities.GetContext().Statuses.ToList();

        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //StringBuilder errors = new StringBuilder();

            //// Проверка даты
            //if (DateDpic.SelectedDate == null)
            //    errors.AppendLine("Укажите дату записи!");
            //else if (DateDpic.SelectedDate < DateTime.Today)
            //    errors.AppendLine("Дата записи не может быть в прошлом!");

            //// Проверка времени
            //if (string.IsNullOrWhiteSpace(TimeTextB.Text))
            //    errors.AppendLine("Укажите время записи!");
            //else if (!TimeSpan.TryParse(TimeTextB.Text, out _))
            //    errors.AppendLine("Время указано в неверном формате (используйте HH:MM)!");

            //// Проверка клиента
            //if (string.IsNullOrWhiteSpace(ClientSurnameTBox.Text))
            //    errors.AppendLine("Укажите фамилию клиента!");
            //if (string.IsNullOrWhiteSpace(NameClientTBox.Text))
            //    errors.AppendLine("Укажите имя клиента!");
            //if (string.IsNullOrWhiteSpace(PatronymicClientTBox.Text))
            //    errors.AppendLine("Укажите отчество клиента!");

            //// Проверка услуги
            //if (string.IsNullOrWhiteSpace(ServiceTBox.Text))
            //    errors.AppendLine("Укажите услугу клиента!");

            //// Проверка статуса
            //if (CBStatus.SelectedItem == null)
            //    errors.AppendLine("Выберите статус записи!");

            //// Проверка суммы
            //if (string.IsNullOrWhiteSpace(TBCum.Text))
            //    errors.AppendLine("Укажите сумму!");
            //else if (!decimal.TryParse(TBCum.Text, out decimal sum) || sum <= 0)
            //    errors.AppendLine("Сумма должна быть положительным числом!");

            //if (errors.Length > 0)
            //{
            //    MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            //// Получаем текущую запись из контекста данных
            //var currentRegistration = (Registrations)DataContext;

            //try
            //{
            //    if (currentRegistration.RegistrationID == 0)
            //    {
            //        // Добавляем новую запись
            //        SalonPhenEntities.GetContext().Registrations.Add(currentRegistration);
            //    }
            //    else
            //    {
            //        // Обновляем существующую запись
            //        SalonPhenEntities.GetContext().Entry(currentRegistration).State = EntityState.Modified;
            //    }

            //    SalonPhenEntities.GetContext().SaveChanges();
            //    MessageBox.Show("Запись успешно сохранена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            //    // Возвращаемся на предыдущую страницу
            //    Manager.MainFrame.GoBack();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка при сохранении: {ex.Message}\n\nПодробности:\n{ex.InnerException?.Message}",
            //                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }
    }
}
