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

namespace DiscordVentriloquist
{
    /// <summary>
    /// Interaction logic for TextWindow.xaml
    /// </summary>
    public partial class TextWindow : Window
    {
        public TextWindow()
        {
            InitializeComponent();
        }

        public static string ShowDialog(string text, string title, bool showCancel, bool readOnly) {
            var win = new TextWindow();
            win.Owner = Application.Current.MainWindow;
            win.Title = title;
            win.MainTextbox.SetCurrentValue(TextBox.TextProperty, text);
            win.CancelButton.SetCurrentValue(VisibilityProperty, showCancel ? Visibility.Visible : Visibility.Collapsed);
            win.CopyButton.SetCurrentValue(VisibilityProperty, readOnly ? Visibility.Visible : Visibility.Collapsed);
            win.MainTextbox.SetCurrentValue(System.Windows.Controls.Primitives.TextBoxBase.IsReadOnlyProperty, readOnly);

            var ok = win.ShowDialog();
            if (ok != true) return null;

            return win.MainTextbox.Text;
        }

        private void CancelClicked(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        private void OkClicked(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void CopyClicked(object sender, RoutedEventArgs e) {
            MainTextbox.SelectAll();
            MainTextbox.Copy();
        }

        protected override void OnActivated(EventArgs e) {
            base.OnActivated(e);
            MainTextbox.Focus();
            MainTextbox.SelectAll();
        }
    }
}
