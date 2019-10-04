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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Challenge1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private SolidColorBrush Black = new SolidColorBrush(Colors.Black);
        private SolidColorBrush Gray = new SolidColorBrush(Colors.Gray);

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            txtPlayerName.Foreground = Gray;

            txtPlayerName.GotFocus += DefaultTextbox_Enter;
            txtPlayerName.LostFocus += DefaultTextbox_Leave;

            txtHeight.GotFocus += DefaultTextbox_Enter;
            txtHeight.LostFocus += DefaultTextbox_Leave;
            txtWidth.GotFocus += DefaultTextbox_Enter;
            txtWidth.LostFocus += DefaultTextbox_Leave;
            txtDifficulty.GotFocus += DefaultTextbox_Enter;
            txtDifficulty.LostFocus += DefaultTextbox_Leave;

        }

        private void DefaultTextbox_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Foreground == Black) { return; }
            tb.Text = string.Empty;
            tb.Foreground = Black;
        }    

        private void DefaultTextbox_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if(tb.Text.Trim() != string.Empty) { return; }
            tb.Foreground = Gray;
            // Lets get a new random player name just for fun
            if (tb == txtPlayerName) { tb.Text = _viewModel.RandomPlayer; }
        }
    }
}
