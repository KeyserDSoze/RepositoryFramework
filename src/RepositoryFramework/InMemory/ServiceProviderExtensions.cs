using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;
using RepositoryFramework.Population;
using RepositoryFramework.Services;
using System.Collections;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        internal static List<PopulationServiceSettings> AllPopulationServiceSettings { get; } = new();
        public static IServiceProvider Populate(this IServiceProvider serviceProvider)
        {
            foreach (var populationServiceSetting in AllPopulationServiceSettings)
            {
                var populationService = serviceProvider.GetService(populationServiceSetting.PopulationServiceType) as IPopulationService;
                var instanceCreator = serviceProvider.GetService<IInstanceCreator>();
                var properties = populationServiceSetting.EntityType.GetProperties();
                for (int i = 0; i < populationServiceSetting.NumberOfElements; i++)
                {
                    var entity = instanceCreator!.CreateInstance(populationServiceSetting.EntityType, populationService!, populationServiceSetting.NumberOfElementsWhenEnumerableIsFound, string.Empty);
                    foreach (var property in properties.Where(x => x.CanWrite))
                        property.SetValue(entity, populationService!.Construct(property.PropertyType, populationServiceSetting.NumberOfElementsWhenEnumerableIsFound, string.Empty, property.Name));
                    var key = properties.First(x => x.Name == populationServiceSetting.KeyName).GetValue(entity);
                    populationServiceSetting.AddElementToMemory(key!, entity!);
                }
            }
            return serviceProvider;
        }
    }
}