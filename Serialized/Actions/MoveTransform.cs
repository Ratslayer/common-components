using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class MoveTransform : SerializedActionSync, ISerializedSceneAction
    {
        public Transform _target;
        public Transform _position;
        protected override void InvokeSync(SerializedActionContext context)
        {
            _target.SetPositionAndRotation(_position.position, _position.rotation);
        }
        protected override void SetupValidator(Validator validator, SerializedActionContext context)
        {
            validator
                .IsAssigned(_target, nameof(_target))
                .IsAssigned(_position, nameof(_position));

        }
    }
}