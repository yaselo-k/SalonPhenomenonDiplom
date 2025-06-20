using SalonPhenomenon.Modules;
using SalonPhenomenon.Utils;
using SalonPhenomenon.Windows;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages
{
    public partial class RegistrationAdmin : Page
    {
        private readonly SalonPhenEntities _context;
        private Registrations _currentReg;

        public RegistrationAdmin()
        {
            InitializeComponent();
            _context = SalonPhenEntities.GetContext();
            LoadRegistrations();
            LoadEditLists();
        }

        private void LoadRegistrations()
        {
            RecordsDataGrid.ItemsSource = _context.Registrations.ToList();
            RecordEditBtn.IsEnabled = false;
            RecordDeleteBtn.IsEnabled = false;
        }

        private void LoadEditLists()
        {
            // Статусы
            CbEditStatus.ItemsSource = _context.Statuses
                                              .OrderBy(s => s.StatusName)
                                              .ToList();
            CbEditStatus.DisplayMemberPath = "StatusName";
            CbEditStatus.SelectedValuePath = "StatusID";

            // Услуги
            CbEditService.ItemsSource = _context.Services
                                               .OrderBy(s => s.ServiceName)
                                               .ToList();
            CbEditService.DisplayMemberPath = "ServiceName";
            CbEditService.SelectedValuePath = "ServiceID";

            // Мастера
            CbEditMaster.ItemsSource = _context.Masters
                                              .OrderBy(m => m.MasterSurname)
                                              .ToList();
            CbEditMaster.DisplayMemberPath = "MasterSurname";
            CbEditMaster.SelectedValuePath = "MasterID";
        }

        private void RecordsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = RecordsDataGrid.SelectedItem != null;
            RecordEditBtn.IsEnabled = sel;
            RecordDeleteBtn.IsEnabled = sel;
        }

        private void MastersBtn_Click(object sender, RoutedEventArgs e)
            => Manager.MainFrame.Navigate(new MastersListPage());

        private void ServicesBtn_Click(object sender, RoutedEventArgs e)
            => Manager.MainFrame.Navigate(new ServicesPage());

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
            => Manager.MainFrame.Navigate(new AutorizationPage());

        private void DetailBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RecordsDataGrid.SelectedItem is Registrations rec)
            {
                var wnd = new DetailsWindow(rec)
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                wnd.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите запись для просмотра деталей.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RecordAddBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentReg = null;
            ClearEditFields();
            EditPanel.Visibility = Visibility.Visible;
        }

        private void RecordEditBtn_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Visible;

            if (RecordsDataGrid.SelectedItem is Registrations sel)
            {
                _currentReg = _context.Registrations.Find(sel.RegistrationID);
                if (_currentReg != null)
                {
                    DpEditDate.SelectedDate = _currentReg.RegistrationDate;
                    TbEditTime.Text = _currentReg.RegistrationTime.ToString(@"hh\:mm");
                    TbEditSum.Text = _currentReg.RegistrationSum.ToString("F2");
                    TbEditSurname.Text = _currentReg.RegSurnameClient;
                    TbEditName.Text = _currentReg.RegNameClient;
                    TbEditPatronymic.Text = _currentReg.RegPatronymicClient;
                    TbEditPhone.Text = _currentReg.RegPhoneNumberClient;
                    CbEditStatus.SelectedValue = _currentReg.IDStatus;
                    CbEditService.SelectedValue = _currentReg.RegServiceID;
                    CbEditMaster.SelectedValue = _currentReg.IDMaster;
                    EditPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void RecordDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (RecordsDataGrid.SelectedItem is Registrations sel)
            {
                var toDel = _context.Registrations.Find(sel.RegistrationID);
                if (toDel != null &&
                    MessageBox.Show("Удалить запись?", "Подтвердите",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _context.Registrations.Remove(toDel);
                    _context.SaveChanges();
                    LoadRegistrations();
                }
            }
        }

        private bool ContainsSpecialChars(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            char[] specialChars = "!@#$%^&*(){}[]|\\\"'<>?,./~`+=_-".ToCharArray();
            return input.Any(c => specialChars.Contains(c));
        }

        private bool IsValidPhoneNumber(string number)
        {
            return !string.IsNullOrEmpty(number) &&
                   number.Length == 11 &&
                   number.All(char.IsDigit);
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();

            bool ContainsDigit(string input) =>
                !string.IsNullOrEmpty(input) && input.Any(char.IsDigit);

            bool HasValidLength(string input) =>
                input?.Trim().Length >= 2;

            if (DpEditDate.SelectedDate == null)
                sb.AppendLine("• Выберите дату.");
            if (!TimeSpan.TryParse(TbEditTime.Text, out var ts))
                sb.AppendLine("• Неверное время (HH:MM).");
            if (!decimal.TryParse(TbEditSum.Text, out var sum))
                sb.AppendLine("• Неверная сумма.");
            else if (sum < 0)
                sb.AppendLine("• Сумма не может быть отрицательной.");

            if (string.IsNullOrWhiteSpace(TbEditSurname.Text))
                sb.AppendLine("• Введите фамилию клиента.");
            else
            {
                if (!HasValidLength(TbEditSurname.Text))
                    sb.AppendLine("• Фамилия должна содержать минимум 2 символа.");
                if (ContainsDigit(TbEditSurname.Text))
                    sb.AppendLine("• Фамилия не должна содержать цифры.");
                if (ContainsSpecialChars(TbEditSurname.Text))
                    sb.AppendLine("• Фамилия содержит недопустимые символы.");
            }

            if (string.IsNullOrWhiteSpace(TbEditName.Text))
                sb.AppendLine("• Введите имя клиента.");
            else
            {
                if (!HasValidLength(TbEditName.Text))
                    sb.AppendLine("• Имя должно содержать минимум 2 символа.");
                if (ContainsDigit(TbEditName.Text))
                    sb.AppendLine("• Имя не должно содержать цифры.");
                if (ContainsSpecialChars(TbEditName.Text))
                    sb.AppendLine("• Имя содержит недопустимые символы.");
            }

            if (string.IsNullOrWhiteSpace(TbEditPatronymic.Text))
                sb.AppendLine("• Введите отчество клиента.");
            else
            {
                if (!HasValidLength(TbEditPatronymic.Text))
                    sb.AppendLine("• Отчество должно содержать минимум 2 символа.");
                if (ContainsDigit(TbEditPatronymic.Text))
                    sb.AppendLine("• Отчество не должно содержать цифры.");
                if (ContainsSpecialChars(TbEditPatronymic.Text))
                    sb.AppendLine("• Отчество содержит недопустимые символы.");
            }

            if (string.IsNullOrWhiteSpace(TbEditPhone.Text))
                sb.AppendLine("• Введите телефон клиента.");

            if (CbEditStatus.SelectedValue == null)
                sb.AppendLine("• Выберите статус.");
            if (CbEditService.SelectedValue == null)
                sb.AppendLine("• Выберите услугу.");
            if (CbEditMaster.SelectedValue == null)
                sb.AppendLine("• Выберите мастера.");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString().TrimEnd(), "Ошибка ввода",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var statusId = (int)CbEditStatus.SelectedValue;
            var serviceId = (int)CbEditService.SelectedValue;
            var masterId = (int)CbEditMaster.SelectedValue;

            if (_currentReg == null)
            {
                _currentReg = new Registrations();
                _context.Registrations.Add(_currentReg);
            }

            _currentReg.RegistrationDate = DpEditDate.SelectedDate.Value;
            _currentReg.RegistrationTime = ts;
            _currentReg.RegistrationSum = sum;
            _currentReg.RegSurnameClient = TbEditSurname.Text.Trim();
            _currentReg.RegNameClient = TbEditName.Text.Trim();
            _currentReg.RegPatronymicClient = TbEditPatronymic.Text.Trim();
            _currentReg.RegPhoneNumberClient = TbEditPhone.Text.Trim();
            _currentReg.IDStatus = statusId;
            _currentReg.RegServiceID = serviceId;
            _currentReg.IDMaster = masterId;

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

            LoadRegistrations();
            EditPanel.Visibility = Visibility.Collapsed;
            ClearEditFields();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Collapsed;
            ClearEditFields();
        }

        private void ClearEditFields()
        {
            DpEditDate.SelectedDate = null;
            TbEditTime.Clear();
            TbEditSum.Clear();
            TbEditSurname.Clear();
            TbEditName.Clear();
            TbEditPatronymic.Clear();
            TbEditPhone.Clear();
            CbEditStatus.SelectedIndex = -1;
            CbEditService.SelectedIndex = -1;
            CbEditMaster.SelectedIndex = -1;
            _currentReg = null;
        }

        private void CbEditService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbEditService.SelectedItem is Services selectedService)
            {
                TbEditSum.Text = selectedService.ServiceCost.ToString("F2");
            }
        }

        private void TbEditTime_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var input = e.Text;

            if (!char.IsDigit(input, 0) && input != ":")
            {
                e.Handled = true;
                return;
            }

            string currentText = textBox.Text + input;

            var textbox = sender as TextBox;
            string currenttext = textBox.Text;
            string Input = e.Text;

            if (!char.IsDigit(input, 0) && input != ":" && input != " ")
            {
                e.Handled = true;
                return;
            }

            if (input == " ")
            {
                if (currentText.Length <= 2 && !currentText.Contains(":"))
                {
                    textBox.Text += ":";
                    textBox.CaretIndex = textBox.Text.Length;
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                }
                return;
            }

            if (input != ":" && currentText.Length == 3 && !currentText.Contains(":"))
            {
                textBox.Text += ":";
                textBox.CaretIndex = textBox.Text.Length;
            }

            if ((textBox.Text + input).Length > 5)
            {
                e.Handled = true;
                return;
            }

            if (input == ":" && currentText.Contains(":"))
            {
                e.Handled = true;
            }

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text.Trim().ToLower();

            var masterIDs = _context.Masters
                                    .Where(m => m.MasterSurname.ToLower().Contains(searchText))
                                    .Select(m => m.MasterID)
                                    .ToList();

            var query = _context.Registrations
                                .Where(r => masterIDs.Contains(r.IDMaster))
                                .ToList();

            RecordsDataGrid.ItemsSource = query;
        }

        private bool _isUpdatingText = false;

        private void TbEditPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdatingText) return;

            var tb = sender as TextBox;
            string rawText = tb.Text;

            string digits = new string(rawText.Where(char.IsDigit).ToArray());

            if (digits.Length > 11)
                digits = digits.Substring(0, 11);

            StringBuilder formatted = new StringBuilder();

            if (!string.IsNullOrEmpty(digits))
            {
                formatted.Append("+7");

                if (digits.Length > 1)
                {
                    formatted.Append("(");
                    formatted.Append(digits.Substring(1, Math.Min(3, digits.Length - 1)));
                    formatted.Append(")");
                }

                if (digits.Length > 4)
                {
                    formatted.Append(" ");
                    formatted.Append(digits.Substring(4, Math.Min(3, digits.Length - 4)));
                }

                if (digits.Length > 7)
                {
                    formatted.Append("-");
                    formatted.Append(digits.Substring(7, Math.Min(2, digits.Length - 7)));
                }

                if (digits.Length > 9)
                {
                    formatted.Append("-");
                    formatted.Append(digits.Substring(9, digits.Length - 9));
                }
            }

            try
            {
                _isUpdatingText = true;
                tb.Text = formatted.ToString();
                tb.CaretIndex = tb.Text.Length;
            }
            finally
            {
                _isUpdatingText = false;
            }
        }
    }
}
