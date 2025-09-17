using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
	[Serializable]
	public sealed class Spawn : ISerializedActionSync
	{
		public GameObject _prefab;
		public Transform _location;
		public bool _parentToLocation;
		public void Invoke(in SerializedActionContext context)
		{
			if (!Validator.GetPooled(context.Entity)
				.IsAssigned(_prefab, nameof(_prefab))
				.IsAssigned(_location, nameof(_location))
				.ValidateAndDispose())
				return;
			if (_parentToLocation)
				_prefab.SpawnEntity(_location);
			else _prefab.SpawnEntity(new(_location.position, _location.rotation));
		}
	}
}
