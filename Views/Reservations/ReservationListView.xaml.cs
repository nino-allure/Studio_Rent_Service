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
using Studio_Rent_Service.Models;
using Studio_Rent_Service.ViewModels;

namespace Studio_Rent_Service.Views.Reservations
{
    /// <summary>
    /// Логика взаимодействия для ReservationListView.xaml
    /// </summary>
    public partial class ReservationListView : Window
    {
        private ReservationViewModel viewModel;
        private string currentStatusFilter = "all";
        private DateTime? filterDate = null;
        private Studio selectedStudio = null;

        public ReservationListView()
        {
            InitializeComponent();
            viewModel = (ReservationViewModel)this.DataContext;
            ApplyFilters();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ReservationEditView(null, viewModel);
            if (editWindow.ShowDialog() == true)
            {
                ApplyFilters();
                UpdateStatus("Бронирование добавлено успешно");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int reservationId)
            {
                var reservation = viewModel.Reservations.FirstOrDefault(r => r.Id == reservationId);
                if (reservation != null)
                {
                    var editWindow = new ReservationEditView(reservation, viewModel);
                    if (editWindow.ShowDialog() == true)
                    {
                        ApplyFilters();
                        UpdateStatus("Бронирование обновлено успешно");
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int reservationId)
            {
                var reservation = viewModel.Reservations.FirstOrDefault(r => r.Id == reservationId);
                if (reservation != null)
                {
                    var result = MessageBox.Show(
                        $"Удалить бронирование #{reservation.Code} от {reservation.BookingDate:dd.MM.yyyy}?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        viewModel.DeleteReservation(reservationId);
                        ApplyFilters();
                        UpdateStatus($"Бронирование #{reservation.Code} удалено");
                    }
                }
            }
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int reservationId)
            {
                var reservation = viewModel.Reservations.FirstOrDefault(r => r.Id == reservationId);
                if (reservation != null && reservation.Status == "Активно")
                {
                    reservation.Status = "Завершено";
                    ApplyFilters();
                    UpdateStatus($"Бронирование #{reservation.Code} завершено");
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int reservationId)
            {
                var reservation = viewModel.Reservations.FirstOrDefault(r => r.Id == reservationId);
                if (reservation != null && reservation.Status == "Активно")
                {
                    var result = MessageBox.Show(
                        $"Отменить бронирование #{reservation.Code}?",
                        "Подтверждение отмены",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        reservation.Status = "Отменено";
                        ApplyFilters();
                        UpdateStatus($"Бронирование #{reservation.Code} отменено");
                    }
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DpFilterDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterDate = dpFilterDate.SelectedDate;
            ApplyFilters();
        }

        private void CbStudioFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStudio = cbStudioFilter.SelectedItem as Studio;
            ApplyFilters();
        }

        private void StatusFilter_Checked(object sender, RoutedEventArgs e)
        {
            if (rbFilterAll.IsChecked == true)
                currentStatusFilter = "all";
            else if (rbFilterActive.IsChecked == true)
                currentStatusFilter = "active";
            else if (rbFilterCompleted.IsChecked == true)
                currentStatusFilter = "completed";
            else if (rbFilterCancelled.IsChecked == true)
                currentStatusFilter = "cancelled";

            ApplyFilters();
        }

        private void BtnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            dpFilterDate.SelectedDate = null;
            cbStudioFilter.SelectedItem = null;
            rbFilterAll.IsChecked = true;

            filterDate = null;
            selectedStudio = null;
            currentStatusFilter = "all";

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<Reservation> filtered = viewModel.Reservations;

            // Фильтрация по текстовому поиску
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchText = txtSearch.Text.ToLower();
                filtered = filtered.Where(r =>
                    r.Client.FullName.ToLower().Contains(searchText) ||
                    r.Studio.Name.ToLower().Contains(searchText) ||
                    r.Code.ToLower().Contains(searchText) ||
                    r.Comment.ToLower().Contains(searchText));
            }

            // Фильтрация по дате
            if (filterDate.HasValue)
            {
                filtered = filtered.Where(r => r.BookingDate.Date == filterDate.Value.Date);
                txtFilterInfo.Text = $"Фильтр: {filterDate.Value:dd.MM.yyyy}";
            }
            else
            {
                txtFilterInfo.Text = "Показаны все бронирования";
            }

            // Фильтрация по студии
            if (selectedStudio != null)
            {
                filtered = filtered.Where(r => r.Studio.Id == selectedStudio.Id);
                txtFilterInfo.Text += $" | Студия: {selectedStudio.Name}";
            }

            // Фильтрация по статусу
            switch (currentStatusFilter)
            {
                case "active":
                    filtered = filtered.Where(r => r.Status == "Активно");
                    txtFilterInfo.Text += " | Только активные";
                    break;
                case "completed":
                    filtered = filtered.Where(r => r.Status == "Завершено");
                    txtFilterInfo.Text += " | Только завершенные";
                    break;
                case "cancelled":
                    filtered = filtered.Where(r => r.Status == "Отменено");
                    txtFilterInfo.Text += " | Только отмененные";
                    break;
            }

            dgReservations.ItemsSource = filtered.OrderBy(r => r.BookingDate).ThenBy(r => r.StartTime);
        }

        private void DgReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Можно добавить логику для показа деталей выделенного бронирования
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }
    }
}