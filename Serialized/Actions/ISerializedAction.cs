using Cysharp.Threading.Tasks;

namespace BB.Serialized.Actions
{
	public readonly struct SerializedActionContext
	{
		public Entity Entity { get; init; }
	}
	public interface ISerializedAction
	{
	}
	public interface ISerializedActionSync : ISerializedAction
	{
		void Invoke(in SerializedActionContext context);
	}
	public interface ISerializedActionAsync : ISerializedAction
	{
		bool WaitForExecution { get; }
		UniTask Invoke(SerializedActionContext context);
	}
	public interface ISerializedActionCondition
	{
		bool CanInvoke(in SerializedActionContext context);
	}

	public static class SerializedActionExtensions
	{
	}
}
