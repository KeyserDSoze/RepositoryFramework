using Fare;
using RepositoryFramework.Services;

namespace RepositoryFramework.Population
{
    internal class RegexPopulationService : IRegexPopulationService
    {
        private readonly IRegexService _regexService;
        public RegexPopulationService(IRegexService regexService) 
            => _regexService = regexService;
        public dynamic GetValue(Type type, IPopulationService populationService, int numberOfEntities, string treeName, InternalBehaviorSettings settings, dynamic args)
        {
            string[] regexes = args;
            var seed = regexes.First();
            if (!string.IsNullOrWhiteSpace(seed))
            {
                var generatedString = _regexService.GetRandomString(seed);
                if (type.Name.Contains("Nullable`1"))
                    type = type.GenericTypeArguments[0];
                if (type == typeof(Guid))
                    return Guid.Parse(generatedString);
                else if (type == typeof(DateTimeOffset))
                    return DateTimeOffset.Parse(generatedString);
                else if (type == typeof(TimeSpan))
                    return new TimeSpan(long.Parse(generatedString));
                else if (type == typeof(nint))
                    return nint.Parse(generatedString);
                else if (type == typeof(nuint))
                    return nuint.Parse(generatedString);
                else if (type == typeof(Range))
                {
                    var first = int.Parse(generatedString);
                    generatedString = _regexService.GetRandomString(regexes.Last());
                    var second = int.Parse(generatedString);
                    if (first > second)
                        (second, first) = (first, second);
                    if (first == second)
                        second++;
                    return new Range(new Index(first), new Index(second));
                }
                else
                    return Convert.ChangeType(generatedString, type);
            }
            return null!;
        }
    }
}