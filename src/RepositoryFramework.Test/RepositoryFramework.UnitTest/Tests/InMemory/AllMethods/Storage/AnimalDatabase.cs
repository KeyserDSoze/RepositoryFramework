using System.Collections.Generic;
using RepositoryFramework.UnitTest.AllMethods.Models;

namespace RepositoryFramework.UnitTest.AllMethods.Storage
{
    public class AnimalDatabase
    {
        private static readonly Dictionary<int, Animal> s_animals = new();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test purpose")]
        public Dictionary<int, Animal> Animals => s_animals;
    }
}
