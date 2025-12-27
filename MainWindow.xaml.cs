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
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Загрузка статистики
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            // Здесь будет загрузка реальной статистики
            txtStudioCount.Text = "Студии: 3";
            txtClientCount.Text = "Клиенты: 5";
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
            var reservationWindow = new Views.Reservations.ReservationListView();
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
    }
}

