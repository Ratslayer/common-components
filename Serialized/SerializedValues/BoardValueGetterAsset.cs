namespace BB
{
    public abstract class BoardValueGetterAsset : BaseScriptableObject
    {
        public abstract double GetValue(IBoard board, in GetBoardContext context);
    }
}