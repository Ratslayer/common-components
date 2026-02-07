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
    }
}