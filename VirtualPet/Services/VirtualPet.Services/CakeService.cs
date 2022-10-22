using VirtualPet.Services.Interfaces;
using VirtualPet.Business.Models;
using System.Collections.Generic;

namespace VirtualPet.Services
{
    public class CakeService : ICakeService
    {
        private readonly List<Cake> cakes = new()
        {
            new Cake("cake", 2, 0, 0),
            new Cake("berry", 10, 5, 5),
            new Cake("banana", 15, 2, 10),
            new Cake("peach", 20, 0, 20),
            new Cake("pea", 5, 10, 10),
            new Cake("bean", 2, 15, 25),
            new Cake("pod", 0, 20, 40),
            new Cake("ambrosia", 10000, 10000, 200)
        };

        public List<Cake> GetCakes()
        {
            return cakes;
        }
    }
}
