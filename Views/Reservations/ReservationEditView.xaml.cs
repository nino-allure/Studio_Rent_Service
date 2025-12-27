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
    /// Логика взаимодействия для ReservationEditView.xaml
    /// </summary>
    public partial class ReservationEditView : Window
    {
        public partial class ReservationEditView : Window
        {
            private ReservationViewModel parentViewModel;
            private Reservation currentReservation;

            public ReservationEditView(Reservation reservation, ReservationViewModel viewModel)
            {
                InitializeComponent();
                parentViewModel = viewModel;
                currentReservation = reservation;

                if (reservation == null)
                {
                    // Новый объект
                    DataContext = new EditViewModel(parentViewModel);
                }
                else
                {
                    // Редактирование существующего
                    DataContext = new EditViewModel(parentViewModel, reservation);
                }
            }

            private void BtnSave_Click(object sender, RoutedEventArgs e)
            {
                var editViewModel = (EditViewModel)DataContext;

                // Валидация
                if (editViewModel.SelectedStudio == null)
                {
                    MessageBox.Show("Выберите студию", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (editViewModel.SelectedClient == null)
                {
                    MessageBox.Show("Выберите клиента", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (editViewModel.BookingDate < DateTime.Today)
                {
                    MessageBox.Show("Дата бронирования не может быть в прошлом", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(editViewModel.SelectedStartTime))
                {
                    MessageBox.Show("Выберите время начала", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(editViewModel.SelectedDuration))
                {
                    MessageBox.Show("Выберите продолжительность", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(editViewModel.SelectedStatus))
                {
                    MessageBox.Show("Выберите статус", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Извлечение числовых значений
                var durationHours = int.Parse(editViewModel.SelectedDuration.Split(' ')[0]);
                var startTime = TimeSpan.Parse(editViewModel.SelectedStartTime);

                // Проверка доступности студии
                if (!parentViewModel.IsStudioAvailable(
                    editViewModel.SelectedStudio,
                    editViewModel.BookingDate,
                    startTime,
                    durationHours,
                    currentReservation?.Id ?? 0))
                {
                    MessageBox.Show($"Студия '{editViewModel.SelectedStudio.Name}' уже забронирована на это время!",
                        "Конфликт бронирования", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Расчет стоимости
                var cost = parentViewModel.CalculateCost(editViewModel.SelectedStudio, durationHours);

                if (currentReservation == null)
                {
                    // Добавление нового
                    var newReservation = new Reservation
                    {
                        Id = parentViewModel.GetNextId(),
                        Code = parentViewModel.GetNextCode(),
                        BookingDate = editViewModel.BookingDate,
                        StartTime = startTime,
                        DurationHours = durationHours,
                        Studio = editViewModel.SelectedStudio,
                        Client = editViewModel.SelectedClient,
                        Status = editViewModel.SelectedStatus,
                        Cost = cost,
                        Comment = editViewModel.Comment
                    };

                    parentViewModel.AddReservation(newReservation);
                }
                else
                {
                    // Обновление существующего
                    currentReservation.BookingDate = editViewModel.BookingDate;
                    currentReservation.StartTime = startTime;
                    currentReservation.DurationHours = durationHours;
                    currentReservation.Studio = editViewModel.SelectedStudio;
                    currentReservation.Client = editViewModel.SelectedClient;
                    currentReservation.Status = editViewModel.SelectedStatus;
                    currentReservation.Cost = cost;
                    currentReservation.Comment = editViewModel.Comment;

                    parentViewModel.UpdateReservation(currentReservation);
                }

                DialogResult = true;
                Close();
            }

            private void BtnCancel_Click(object sender, RoutedEventArgs e)
            {
                DialogResult = false;
                Close();
            }

            // ViewModel для окна редактирования
            public class EditViewModel
            {
                private ReservationViewModel parentViewModel;

                public string WindowTitle { get; set; }
                public string Code { get; set; }
                public DateTime BookingDate { get; set; }
                public string SelectedStartTime { get; set; }
                public string SelectedDuration { get; set; }
                public Studio SelectedStudio { get; set; }
                public Client SelectedClient { get; set; }
                public string SelectedStatus { get; set; }
                public decimal Cost { get; set; }
                public string Comment { get; set; }

                public string[] AvailableTimes { get; } = new[]
                {
                "08:00", "09:00", "10:00", "11:00", "12:00", "13:00",
                "14:00", "15:00", "16:00", "17:00", "18:00", "19:00",
                "20:00", "21:00", "22:00"
            };

                public string[] Durations { get; } = new[]
                {
                "1 час", "2 часа", "3 часа", "4 часа", "5 часов", "6 часов", "8 часов"
            };

                public string[] Statuses { get; } = new[] { "Активно", "Завершено", "Отменено" };

                public System.Collections.Generic.List<Studio> Studios => parentViewModel.Studios.ToList();
                public System.Collections.Generic.List<Client> Clients => parentViewModel.Clients.ToList();

                public string CostCalculationInfo
                {
                    get
                    {
                        if (SelectedStudio != null && !string.IsNullOrEmpty(SelectedDuration))
                        {
                            int hours = int.Parse(SelectedDuration.Split(' ')[0]);
                            return $"{hours} ч × {SelectedStudio.RentalCost:N0} ₽/ч = {hours * SelectedStudio.RentalCost:N0} ₽";
                        }
                        return "Выберите студию и продолжительность для расчета";
                    }
                }

                public EditViewModel(ReservationViewModel viewModel)
                {
                    parentViewModel = viewModel;
                    WindowTitle = "Добавление бронирования";
                    Code = parentViewModel.GetNextCode();
                    BookingDate = DateTime.Today;
                    SelectedStartTime = "10:00";
                    SelectedDuration = "2 часа";
                    SelectedStudio = Studios.FirstOrDefault();
                    SelectedClient = Clients.FirstOrDefault();
                    SelectedStatus = "Активно";
                    Cost = 0;
                    Comment = "";
                }

                public EditViewModel(ReservationViewModel viewModel, Reservation reservation)
                {
                    parentViewModel = viewModel;
                    WindowTitle = $"Редактирование бронирования #{reservation.Code}";
                    Code = reservation.Code;
                    BookingDate = reservation.BookingDate;
                    SelectedStartTime = reservation.StartTime.ToString(@"hh\:mm");
                    SelectedDuration = $"{reservation.DurationHours} час{(reservation.DurationHours > 1 ? "а" : "")}";
                    SelectedStudio = reservation.Studio;
                    SelectedClient = reservation.Client;
                    SelectedStatus = reservation.Status;
                    Cost = reservation.Cost;
                    Comment = reservation.Comment;
                }
            }
        }
    }
}
