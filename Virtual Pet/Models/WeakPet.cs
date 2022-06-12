using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virtual_Pet.Models
{
    public class WeakPet : Pet
    {
        // A pet with reduced health but an increased boredom limit
        public WeakPet(string name, int petNo, string imageType) : base(name, petNo, imageType)
        {
            this.name = name;
            this.petNo = petNo;
            this.imageType = imageType;
            boredomLimit = 100;
            health = 50;
            maxHealth = standardMaxHealth / 2;
            strength = "weak";
        }
    }
}
