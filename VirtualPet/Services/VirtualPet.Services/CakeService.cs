using VirtualPet.Services.Interfaces;
using VirtualPet.Business.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace VirtualPet.Services
{
    public class CakeService : ICakeService
    {
        private const string cakeFile = @"..\..\..\..\Services\VirtualPet.Services\Data\cakes.json";

        public List<Cake> GetCakes()
        {
            return JsonSerializer.Deserialize<List<Cake>>(File.ReadAllText(cakeFile))!;
        }
    }
}
