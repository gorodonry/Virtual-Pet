using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using VirtualPet.Business.Models;
using VirtualPet.Modules.Game.Views;
using VirtualPet.Core;
using VirtualPet.Services.Interfaces;
using VirtualPet.Modules.Game.Models;

using System.Diagnostics;

namespace VirtualPet.Modules.Game.ViewModels
{
    /// <summary>
    /// View model for the cemetery view.
    /// </summary>
    public class CemeteryViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly CemeteryModel _model = new();

        private readonly string _backgroundImagePath;

        /// <summary>
        /// The path of the background image for the cemetery.
        /// </summary>
        public string BackgroundImagePath => _backgroundImagePath;

        /// <summary>
        /// List of all pets that are currently dead.
        /// </summary>
        public List<Pet> DeadPets => _model.DeadPets;

        /// <summary>
        /// Number of ticks survived by the user.
        /// </summary>
        public int TicksSurvived => _model.TicksSurvived;

        private DelegateCommand _returnToGame;
        public DelegateCommand ReturnToGame => _returnToGame ??= new DelegateCommand(ExecuteReturnToGame, CanExecuteReturnToGame);

        /// <summary>
        /// Returns the user to their game (provided their's one currently in progress).
        /// </summary>
        void ExecuteReturnToGame()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(Gameplay));
        }

        /// <summary>
        /// Indicates whether or not the user can return to their game.
        /// </summary>
        /// <returns>False if the user's pets are all dead, otherwise true.</returns>
        bool CanExecuteReturnToGame()
        {
            return _model.GameOnGoing;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Obtain all necessary information from the gameplay model.
            _model.ImportInformation(navigationContext.Parameters.GetValue<GameplayModel>("GameplayModel"));

            RaisePropertyChanged(nameof(DeadPets));
            RaisePropertyChanged(nameof(TicksSurvived));
            ReturnToGame.RaiseCanExecuteChanged();
        }

        public bool KeepAlive => false;

        private readonly IRegionManager _regionManager;

        public CemeteryViewModel(IRegionManager regionManager, IImageService imageService)
        {
            _regionManager = regionManager;

            _backgroundImagePath = imageService.GetCemeteryBackgroundPath();
        }
    }
}
