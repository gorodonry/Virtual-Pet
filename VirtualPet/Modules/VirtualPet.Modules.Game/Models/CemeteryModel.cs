using System.Collections.Generic;
using System.Linq;
using VirtualPet.Business.Models;

namespace VirtualPet.Modules.Game.Models
{
    /// <summary>
    /// Contains all information and logic necessary to run the cemetery view.
    /// </summary>
    public class CemeteryModel
    {
        protected List<Pet> _deadPets;
        protected int _ticksSurvived;
        protected bool _allPetsDead;

        /// <summary>
        /// Creates a new instance of the cemetery model.
        /// </summary>
        public CemeteryModel()
        {

        }

        /// <summary>
        /// A list of all pets that have died thus far.
        /// </summary>
        public List<Pet> DeadPets => _deadPets;

        /// <summary>
        /// The number of ticks survived (thus far) by the user.
        /// </summary>
        public int TicksSurvived => _ticksSurvived;

        /// <summary>
        /// Boolean indicating whether or not the game is still on-going.
        /// </summary>
        /// <remarks>
        /// Set to true if at least one pet is still alive.
        /// </remarks>
        public bool GameOnGoing => !_allPetsDead;

        /// <summary>
        /// Gets and processes the information needed to run the cemetery view from the on-going/recent game model.
        /// </summary>
        /// <param name="gameplayModel"></param>
        public void ImportInformation(GameplayModel gameplayModel)
        {
            _deadPets = gameplayModel.DeadPets.ToList();
            _ticksSurvived = gameplayModel.TicksSurvived;
            _allPetsDead = gameplayModel.AllPetsDead;
        }
    }
}
