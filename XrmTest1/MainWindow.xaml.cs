using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XrmTest1.Core;

namespace XrmTest1
{
    /// <summary>
    /// Представление главного окна
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            if (row == null)
                return;
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource == null)
                return;
            if (!(originalSource.DataContext is Resume))
                return;
            row.DetailsVisibility = row.DetailsVisibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
