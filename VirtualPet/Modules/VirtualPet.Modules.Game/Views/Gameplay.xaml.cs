using System.Windows;
using System.Windows.Controls;

namespace VirtualPet.Modules.Game.Views
{
    /// <summary>
    /// Interaction logic for Gameplay
    /// </summary>
    public partial class Gameplay : UserControl
    {
        public Gameplay()
        {
            InitializeComponent();
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
