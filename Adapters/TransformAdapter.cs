using UnityEngine;
namespace BB
{
    public readonly struct ScaleAdapter
    {
        public readonly Vector3 _scale;
        public ScaleAdapter(Vector3 scale)
            => _scale = scale;
        public static implicit operator ScaleAdapter(Vector3 scale)
            => new(scale);
        public static implicit operator ScaleAdapter(float scale)
            => new(scale * Vector3.one);
        public static implicit operator Vector3(ScaleAdapter scale)
            => scale._scale;
    }
    public readonly struct RotationAdapter
    {
        public readonly Quaternion _rotation;
        public RotationAdapter(Quaternion rotation)
            => _rotation = rotation;
        public static implicit operator RotationAdapter(Vector3 euler)
            => new(Quaternion.Euler(euler));
        public static implicit operator RotationAdapter(Quaternion rotation)
            => new(rotation);
    }
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