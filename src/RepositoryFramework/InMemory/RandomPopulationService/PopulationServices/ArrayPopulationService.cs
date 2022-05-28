﻿namespace RepositoryFramework.Population
{
    internal class ArrayPopulationService : IArrayPopulationService
    {
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            var entity = Activator.CreateInstance(type, numberOfEntities);
            var valueType = type.GetElementType();
            for (int i = 0; i < numberOfEntities; i++)
                (entity as dynamic)![i] = populationService.Construct(valueType!, numberOfEntities, treeName, string.Empty, settings);
            return entity!;
        }
    }
}