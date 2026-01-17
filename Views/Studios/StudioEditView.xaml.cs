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

namespace Studio_Rent_Service.Views.Studios
{
    /// <summary>
    /// Логика взаимодействия для StudioEditView.xaml
    /// </summary>
    public partial class StudioEditView : Window
    {
        private StudioViewModel parentViewModel;
        private Studio currentStudio;

        public StudioEditView(Studio studio, StudioViewModel viewModel)
        {
            InitializeComponent();
            parentViewModel = viewModel;
            currentStudio = studio;

            if (studio == null)
            {
                // Новый объект
                DataContext = new EditViewModel(parentViewModel);
            }
            else
            {
                // Редактирование существующего
                DataContext = new EditViewModel(parentViewModel, studio);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var editViewModel = (EditViewModel)DataContext;

            if (string.IsNullOrWhiteSpace(editViewModel.Name))
            {
                MessageBox.Show("Введите название студии", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (editViewModel.RentalCost <= 0)
            {
                MessageBox.Show("Введите корректную стоимость аренды", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentStudio == null)
            {
                // Добавление нового
                var newStudio = new Studio
                {
                    Id = parentViewModel.GetNextId(),
                    Code = editViewModel.Code,
                    Name = editViewModel.Name,
                    Address = editViewModel.Address,
                    Equipment = editViewModel.Equipment,
                    RentalCost = editViewModel.RentalCost
                };

                parentViewModel.AddStudio(newStudio);
            }
            else
            {
                // Обновление существующего
                currentStudio.Code = editViewModel.Code;
                currentStudio.Name = editViewModel.Name;
                currentStudio.Address = editViewModel.Address;
                currentStudio.Equipment = editViewModel.Equipment;
                currentStudio.RentalCost = editViewModel.RentalCost;

                parentViewModel.UpdateStudio(currentStudio);
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
            private StudioViewModel parentViewModel;

            public string WindowTitle { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Equipment { get; set; }
            public decimal RentalCost { get; set; }

            public EditViewModel(StudioViewModel viewModel)
            {
                parentViewModel = viewModel;
                WindowTitle = "Добавление студии";
                Code = parentViewModel.GetNextCode();
                Name = "";
                Address = "";
                Equipment = "";
                RentalCost = 0;
            }

            public EditViewModel(StudioViewModel viewModel, Studio studio)
            {
                parentViewModel = viewModel;
                WindowTitle = $"Редактирование студии {studio.Code}";
                Code = studio.Code;
                Name = studio.Name;
                Address = studio.Address;
                Equipment = studio.Equipment;
                RentalCost = studio.RentalCost;
            }
        }
    }
}
