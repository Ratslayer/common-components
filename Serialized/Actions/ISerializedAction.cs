using Cysharp.Threading.Tasks;
using System.Threading;

namespace BB.Serialized
{
    public readonly struct SerializedActionContext
    {
        public Entity Entity { get; init; }
        public CancellationToken CancellationToken { get; init; }
    }
}
namespace BB.Serialized.Actions
{
    public interface ISerializedAllAction : ISerializedSceneAction, ISerializedAssetAction { }
    public interface ISerializedSceneAction : ISerializedAction { }
    public interface ISerializedAssetAction : ISerializedAction { }
    public interface ISerializedAction
    {
        bool WaitForExecution { get; }
        UniTask Invoke(SerializedActionContext context);
    }
    public interface ISerializedActionCondition
    {
        bool CanInvoke(in SerializedActionContext context);
    }
}