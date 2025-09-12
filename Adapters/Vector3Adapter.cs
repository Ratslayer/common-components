using UnityEngine;
namespace BB
{
	public readonly struct Vector3Adapter
	{
		public readonly Vector3 _vector;
		public Vector3Adapter(Vector3 vector) { _vector = vector; }
		public static implicit operator Vector3(Vector3Adapter a)
			=> a._vector;
		public static implicit operator Vector3Adapter(Vector3 v)
			=> new(v);
		public static implicit operator Vector3Adapter(Component c)
			=> new(c.transform.position);
		public static implicit operator Vector3Adapter(GameObject go)
			=> new(go.transform.position);
		public static implicit operator Vector3Adapter(Root root)
			=> new(root.Position);
	}
}