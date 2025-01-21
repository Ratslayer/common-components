using UnityEngine;
public interface IMovementOverride
{
	bool OverrideHorizontal(out Vector3 velocity);
	bool OverrideVertical(out float velocity);
}