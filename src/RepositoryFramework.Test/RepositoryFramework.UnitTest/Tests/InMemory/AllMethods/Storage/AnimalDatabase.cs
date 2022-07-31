using System.Collections.Generic;
using RepositoryFramework.UnitTest.AllMethods.Models;

namespace RepositoryFramework.UnitTest.AllMethods.Storage
{
    public class AnimalDatabase
    {
        private static readonly Dictionary<int, Animal> _animals = new();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test purpose")]
        public Dictionary<int, Animal> Animals => _animals;
    }
}
