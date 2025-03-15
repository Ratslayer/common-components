using UnityEngine;
namespace BB
{
	public readonly struct TransformProvider
	{
		public readonly Transform _transform;
		public TransformProvider(Transform transform)
			=> _transform = transform;
		public static implicit operator TransformProvider(GameObject gameObject)
			=> new(gameObject.transform);
		public static implicit operator TransformProvider(Component component)
			=> new(component.transform);
	}
}