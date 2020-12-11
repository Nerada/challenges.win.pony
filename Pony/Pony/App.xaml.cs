// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.App.xaml.cs
// Created on: 20201211
// -----------------------------------------------

using System.Windows;
using Pony.Support.Dialogs;

namespace Pony
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            //Disable shutdown when the dialog closes
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var dialog = new LanguageDialog();
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var mainWindow = new MainWindow();
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
}