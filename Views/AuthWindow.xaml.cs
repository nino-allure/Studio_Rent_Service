using Studio_Rent_Service.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Studio_Rent_Service.Views
{
        public partial class AuthWindow : Window
        {
            private AuthViewModel authViewModel;

            public AuthWindow()
            {
                InitializeComponent();
                authViewModel = new AuthViewModel();
                Loaded += AuthWindow_Loaded;
            }

            private void AuthWindow_Loaded(object sender, RoutedEventArgs e)
            {
                // Устанавливаем фокус на поле ввода логина при загрузке
                txtUsername.Focus();
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
                var animation = new DoubleAnimation
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
                if (btnLogin == null) return;

                btnLogin.IsEnabled = !string.IsNullOrWhiteSpace(txtUsername.Text) &&
                                    !string.IsNullOrWhiteSpace(txtPassword.Password);

                // Изменение цвета кнопки
                if (btnLogin.IsEnabled)
                {
                    btnLogin.Background = new SolidColorBrush(Color.FromRgb(39, 174, 96)); // #27ae60
                    btnLogin.Foreground = Brushes.White;
                }
                else
                {
                    btnLogin.Background = new SolidColorBrush(Color.FromRgb(189, 195, 199)); // #bdc3c7
                    btnLogin.Foreground = Brushes.Black;
                }
            }

            // Обработчик для нажатия Enter в поле логина
            private void TxtUsername_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    txtPassword.Focus();
                }
            }

            // Обработчик для нажатия Enter в поле пароля
            private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
            {
                if (e.Key == System.Windows.Input.Key.Enter && btnLogin.IsEnabled)
                {
                    BtnLogin_Click(sender, e);
                }
            }
        }
    }