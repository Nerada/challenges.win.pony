﻿// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.App.xaml.cs
// Created on: 20210729
// -----------------------------------------------

using System.Windows;
using Pony.Support.Dialogs;

namespace Pony;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private void ApplicationStart(object sender, StartupEventArgs e)
    {
        //Disable shutdown when the dialog closes
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        LanguageDialog dialog = new();
        dialog.ShowDialog();

        if (dialog.DialogResult == true)
        {
            MainWindow mainWindow = new();
            //Re-enable normal shutdown mode.
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Current.MainWindow   = mainWindow;
            mainWindow.Show();
        }
        else
        {
            MessageBox.Show("Unable to load data.", string.Empty, MessageBoxButton.OK);
            Current.Shutdown(-1);
        }
    }
}