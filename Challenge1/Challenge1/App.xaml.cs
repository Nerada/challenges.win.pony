using Challenge1.Support.Dialogs;

using System.Windows;

namespace Challenge1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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
                Current.MainWindow = mainWindow;
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