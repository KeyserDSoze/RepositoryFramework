using RepositoryFramework.Population;

namespace RepositoryFramework.Services
{
    public class InstanceCreator : IInstanceCreator
    {
        public object? CreateInstance(RandomPopulationOptions options, object?[]? args = null)
        {
            var constructor = options.Type.GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .FirstOrDefault();
            if (constructor == null)
                return null;
            else if (constructor.GetParameters().Length == 0)
                return Activator.CreateInstance(options.Type, args);
            else
            {
                List<object> instances = new();
                foreach (var x in constructor.GetParameters())
                    instances.Add(options.PopulationService.Construct(x.ParameterType,
                        options.NumberOfEntities, options.TreeName, $"{x.Name![0..1].ToUpper()}{x.Name[1..]}"));
                return Activator.CreateInstance(options.Type, instances.ToArray());
            }
        }
    }
}