// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Pony.LanguageDialog.xaml.cs
// Created on: 20191006
// -----------------------------------------------

using System.ComponentModel;
using System.Windows;
using Pony.Localization;

namespace Pony.Support.Dialogs
{
    /// <summary>
    ///     Interaction logic for Dialog.xaml
    /// </summary>
    public partial class LanguageDialog
    {
        public LanguageDialog()
        {
            InitializeComponent();
            Closing += Window_Closing;
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            LocalizationHandler.SetLanguage(LocalizationHandler.Language.English);
            DialogResult = true;
            Close();
        }

        private void DutchButton_Click(object sender, RoutedEventArgs e)
        {
            LocalizationHandler.SetLanguage(LocalizationHandler.Language.Dutch);
            DialogResult = true;
            Close();
        }

        private void ChineseButton_Click(object sender, RoutedEventArgs e)
        {
            LocalizationHandler.SetLanguage(LocalizationHandler.Language.Chinese);
            DialogResult = true;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e) => DialogResult = true;
    }
}