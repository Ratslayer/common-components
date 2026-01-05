using UnityEngine;
namespace BB
{
    public readonly struct RotationAdapter
    {
        public readonly Quaternion _rotation;
        public RotationAdapter(Quaternion rotation)
            => _rotation = rotation;
        public static implicit operator RotationAdapter(Vector3 euler)
            => new(Quaternion.Euler(euler));
        public static implicit operator RotationAdapter(Quaternion rotation)
            => new(rotation);
        public static implicit operator Quaternion(RotationAdapter rot)
            => rot._rotation;
        public static implicit operator RotationAdapter(Component c)
            => new(c.transform.rotation);
        public static implicit operator RotationAdapter(GameObject c)
            => new(c.transform.rotation);
    }
}