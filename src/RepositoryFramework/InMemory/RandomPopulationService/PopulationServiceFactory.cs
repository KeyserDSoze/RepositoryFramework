namespace RepositoryFramework.Population
{
    internal class PopulationServiceFactory<T, TKey> : IPopulationServiceFactory<T, TKey>
        where TKey : notnull
    {
        private readonly IArrayPopulationService<T, TKey> _arrayPopulationService;
        private readonly IAbstractPopulationService<T, TKey> _abstractPopulationService;
        private readonly IBoolPopulationService<T, TKey> _boolPopulationService;
        private readonly IBytePopulationService<T, TKey> _bytePopulationService;
        private readonly ICharPopulationService<T, TKey> _charPopulationService;
        private readonly IDictionaryPopulationService<T, TKey> _dictionaryPopulationService;
        private readonly IEnumerablePopulationService<T, TKey> _enumerablePopulationService;
        private readonly INumberPopulationService<T, TKey> _numberPopulationService;
        private readonly IGuidPopulationService<T, TKey> _guidPopulationService;
        private readonly ITimePopulationService<T, TKey> _timePopulationService;
        private readonly IStringPopulationService<T, TKey> _stringPopulationService;
        private readonly IRangePopulationService<T, TKey> _rangePopulationService;
        private readonly IClassPopulationService<T, TKey> _classPopulationService;

        public PopulationServiceFactory(IArrayPopulationService<T,TKey> arrayPopulationService,
            IAbstractPopulationService<T,TKey> abstractPopulationService,
            IBoolPopulationService<T, TKey> boolPopulationService,
            IBytePopulationService<T, TKey> bytePopulationService,
            ICharPopulationService<T, TKey> charPopulationService,
            IDictionaryPopulationService<T, TKey> dictionaryPopulationService,
            IEnumerablePopulationService<T, TKey> enumerablePopulationService,
            INumberPopulationService<T, TKey> numberPopulationService,
            IGuidPopulationService<T, TKey> guidPopulationService,
            ITimePopulationService<T, TKey> timePopulationService,
            IStringPopulationService<T, TKey> stringPopulationService,
            IRangePopulationService<T, TKey> rangePopulationService,
            IClassPopulationService<T, TKey> classPopulationService)
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
        public IRandomPopulationService<T, TKey> GetService(Type type, string treeName)
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