using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Studio_Rent_Service
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Запускаем с окна авторизации
            var authWindow = new Views.AuthWindow();
            authWindow.Show();
        }
    }
}

