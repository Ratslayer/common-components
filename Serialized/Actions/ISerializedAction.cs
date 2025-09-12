using Cysharp.Threading.Tasks;

namespace BB.Serialized.Actions
{
	public readonly struct SerializedSceneActionContext
	{
		public Entity Entity { get; init; }
	}
	public interface ISerializedAction
	{
	}
	public interface ISerializedActionSync : ISerializedAction
	{
		void Invoke(in SerializedSceneActionContext context);
	}
	public interface ISerializedActionAsync : ISerializedAction
	{
		bool WaitForExecution { get; }
		UniTask Invoke(SerializedSceneActionContext context);
	}
	public interface ISerializedActionCondition
	{
		bool CanInvoke(in SerializedSceneActionContext context);
	}

	public static class SerializedActionExtensions
	{
	}
}
