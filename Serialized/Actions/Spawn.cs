using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
	[Serializable]
	public sealed class Spawn : ISerializedActionSync
	{
		public GameObject _prefab;
		public Transform _location;
		public void Invoke(in SerializedSceneActionContext context)
		{
			if (!Validator.GetPooled(context.Entity)
				.IsAssigned(_prefab, nameof(_prefab))
				.IsAssigned(_location, nameof(_location))
				.ValidateAndDispose())
				return;
			_prefab.SpawnEntity(_location);
		}
	}
}
