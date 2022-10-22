using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;
using VirtualPet.Business.Models;
using System.Collections.ObjectModel;
using System.IO;
using VirtualPet.Modules.Game.Views;
using VirtualPet.Core;

using System.Diagnostics;

namespace VirtualPet.Modules.Game.ViewModels
{
    public class CemeteryViewModel : BindableBase, INavigationAware
    {
        // List of all pets that are currently dead
        private List<Pet> _deadPets;

        /// <summary>
        /// List of all pets that are currently dead.
        /// </summary>
        public List<Pet> DeadPets => _deadPets;

        // Number of ticks survived by the user (either so far or in total)
        private int _ticksSurvived;
        public int TicksSurvived
        {
            get { return _ticksSurvived; }
        }

        private bool _allPetsDead;
        public bool AllPetsDead
        {
            get { return _allPetsDead; }
        }

        // Background image for the usercontrol
        private readonly string _meadowImage = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\VirtualPet\Images\meadow.jpg");
        public string MeadowImage
        {
            get { return _meadowImage; }
        }

        // Command to return the user to the gameplay view, only available if the user still has live pets
        private DelegateCommand _returnToGame;
        public DelegateCommand ReturnToGame =>
            _returnToGame ?? (_returnToGame = new DelegateCommand(ExecuteReturnToGame, CanExecuteReturnToGame));

        void ExecuteReturnToGame()
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(Gameplay));
        }

        bool CanExecuteReturnToGame()
        {
            // The user can navigate back to gameplay if at least one of their pets are still alive
            return !AllPetsDead;
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
            _deadPets = navigationContext.Parameters.GetValue<ObservableCollection<Pet>>("DeadPets").ToList();
            _ticksSurvived = navigationContext.Parameters.GetValue<int>("TicksSurvived");
            _allPetsDead = navigationContext.Parameters.GetValue<bool>("AllPetsDead");

            RaisePropertyChanged(nameof(DeadPets));
            RaisePropertyChanged(nameof(AllPetsDead));
            RaisePropertyChanged(nameof(TicksSurvived));
            ReturnToGame.RaiseCanExecuteChanged();
        }

        private readonly IRegionManager _regionManager;

        public CemeteryViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
