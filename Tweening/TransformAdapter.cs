using BB.Di;
using UnityEngine;
namespace BB
{
	public readonly struct TransformAdapter
	{
		public readonly Transform _transform;
		public TransformAdapter(Transform transform)
			=> _transform = transform;
		public static implicit operator TransformAdapter(GameObject gameObject)
			=> new(gameObject.transform);
		public static implicit operator TransformAdapter(Component component)
			=> new(component.transform);
		public static implicit operator TransformAdapter(Root root)
			=> new(root.Transform);
		public static implicit operator TransformAdapter(Entity entity)
			=> new(entity.Root);
		public static implicit operator bool(TransformAdapter adapter)
			=> adapter._transform;
	}
}