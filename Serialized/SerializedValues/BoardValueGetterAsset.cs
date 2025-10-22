namespace BB
{
	public abstract class BoardValueGetterAsset : BaseScriptableObject
	{
		public abstract double GetValue(in GetBoardContext context);
	}
}