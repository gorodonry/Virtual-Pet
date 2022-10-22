namespace VirtualPet.Services.Interfaces
{
    /// <summary>
    /// Paths for the images used in this program.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Gets the path of the program icon (PNG format).
        /// </summary>
        /// <returns>The path of the icon.</returns>
        public string GetIconPath();
    }
}
