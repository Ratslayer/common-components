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
}