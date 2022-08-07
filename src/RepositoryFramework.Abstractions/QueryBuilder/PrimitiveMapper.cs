namespace RepositoryFramework
{
    public sealed class PrimitiveMapper
    {
        public Dictionary<string, string> FromNameToAssemblyQualifiedName { get; }
        public Dictionary<string, string> FromAssemblyQualifiedNameToName { get; }
        private PrimitiveMapper()
        {
            FromNameToAssemblyQualifiedName = new();
            FromNameToAssemblyQualifiedName.Add("int", typeof(int).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("int?", typeof(int?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("uint", typeof(uint).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("uint?", typeof(uint?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("short", typeof(short).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("short?", typeof(short?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("ushort", typeof(ushort).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("ushort?", typeof(ushort?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("long", typeof(long).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("long?", typeof(long?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("ulong", typeof(ulong).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("ulong?", typeof(ulong?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("nint", typeof(nint).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("nint?", typeof(nint?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("nuint", typeof(nuint).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("nuint?", typeof(nuint?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("float", typeof(float).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("float?", typeof(float?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("double", typeof(double).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("double?", typeof(double?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("decimal", typeof(decimal).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("decimal?", typeof(decimal?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("Range", typeof(Range).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("Range?", typeof(Range?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("DateTime", typeof(DateTime).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("DateTime?", typeof(DateTime?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("TimeSpan", typeof(TimeSpan).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("TimeSpan?", typeof(TimeSpan?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("DateTimeOffset", typeof(DateTimeOffset).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("DateTimeOffset?", typeof(DateTimeOffset?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("Guid", typeof(Guid).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("Guid?", typeof(Guid?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("char", typeof(char).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("char?", typeof(char?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("bool", typeof(bool).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("bool?", typeof(bool?).AssemblyQualifiedName!);
            FromNameToAssemblyQualifiedName.Add("string", typeof(string).AssemblyQualifiedName!);
            FromAssemblyQualifiedNameToName = FromNameToAssemblyQualifiedName.ToDictionary(x => x.Value, x => x.Key);
        }
        public static PrimitiveMapper Instance { get; } = new();
    }
}