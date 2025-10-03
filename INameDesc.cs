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
}