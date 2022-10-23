namespace VirtualPet.Services.Interfaces
{
    /// <summary>
    /// Paths for the images used in this program.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Gets the path of the program icon.
        /// </summary>
        /// <remarks>
        /// PNG, not ICO format.
        /// </remarks>
        /// <returns>The path of the program icon.</returns>
        public string GetIconPath();

        /// <summary>
        /// Gets the path of the cemetery background.
        /// </summary>
        /// <returns>The path of the cemetery background image.</returns>
        public string GetCemeteryBackgroundPath();
    }
}
