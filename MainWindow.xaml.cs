using Studio_Rent_Service.Models;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Studio_Rent_Service
{
    public partial class MainWindow : Window
    {
        private AuthViewModel authViewModel;

        public MainWindow()
        {
            InitializeComponent();
            authViewModel = new AuthViewModel(); // Создаем новый AuthViewModel
            ConfigureUIForRole();
            Loaded += MainWindow_Loaded;
        }

        public MainWindow(AuthViewModel authViewModel)
        {
            InitializeComponent();
            this.authViewModel = authViewModel;

            // Настройка интерфейса в зависимости от роли
            ConfigureUIForRole();
            Loaded += MainWindow_Loaded;
        }

        private void ConfigureUIForRole()
        {
            // Проверяем, есть ли authViewModel и CurrentUser
            if (authViewModel == null || authViewModel.CurrentUser == null)
            {
                // Если пользователь не авторизован, скрываем кнопку выхода
                if (btnLogout != null)
                    btnLogout.Visibility = Visibility.Collapsed;

                // Показываем информацию о необходимости авторизации
                if (txtUserInfo != null)
                    txtUserInfo.Text = "Пользователь не авторизован";

                return;
            }

            // Обновляем информацию о пользователе
            if (txtUserInfo != null)
                txtUserInfo.Text = authViewModel.WelcomeMessage;

            // В зависимости от роли пользователя настраиваем интерфейс
            var role = authViewModel.CurrentUser.Role;

            switch (role)
            {
                case UserRole.Admin:
                    // Администратор видит все кнопки
                    if (btnSettings != null)
                        btnSettings.IsEnabled = true;
                    break;

                case UserRole.Client:
                    // Клиенту скрываем настройки
                    if (btnSettings != null)
                        btnSettings.IsEnabled = false;
                    break;

                default:
                    // Для других ролей настройки по умолчанию
                    if (btnSettings != null)
                        btnSettings.IsEnabled = false;
                    break;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка статистики
            UpdateStatistics();

            // Обновление информации о пользователе после загрузки
            if (authViewModel != null && authViewModel.CurrentUser != null && txtUserInfo != null)
            {
                txtUserInfo.Text = authViewModel.WelcomeMessage;
            }
        }

        private void UpdateStatistics()
        {
            // Здесь будет загрузка реальной статистики
            if (txtStudioCount != null)
                txtStudioCount.Text = "Студии: 3";
            if (txtClientCount != null)
                txtClientCount.Text = "Клиенты: 5";
            if (txtReservationCount != null)
                txtReservationCount.Text = "Бронирования: 8";
        }

        private void BtnStudios_Click(object sender, RoutedEventArgs e)
        {
            var studioWindow = new Views.Studios.StudioListView();
            studioWindow.Owner = this;
            studioWindow.ShowDialog();
            UpdateStatistics();
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            var clientWindow = new Views.Clients.ClientListView();
            clientWindow.Owner = this;
            clientWindow.ShowDialog();
            UpdateStatistics();
        }

        private void BtnReservations_Click(object sender, RoutedEventArgs e)
        {
            var reservationWindow = new Views.Reservations.ReservationListView(authViewModel);
            reservationWindow.Owner = this;
            reservationWindow.ShowDialog();
            UpdateStatistics();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Раздел настроек в разработке", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Studio Rent Service v1.0\n\n" +
                          "Система управления Аренды Студий Звукозаписи\n" +
                          "© 2025 Все права защищены",
                          "О программе",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите выйти из системы?",
                                       "Подтверждение выхода",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Сброс текущего пользователя
                if (authViewModel != null)
                {
                    authViewModel.CurrentUser = null;
                }

                // Закрытие главного окна
                this.Close();

                // Открытие нового главного окна (или окна авторизации)
                // Если у вас есть отдельное окно авторизации, используйте его:
                // var loginWindow = new LoginWindow();
                // loginWindow.Show();

                // Или открываем новое главное окно:
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}