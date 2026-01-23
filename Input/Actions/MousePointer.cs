using UnityEngine;
using UnityEngine.InputSystem;
namespace BB
{
    public interface IPointer
    {
        public Vector2 Position { get; }
        public Vector3 WorldPosition { get; }
        public PositionWithSpace PositionWithSpace => new()
        {
            Position = Position,
            Space = TransformSpace.Screen
        };

    }
    public sealed class MousePointer : IPointer
    {
        public Vector2 Position => Mouse.current.position.value;
        public Vector3 WorldPosition => CameraUtils.GetMouseRaycastPosition(10);
    }
}