using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using VirtualPet.Modules.Game;
using VirtualPet.Views;
using VirtualPet.Services.Interfaces;
using VirtualPet.Services;

namespace VirtualPet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICakeService, CakeService>();
            containerRegistry.RegisterSingleton<IImageService, ImageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<GameModule>();
        }
    }
}
