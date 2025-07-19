namespace BB
{
	public interface IInputActionDetails
	{
		InputActionWrapperAsset Event { get; }
		string Name { get; }
		string InputName { get; }
		InputActionPosition Position { get; }
	}
}