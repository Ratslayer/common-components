namespace BB
{
    public interface IName
    {
        string Name { get; }
    }
    public interface INameDesc : IName
    {
        string Desc { get; }
    }

    public readonly struct NameDesc
    {
        public string Name { get; init; }
        public string Desc { get; init; }

        public static implicit operator NameDesc((string name, string desc) data)
            => new()
            {
                Name = data.name,
                Desc = data.desc
            };
        public static implicit operator NameDesc(string name)
            => new() { Name = name };
        public static implicit operator bool(NameDesc nameDesc)
            => nameDesc.Name.IsValid() || nameDesc.Desc.IsValid();
    }
}