using Prism.Mvvm;

namespace Virtual_Pet.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Virtual Pet";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
