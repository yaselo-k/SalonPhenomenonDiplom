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
    /// Логика взаимодействия для MastersListPage.xaml
    /// </summary>
    public partial class MastersListPage : Page
    {
        private readonly SalonEntities _context;
        private Masters _currentMaster;
        public MastersListPage()
        {
            InitializeComponent();
            _context = SalonEntities.GetContext();
            LoadMasters();
            LoadEditLists();
        }
        private void LoadMasters()
        {
            using (var context = new SalonEntities())
            {
                MastersDataGrid.ItemsSource = _context.Masters.ToList();
                MasterEditBtn.IsEnabled = false;
                MasterAddBtn.IsEnabled = false;
            }
        }
        private void LoadEditLists()
        {
            // Специализации
            MasterSpecCB.ItemsSource = _context.Specializations.OrderBy(s => s.SpecializationName).ToList();
            MasterSpecCB.DisplayMemberPath = "SpecializationName";
            MasterSpecCB.SelectedValuePath = "SpecializationID";

        }
        private void MasterDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = MastersDataGrid.SelectedItem != null;
            MasterEditBtn.IsEnabled = sel;
            MasterAddBtn.IsEnabled = sel;
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
                    var entry = context.Masters.Find(master.MasterID);
                    if (entry != null)
                    {
                        context.Masters.Remove(entry);
                        context.SaveChanges();
                        LoadMasters();
                    }
                }
            }
        }

        private void MasterAddBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentMaster = null;
            ClearEditFields();
            EditMasterPanel.Visibility = Visibility.Visible;
        }

        private void MasterEditBtn_Click(object sender, RoutedEventArgs e)
        {
            EditMasterPanel.Visibility = Visibility.Visible;

            if (MastersDataGrid.SelectedItem is Masters sel)
            {
                _currentMaster = _context.Masters.Find(sel.MasterID);
                if (_currentMaster != null)
                {
                    MasterSurnameTB.Text = _currentMaster.MasterSurname;
                    MasterNameTB.Text = _currentMaster.MasterName;
                    MasterPatTB.Text = _currentMaster.MasterPatronymic;
                    MasterSpecCB.SelectedValue = _currentMaster.MasterSpecialization;
                    EditMasterPanel.Visibility = Visibility.Visible;
                }
            }
        }
        private void ClearEditFields()
        {
            MasterSurnameTB.Clear();
            MasterNameTB.Clear();
            MasterPatTB.Clear();
            MasterSpecCB.SelectedIndex = -1;
            _currentMaster = null;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            EditMasterPanel.Visibility = Visibility.Collapsed;
            ClearEditFields();
        }
        private bool ContainsDigit(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Any(char.IsDigit);
        }
        private bool ContainsSpecialChars(string input)
        {
            // Список запрещённых символов
            char[] invalidChars = "!@#$%^&*(){}[]|\\\"'<>?,./".ToCharArray();

            return input.Any(c => invalidChars.Contains(c));
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(MasterSurnameTB.Text))
                sb.AppendLine("• Введите фамилию мастера.");
            else if (ContainsDigit(MasterSurnameTB.Text))
                sb.AppendLine("• Фамилия не должна содержать цифры.");
            else if (ContainsSpecialChars(MasterSurnameTB.Text))
                sb.AppendLine("• Фамилия содержит недопустимые символы.");

            if (string.IsNullOrWhiteSpace(MasterNameTB.Text))
                sb.AppendLine("• Введите имя мастера.");
            else if (ContainsDigit(MasterNameTB.Text))
                sb.AppendLine("• Имя не должно содержать цифры.");
            else if (ContainsSpecialChars(MasterNameTB.Text))
                sb.AppendLine("• Имя содержит недопустимые символы.");

            if (string.IsNullOrWhiteSpace(MasterPatTB.Text))
                sb.AppendLine("• Введите отчество мастера.");
            else if (ContainsDigit(MasterPatTB.Text))
                sb.AppendLine("• Отчество не должно содержать цифры.");
            else if (ContainsSpecialChars(MasterPatTB.Text))
                sb.AppendLine("• Отчество содержит недопустимые символы.");

            if (MasterSpecCB.SelectedValue == null)
                sb.AppendLine("• Выберите специализацию.");

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString().TrimEnd(), "Ошибка ввода",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Получаем связанные сущности
            var specId = (int)MasterSpecCB.SelectedValue;

            if (_currentMaster == null)
            {
                _currentMaster = new Masters();
                _context.Masters.Add(_currentMaster);
            }

            _currentMaster.MasterSurname = MasterSurnameTB.Text.Trim();
            _currentMaster.MasterName = MasterNameTB.Text.Trim();
            _currentMaster.MasterPatronymic = MasterPatTB.Text.Trim();
            _currentMaster.MasterSpecialization = specId;

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

            LoadMasters();
            EditMasterPanel.Visibility = Visibility.Collapsed;
            ClearEditFields();
        }
    }
}
