// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Challenge1.LanguageDialog.xaml.cs
// Created on: 20191006
// -----------------------------------------------

using System.ComponentModel;
using System.Windows;
using Challenge1.Resources;

namespace Challenge1.Support.Dialogs
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
            ResourceHandler.SetLanguage(ResourceHandler.Language.English);
            DialogResult = true;
            Close();
        }

        private void DutchButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceHandler.SetLanguage(ResourceHandler.Language.Dutch);
            DialogResult = true;
            Close();
        }

        private void ChineseButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceHandler.SetLanguage(ResourceHandler.Language.Chinese);
            DialogResult = true;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e) { DialogResult = true; }
    }
}