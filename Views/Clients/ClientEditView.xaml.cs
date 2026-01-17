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

namespace Studio_Rent_Service.Views.Clients
{
    /// <summary>
    /// Логика взаимодействия для ClientEditView.xaml
    /// </summary>
    public partial class ClientEditView : Window
    {
        private ClientViewModel parentViewModel;
        private Client currentClient;

        public ClientEditView(Client client, ClientViewModel viewModel)
        {
            InitializeComponent();
            parentViewModel = viewModel;
            currentClient = client;

            if (client == null)
            {
                DataContext = new EditViewModel(parentViewModel);
            }
            else
            {
                DataContext = new EditViewModel(parentViewModel, client);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var editViewModel = (EditViewModel)DataContext;

            if (string.IsNullOrWhiteSpace(editViewModel.LastName) ||
                string.IsNullOrWhiteSpace(editViewModel.FirstName))
            {
                MessageBox.Show("Введите фамилию и имя клиента", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentClient == null)
            {
                // Добавление нового
                var newClient = new Client
                {
                    Id = parentViewModel.GetNextId(),
                    Code = editViewModel.Code,
                    LastName = editViewModel.LastName,
                    FirstName = editViewModel.FirstName,
                    MiddleName = editViewModel.MiddleName,
                    Phone = editViewModel.Phone,
                    Email = editViewModel.Email,
                    AdditionalInfo = editViewModel.AdditionalInfo
                };

                parentViewModel.AddClient(newClient);
            }
            else
            {
                // Обновление существующего
                currentClient.Code = editViewModel.Code;
                currentClient.LastName = editViewModel.LastName;
                currentClient.FirstName = editViewModel.FirstName;
                currentClient.MiddleName = editViewModel.MiddleName;
                currentClient.Phone = editViewModel.Phone;
                currentClient.Email = editViewModel.Email;
                currentClient.AdditionalInfo = editViewModel.AdditionalInfo;

                parentViewModel.UpdateClient(currentClient);
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public class EditViewModel
        {
            private ClientViewModel parentViewModel;

            public string WindowTitle { get; set; }
            public string Code { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string AdditionalInfo { get; set; }

            public EditViewModel(ClientViewModel viewModel)
            {
                parentViewModel = viewModel;
                WindowTitle = "Добавление клиента";
                Code = parentViewModel.GetNextCode();
                LastName = "";
                FirstName = "";
                MiddleName = "";
                Phone = "";
                Email = "";
                AdditionalInfo = "";
            }

            public EditViewModel(ClientViewModel viewModel, Client client)
            {
                parentViewModel = viewModel;
                WindowTitle = $"Редактирование клиента {client.Code}";
                Code = client.Code;
                LastName = client.LastName;
                FirstName = client.FirstName;
                MiddleName = client.MiddleName;
                Phone = client.Phone;
                Email = client.Email;
                AdditionalInfo = client.AdditionalInfo;
            }
        }
    }
}
