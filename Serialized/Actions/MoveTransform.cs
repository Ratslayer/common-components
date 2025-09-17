using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
	[Serializable]
    public sealed class MoveTransform : ISerializedActionSync
    {
        public Transform _target;
        public Transform _position;
        public void Invoke(in SerializedActionContext context)
        {
            if (!Validator.GetPooled()
                .WithEntity(context.Entity)
                .IsAssigned(_target, nameof(_target))
                .IsAssigned(_position, nameof(_position))
                .ValidateAndDispose())
                return;

            _target.SetPositionAndRotation(_position.position, _position.rotation);
        }
    }
}