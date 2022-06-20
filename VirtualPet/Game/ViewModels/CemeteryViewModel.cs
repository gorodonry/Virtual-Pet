using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;

namespace Game.ViewModels
{
    public class CemeteryViewModel : BindableBase, INavigationAware
    {
        private List<Pet> _pets = new();
        public List<Pet> Pets
        {
            get { return _pets; }
            set { SetProperty(ref _pets, value); }
        }

        public List<Pet> DeadPets
        {
            get
            {
                List<Pet> deadPets = new();

                foreach (Pet pet in Pets)
                {
                    if (!string.IsNullOrEmpty(pet.ReasonForDeath))
                    {
                        deadPets.Add(pet);
                    }
                }

                return deadPets;
            }
        }

        private int _ticksSurvived;
        public int TicksSurvived
        {
            get { return _ticksSurvived; }
            set { SetProperty(ref _ticksSurvived, value); }
        }

        private readonly string _meadowImage = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Game\Images\meadow.jpg");
        public string MeadowImage
        {
            get { return _meadowImage; }
        }

        private DelegateCommand _returnToGame;
        public DelegateCommand ReturnToGame =>
            _returnToGame ?? (_returnToGame = new DelegateCommand(ExecuteReturnToGame, CanExecuteReturnToGame));

        void ExecuteReturnToGame()
        {

        }

        bool CanExecuteReturnToGame()
        {
            return true;
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
            Pets = navigationContext.Parameters.GetValue<ObservableCollection<Pet>>("Pets").ToList();
            TicksSurvived = navigationContext.Parameters.GetValue<int>("TicksSurvived");

            RaisePropertyChanged(nameof(DeadPets));
        }

        public CemeteryViewModel()
        {

        }
    }
}
