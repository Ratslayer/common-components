using System;
using UnityEngine;

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
    [Serializable]
    public sealed class SetGameObjectActive : SerializedActionSync, ISerializedSceneAction
    {
        public GameObject _target;
        public bool _active;

        protected override void InvokeSync(SerializedActionContext context)
        {
            if(_target)
                _target.SetActive(_active);
        }
    }
}