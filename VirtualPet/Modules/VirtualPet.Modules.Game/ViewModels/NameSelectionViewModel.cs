using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;
using VirtualPet.Modules.Game.Views;
using VirtualPet.Core;
using VirtualPet.Services.Interfaces;

namespace VirtualPet.Modules.Game.ViewModels
{
    /// <summary>
    /// View model for the name selection view.
    /// </summary>
    public class NameSelectionViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly string _virtualPetImage;

        /// <summary>
        /// Path of the icon (PNG format).
        /// </summary>
        public string VirtualPetImage => _virtualPetImage;

        private string _petOneName = string.Empty;

        /// <summary>
        /// Name of pet one.
        /// </summary>
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

        /// <summary>
        /// Name of pet two.
        /// </summary>
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

        /// <summary>
        /// Name of pet three.
        /// </summary>
        public string PetThreeName
        {
            get { return _petThreeName; }
            set
            {
                SetProperty(ref _petThreeName, value.Trim());

                StartPlaying.RaiseCanExecuteChanged();
            }
        }

        private bool _enableHannahExtension = true;

        /// <summary>
        /// Boolean dictating whether or not the 'Hannah extenstion' is enabled.
        /// </summary>
        /// <remarks>
        /// Set to true by default.
        /// </remarks>
        public bool EnableHannahExtension
        {
            get { return _enableHannahExtension; }
            set { SetProperty(ref _enableHannahExtension, value); }
        }
        
        private DelegateCommand _startPlaying;
        public DelegateCommand StartPlaying =>
            _startPlaying ?? (_startPlaying = new DelegateCommand(ExecuteStartPlaying, CanExecuteStartPlaying));

        /// <summary>
        /// Start a new game with the specified names and settings.
        /// </summary>
        void ExecuteStartPlaying()
        {
            // Start gameplay
            var parameters = new NavigationParameters
            {
                { "Names", new List<string>(){ PetOneName, PetTwoName, PetThreeName } },
                { "EnableHannahExtension", EnableHannahExtension }
            };

            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(Gameplay), parameters);
        }

        /// <summary>
        /// Set to true if three distinct names have been entered.
        /// </summary>
        /// <returns>A boolean indicating whether or not the user can start playing.</returns>
        bool CanExecuteStartPlaying()
        {
            // A value must be entered for each pet name.
            if (!string.IsNullOrEmpty(PetOneName.Trim()) && !string.IsNullOrEmpty(PetTwoName.Trim()) && !string.IsNullOrEmpty(PetThreeName.Trim()))
            {
                // No two pets can have the same name.
                List<string> names = new() { PetOneName, PetTwoName, PetThreeName };
                if (names.Distinct().Count() == names.Count)
                    return true;
                
                return false;
            }
            
            return false;
        }

        public bool KeepAlive => false;

        private readonly IRegionManager _regionManager;

        public NameSelectionViewModel(IRegionManager regionManager, IImageService imageService)
        {
            _regionManager = regionManager;

            _virtualPetImage = imageService.GetIconPath();
        }
    }
}
