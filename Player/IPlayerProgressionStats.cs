namespace BB
{
    public interface IPlayerProgressionStats
    {
        void Set(ILoadableAsset key, double value);
        void ApplyToPlayer();
    }
}