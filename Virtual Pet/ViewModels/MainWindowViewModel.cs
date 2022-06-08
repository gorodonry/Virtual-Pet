using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        public static string CheckS(string str)
        {
            // Checks whether to add an s after an apostrophe, returns formatted string
            if (str.ToLower().ToCharArray()[str.Length - 1] == char.Parse("s"))
            {
                return str + "'";
            }
            else
            {
                return str + "'s";
            }
        }

        public static string JoinWithAnd(List<string> iterable)
        {
            // Joins a list of strings together with commas and an and
            if (iterable.Count() == 0)
            {
                return "";
            }
            else if (iterable.Count() == 1)
            {
                return iterable[0];
            }
            else if (iterable.Count() == 2)
            {
                return $"{iterable[0]} and {iterable[1]}";
            }
            else
            {
                return $"{string.Join(", ", iterable.GetRange(0, iterable.Count() - 1))}, and {iterable[iterable.Count() - 1]}";
            }
        }

        public static string Capitalise(string str)
        {
            // Makes the first character of a string upper case then returns the string
            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return string.Join("", chars);
        }

        private bool _nameSelectionVisible = true;
        public bool NameSelectionVisible
        {
            get { return _nameSelectionVisible; }
            set { SetProperty(ref _nameSelectionVisible, value); }
        }

        private bool _gameplayVisible = false;
        public bool GameplayVisible
        {
            get { return _gameplayVisible; }
            set { SetProperty(ref _gameplayVisible, value); }
        }

        // Names of the three pets, user chooses the names
        private string _petOneName = string.Empty;
        public string PetOneName
        {
            get { return _petOneName; }
            set
            {
                SetProperty(ref _petOneName, value.Trim());
                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        private string _petTwoName = string.Empty;
        public string PetTwoName
        {
            get { return _petTwoName; }
            set
            {
                SetProperty(ref _petTwoName, value.Trim());
                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        private string _petThreeName = string.Empty;
        public string PetThreeName
        {
            get { return _petThreeName; }
            set
            {
                SetProperty(ref _petThreeName, value.Trim());
                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        // Command to change from the name selection UI to the gameplay UI
        private DelegateCommand _startPlaying;
        public DelegateCommand StartPlaying =>
            _startPlaying ?? (_startPlaying = new DelegateCommand(ExecuteStartPlaying, CanExecuteStartPlaying));

        void ExecuteStartPlaying()
        {
            NameSelectionVisible = false;
            GameplayVisible = true;
            RaisePropertyChanged(nameof(NameSelectionVisible));
            RaisePropertyChanged(nameof(GameplayVisible));
        }

        bool CanExecuteStartPlaying()
        {
            // A value must be entered for each pet name
            if (PetOneName.Length != 0 && PetTwoName.Length != 0 && PetThreeName.Length != 0)
            {
                // Pet names cannot be repeated
                if (PetOneName.ToLower() != PetTwoName.ToLower() && PetTwoName.ToLower() != PetThreeName.ToLower() && PetThreeName.ToLower() != PetOneName.ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public MainWindowViewModel()
        {

        }
    }
}
