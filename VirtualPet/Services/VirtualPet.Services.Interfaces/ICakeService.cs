using VirtualPet.Business.Models;
using System.Collections.Generic;

namespace VirtualPet.Services.Interfaces
{
    /// <summary>
    /// Information on cakes that can be fed to pets.
    /// </summary>
    public interface ICakeService
    {
        /// <summary>
        /// Gets information all the types of cakes that can be fed to pets.
        /// </summary>
        /// <returns>
        /// A list of cakes.
        /// </returns>
        public List<Cake> GetCakes();
    }
}
