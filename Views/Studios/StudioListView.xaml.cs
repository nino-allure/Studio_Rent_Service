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
using Studio_Rent_Service.Models;

namespace Studio_Rent_Service.Views.Studios
{
    /// <summary>
    /// Логика взаимодействия для StudioListView.xaml
    /// </summary>
    public partial class StudioListView : Window
    {
        private StudioViewModel viewModel;

        public StudioListView()
        {
            InitializeComponent();
            viewModel = (StudioViewModel)this.DataContext;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new StudioEditView(null, viewModel);
            if (editWindow.ShowDialog() == true)
            {
                dgStudios.Items.Refresh();
                UpdateStatus("Студия добавлена успешно");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int studioId)
            {
                var studio = viewModel.Studios.FirstOrDefault(s => s.Id == studioId);
                if (studio != null)
                {
                    var editWindow = new StudioEditView(studio, viewModel);
                    if (editWindow.ShowDialog() == true)
                    {
                        dgStudios.Items.Refresh();
                        UpdateStatus("Студия обновлена успешно");
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int studioId)
            {
                var studio = viewModel.Studios.FirstOrDefault(s => s.Id == studioId);
                if (studio != null)
                {
                    var result = MessageBox.Show(
                        $"Удалить студию {studio.Name}?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        viewModel.DeleteStudio(studioId);
                        UpdateStatus($"Студия удалена");
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
            // Реализация поиска
            var searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                dgStudios.ItemsSource = viewModel.Studios;
            }
            else
            {
                var filtered = viewModel.Studios
                    .Where(s => s.Name.ToLower().Contains(searchText) ||
                               s.Code.ToLower().Contains(searchText) ||
                               s.Address.ToLower().Contains(searchText))
                    .ToList();

                dgStudios.ItemsSource = filtered;
            }
        }

        private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            dgStudios.ItemsSource = viewModel.Studios;
        }

        private void DgStudios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика выделения
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }
    }
}