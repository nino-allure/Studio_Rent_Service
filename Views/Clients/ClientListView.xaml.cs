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

namespace Studio_Rent_Service.Views.Clients
{
    /// <summary>
    /// Логика взаимодействия для ClientListView.xaml
    /// </summary>
    public partial class ClientListView : Window
    {
        private ClientViewModel viewModel;

        public ClientListView()
        {
            InitializeComponent();
            viewModel = (ClientViewModel)this.DataContext;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ClientEditView(null, viewModel);
            if (editWindow.ShowDialog() == true)
            {
                dgClients.Items.Refresh();
                UpdateStatus("Клиент добавлен успешно");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int clientId)
            {
                var client = viewModel.Clients.FirstOrDefault(c => c.Id == clientId);
                if (client != null)
                {
                    var editWindow = new ClientEditView(client, viewModel);
                    if (editWindow.ShowDialog() == true)
                    {
                        dgClients.Items.Refresh();
                        UpdateStatus("Клиент обновлен успешно");
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int clientId)
            {
                var client = viewModel.Clients.FirstOrDefault(c => c.Id == clientId);
                if (client != null)
                {
                    var result = MessageBox.Show(
                        $"Удалить клиента {client.FullName}?",
                        "Подтверждение удаления",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        viewModel.DeleteClient(clientId);
                        UpdateStatus($"Клиент удален");
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
            var searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                dgClients.ItemsSource = viewModel.Clients;
            }
            else
            {
                var filtered = viewModel.Clients
                    .Where(c => c.FullName.ToLower().Contains(searchText) ||
                               c.Code.ToLower().Contains(searchText) ||
                               c.Phone.ToLower().Contains(searchText) ||
                               c.Email.ToLower().Contains(searchText))
                    .ToList();

                dgClients.ItemsSource = filtered;
            }
        }

        private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            dgClients.ItemsSource = viewModel.Clients;
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }
    }
}