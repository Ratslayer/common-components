using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class Spawn : SerializedActionSync, ISerializedSceneAction
    {
        public GameObject _prefab;
        public Transform _location;
        public bool _parentToLocation;
        protected override void InvokeSync(SerializedActionContext context)
        {
            if (_parentToLocation)
                _prefab.SpawnEntity(new()
                {
                    Position = _location.position,
                    Rotation = _location.rotation,
                    Parent = _location
                });
            else _prefab.SpawnEntity(new()
            {
                Position = _location.position,
                Rotation = _location.rotation
            });
        }
        protected override void SetupValidator(Validator validator, SerializedActionContext context)
        {
            validator
                .IsAssigned(_prefab, nameof(_prefab))
                .IsAssigned(_location, nameof(_location));
        }
    }
}
