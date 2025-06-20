using SalonPhenomenon.Modules;
using SalonPhenomenon.Pages.PagesForUsers;
using SalonPhenomenon.Utils;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SalonPhenomenon.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }
        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "")
            {
                LoginPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "")
            {
                LoginPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPBox.Password == "")
            {
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordPBox.Password == "")
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void LogINBT_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrEmpty(LoginTextBox.Text))
            {
                error.AppendLine("Введите логин");
            }

            if (string.IsNullOrEmpty(PasswordPBox.Password))
            {
                error.AppendLine("Введите пароль");
            }
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Users userobj = SalonPhenEntities.GetContext().Users.FirstOrDefault(u => u.UserLogin == LoginTextBox.Text);

                if (userobj == null)
                {
                    error.AppendLine("Пользователь не найден");
                    MessageBox.Show(error.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (userobj.IsBlocked)
                {
                    MessageBox.Show("Ваш аккаунт заблокирован. Обратитесь к администратору.");
                    return;
                }


                if (userobj.UserPassword != PasswordPBox.Password)
                {
                    userobj.FailedAttempts += 1;

                    if (userobj.FailedAttempts >= 3)
                    {
                        userobj.IsBlocked = true;
                        MessageBox.Show("Вы ввели неправильные данные 3 раза. Аккаунт заблокирован.");
                    }
                    else
                    {
                        MessageBox.Show($"Неверный пароль. Осталось попыток: {3 - userobj.FailedAttempts}");
                    }

                    SalonPhenEntities.GetContext().SaveChanges();
                    return;
                }

                userobj.FailedAttempts = 0;
                userobj.LastLoginDate = DateTime.Now;
                SalonPhenEntities.GetContext().SaveChanges();

                Manager.AuthUser = userobj;

                switch (userobj.RoleID)
                {
                    case 1:
                        Manager.MainFrame.Navigate(new RegistrationAdmin());
                        break;
                    case 2:
                        Manager.MainFrame.Navigate(new RecordsPage());
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя.");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
