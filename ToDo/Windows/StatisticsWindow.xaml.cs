using System.Windows;
using ToDo.Data.Models;

namespace ToDo.Windows
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(Stats stats)
        {
            InitializeComponent();
            DataContext = stats;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
