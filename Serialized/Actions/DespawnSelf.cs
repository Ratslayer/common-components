using System;

namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class DespawnSelf : ISerializedActionSync
    {
        public void Invoke(in SerializedActionContext context)
        {
            context.Entity.Despawn();
        }
    }
}