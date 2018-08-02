using DiscordVentriloquist.ViewModels;
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

namespace DiscordVentriloquist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e) {
            base.OnInitialized(e);

            var data = Properties.Settings.Default.AutoSave;
            if (!string.IsNullOrEmpty(data)) (DataContext as MainVM)?.LoadFromText(data, true);
        }

        protected override void OnClosed(EventArgs e) {
            var data = (DataContext as MainVM)?.SaveToText(true);
            if (!string.IsNullOrEmpty(data)) {
                Properties.Settings.Default.AutoSave = data;
                Properties.Settings.Default.Save();
            }

            base.OnClosed(e);
        }

        private void MessageBoxKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) != 0) {
                e.Handled = true;
                var tb = sender as TextBox;
                tb.SelectedText = "\n";
                tb.SelectionLength = 0;
                tb.SelectionStart += 1;
            }
        }
    }
}
