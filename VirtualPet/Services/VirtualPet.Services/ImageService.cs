using System.IO;
using VirtualPet.Services.Interfaces;

namespace VirtualPet.Services
{
    public class ImageService : IImageService
    {
        public string GetIconPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), $@"..\..\..\..\Services\VirtualPet.Services\Images\virtual_pet.png");
        }

        public string GetCemeteryBackgroundPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\Services\VirtualPet.Services\Images\meadow.jpg");
        }
    }
}
