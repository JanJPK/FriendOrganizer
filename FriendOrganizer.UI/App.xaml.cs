﻿using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Startup;
using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Whenever you change the constructor of a class, you have to change this line:
            // var mainWindow = new MainWindow(new MainViewModel(new FriendDataService()));
            // Better replace it with Autofac:
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var mainWindow = container.Resolve<MainWindow>();
            // Resolve method goes to the MainWindow constructor and sees that MainViewModel must be created.
            // It also sees that IFriendDataService must be created.
            mainWindow.Show();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured." + Environment.NewLine + e.Exception.Message,
                "Unexpected Error");
            e.Handled = true;
        }
    }
}