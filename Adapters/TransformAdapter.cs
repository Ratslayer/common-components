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
        public static implicit operator TransformAdapter(BaseRoot root)
            => new(root.GameObject.transform);
        public static implicit operator TransformAdapter(Entity entity)
            => new(entity.Transform);
        public static implicit operator TransformAdapter(Variable<Entity> v)
           => new(v.Value.Transform);
        public static implicit operator TransformAdapter(Variable<Transform> v)
           => new(v.Value);
        public static implicit operator bool(TransformAdapter adapter)
            => adapter._transform;
    }
    public readonly struct TransformAdapter2D
    {
        public readonly RectTransform _rt;
        public TransformAdapter2D(Transform transform)
            => _rt = transform.GetComponent<RectTransform>();
        public static implicit operator TransformAdapter2D(GameObject gameObject)
            => new(gameObject.transform);
        public static implicit operator TransformAdapter2D(Component component)
            => new(component.transform);
        public static implicit operator TransformAdapter2D(Root2D root)
            => new(root.Transform);
        public static implicit operator TransformAdapter2D(Entity entity)
            => new(entity.Transform);
        public static implicit operator TransformAdapter2D(Variable<Entity> v)
           => new(v.Value.Transform);
        public static implicit operator TransformAdapter2D(Variable<Transform> v)
           => new(v.Value);
        public static implicit operator bool(TransformAdapter2D adapter)
            => adapter._rt;
    }
}