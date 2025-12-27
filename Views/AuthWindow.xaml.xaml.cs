using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Studio_Rent_Service.ViewModels;

namespace Studio_Rent_Service.Views
{
    public partial class AuthWindow : Window
    {
        private AuthViewModel authViewModel;

        public AuthWindow()
        {
            InitializeComponent();
            authViewModel = new AuthViewModel();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (authViewModel.Authenticate(username, password, out string errorMessage))
            {
                // Успешная авторизация
                OpenMainWindow();
            }
            else
            {
                // Показать ошибку
                ShowErrorMessage(errorMessage);
            }
        }

        private void OpenMainWindow()
        {
            // Создаем главное окно с информацией о пользователе
            var mainWindow = new MainWindow(authViewModel);
            mainWindow.Show();

            // Закрываем окно авторизации
            this.Close();
        }

        private void ShowErrorMessage(string message)
        {
            txtErrorMessage.Text = message;
            errorMessageBorder.Visibility = Visibility.Visible;

            // Анимация появления
            errorMessageBorder.Opacity = 0;
            var animation = new System.Windows.Media.Animation.DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            errorMessageBorder.BeginAnimation(OpacityProperty, animation);
        }

        private void HideErrorMessage()
        {
            errorMessageBorder.Visibility = Visibility.Collapsed;
        }

        private void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateLoginButtonState();
            HideErrorMessage();
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateLoginButtonState();
            HideErrorMessage();
        }

        private void UpdateLoginButtonState()
        {
            btnLogin.IsEnabled = !string.IsNullOrWhiteSpace(txtUsername.Text) &&
                                !string.IsNullOrWhiteSpace(txtPassword.Password);

            // Изменение цвета кнопки
            if (btnLogin.IsEnabled)
            {
                btnLogin.Background = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(39, 174, 96)); // #27ae60
            }
            else
            {
                btnLogin.Background = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(189, 195, 199)); // #bdc3c7
            }
        }
    }
}
