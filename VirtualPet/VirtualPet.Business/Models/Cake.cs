using VirtualPet.Core.Models;

namespace VirtualPet.Business.Models
{
    /// <summary>
    /// A cake that can be fed to a pet.
    /// </summary>
    public class Cake
    {
        protected readonly string _type;
        protected readonly int _hunger;
        protected readonly int _health;
        protected readonly int _cost;

        /// <summary>
        /// Constructor for a cake that can be fed to a pet.
        /// </summary>
        /// <param name="type">The name of the cake e.g. "banana".</param>
        /// <param name="hunger">Amount of hunger the cake replenishes.</param>
        /// <param name="health">Amount of health the cake replenishes.</param>
        /// <param name="cost">How much the cake costs.</param>
        public Cake(string type, int hunger, int health, int cost)
        {
            this._type = type;
            this._hunger = hunger;
            this._health = health;
            this._cost = cost;
        }

        /// <summary>
        /// The type of cake.
        /// </summary>
        public string Type => _type;

        /// <summary>
        /// The type of cake with the first letter capitalised.
        /// </summary>
        public string CapitalType => Methods.Capitalise(_type);

        /// <summary>
        /// The hunger the cake replenishes.
        /// </summary>
        public int Hunger => _hunger;

        /// <summary>
        /// The health the cake replenishes.
        /// </summary>
        public int Health => _health;

        /// <summary>
        /// The cost of the cake.
        /// </summary>
        public int Cost => _cost;
    }
}
