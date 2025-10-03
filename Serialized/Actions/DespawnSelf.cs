using System;

namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class DespawnSelf : SerializedActionSync, ISerializedAllAction
    {
        protected override void InvokeSync(SerializedActionContext context)
        {
            context.Entity.Despawn();
        }
    }
}