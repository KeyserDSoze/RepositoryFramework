using System.Security.Cryptography;

namespace RepositoryFramework.InMemory.Population
{
    internal class RangePopulationService : IRandomPopulationService
    {
        public int Priority => 1;
        public dynamic GetValue(RandomPopulationOptions options)
        {
            int firstNumber = BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4));
            int secondNumber = BitConverter.ToInt32(RandomNumberGenerator.GetBytes(4));
            if (firstNumber < 0)
                firstNumber *= -1;
            if (secondNumber < 0)
                secondNumber *= -1;
            if (firstNumber > secondNumber)
                (secondNumber, firstNumber) = (firstNumber, secondNumber);
            if (firstNumber == secondNumber)
                secondNumber++;
            return new Range(new Index(firstNumber), new Index(secondNumber));
        }

        public bool IsValid(Type type) 
            => type == typeof(Range) || type == typeof(Range?);
    }
}