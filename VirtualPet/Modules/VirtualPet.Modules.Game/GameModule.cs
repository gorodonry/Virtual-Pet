using VirtualPet.Modules.Game.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using VirtualPet.Modules.Game.ViewModels;
using VirtualPet.Core;

namespace VirtualPet.Modules.Game
{
    public class GameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public GameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(NameSelection));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NameSelection, NameSelectionViewModel>();
            containerRegistry.RegisterForNavigation<Gameplay, GameplayViewModel>();
            containerRegistry.RegisterForNavigation<Cemetery, CemeteryViewModel>();
        }
    }
}
