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
            new EntitySpawnContext3D
            {
                Installer = _prefab,
                Transform = new()
                {
                    Position = _location.position,
                    Rotation = _location.rotation,
                    Parent = _parentToLocation ? _location : null
                }
            }.Spawn();
        }
        protected override void SetupValidator(Validator validator, SerializedActionContext context)
        {
            validator
                .IsAssigned(_prefab, nameof(_prefab))
                .IsAssigned(_location, nameof(_location));
        }
    }
}
