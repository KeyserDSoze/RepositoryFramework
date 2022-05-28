namespace RepositoryFramework.Population
{
    internal class RangePopulationService<T, TKey> : IRangePopulationService<T, TKey>
        where TKey : notnull
    {
        public dynamic GetValue(Type type, IPopulationService<T, TKey> populationService, int numberOfEntities, string treeName, dynamic args)
        {

            int firstNumber = populationService.Construct(typeof(int), numberOfEntities, treeName, "X");
            int secondNumber = populationService.Construct(typeof(int), numberOfEntities, treeName, "Y");
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
    }
}