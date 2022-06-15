using System.Windows;

namespace Virtual_Pet.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        void ClearTeachingInput(object sender, RoutedEventArgs e)
        {
            if (TeachingInput is not null)
            {
                TeachingInput.Text = string.Empty;
            }
        }
    }
}
