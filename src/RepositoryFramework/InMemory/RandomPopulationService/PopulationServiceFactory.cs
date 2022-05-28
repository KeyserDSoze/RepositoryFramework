namespace RepositoryFramework.Population
{
    internal class PopulationServiceFactory : IPopulationServiceFactory
    {
        private readonly IArrayPopulationService _arrayPopulationService;
        private readonly IAbstractPopulationService _abstractPopulationService;
        private readonly IBoolPopulationService _boolPopulationService;
        private readonly IBytePopulationService _bytePopulationService;
        private readonly ICharPopulationService _charPopulationService;
        private readonly IDictionaryPopulationService _dictionaryPopulationService;
        private readonly IEnumerablePopulationService _enumerablePopulationService;
        private readonly INumberPopulationService _numberPopulationService;
        private readonly IGuidPopulationService _guidPopulationService;
        private readonly ITimePopulationService _timePopulationService;
        private readonly IStringPopulationService _stringPopulationService;
        private readonly IRangePopulationService _rangePopulationService;
        private readonly IClassPopulationService _classPopulationService;

        public PopulationServiceFactory(IArrayPopulationService arrayPopulationService,
            IAbstractPopulationService abstractPopulationService,
            IBoolPopulationService boolPopulationService,
            IBytePopulationService bytePopulationService,
            ICharPopulationService charPopulationService,
            IDictionaryPopulationService dictionaryPopulationService,
            IEnumerablePopulationService enumerablePopulationService,
            INumberPopulationService numberPopulationService,
            IGuidPopulationService guidPopulationService,
            ITimePopulationService timePopulationService,
            IStringPopulationService stringPopulationService,
            IRangePopulationService rangePopulationService,
            IClassPopulationService classPopulationService)
        {
            _arrayPopulationService = arrayPopulationService;
            _abstractPopulationService = abstractPopulationService;
            _boolPopulationService = boolPopulationService;
            _bytePopulationService = bytePopulationService;
            _charPopulationService = charPopulationService;
            _dictionaryPopulationService = dictionaryPopulationService;
            _enumerablePopulationService = enumerablePopulationService;
            _numberPopulationService = numberPopulationService;
            _guidPopulationService = guidPopulationService;
            _timePopulationService = timePopulationService;
            _stringPopulationService = stringPopulationService;
            _rangePopulationService = rangePopulationService;
            _classPopulationService = classPopulationService;
        }
        public IRandomPopulationService GetService(Type type, string treeName)
        {
            if (type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?)
                || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?)
                || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?)
                || type == typeof(nint) || type == typeof(nint?) || type == typeof(nuint) || type == typeof(nuint?)
                || type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?)
                || type == typeof(decimal) || type == typeof(decimal?))
                return _numberPopulationService;
            else if (type == typeof(byte) || type == typeof(byte?) || type == typeof(sbyte) || type == typeof(sbyte?))
                return _bytePopulationService;
            else if (type == typeof(bool) || type == typeof(bool?))
                return _boolPopulationService;
            else if (type == typeof(char) || type == typeof(char?))
                return _charPopulationService;
            else if (type == typeof(Guid) || type == typeof(Guid?))
                return _guidPopulationService;
            else if (type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(TimeSpan) || type == typeof(TimeSpan?) || type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
                return _timePopulationService;
            else if (type == typeof(Range) || type == typeof(Range?))
                return _rangePopulationService;
            else if (type == typeof(string))
                return _stringPopulationService;
            else if (!type.IsArray)
            {
                var interfaces = type.GetInterfaces();
                if (type.Name.Contains("IDictionary`2") || interfaces.Any(x => x.Name.Contains("IDictionary`2")))
                    return _dictionaryPopulationService;
                else if (type.Name.Contains("IEnumerable`1") || interfaces.Any(x => x.Name.Contains("IEnumerable`1")))
                    return _enumerablePopulationService;
                else
                    return _classPopulationService;
            }
            else if (type.IsArray)
                return _arrayPopulationService;
            else
                return _abstractPopulationService;
        }
    }
}