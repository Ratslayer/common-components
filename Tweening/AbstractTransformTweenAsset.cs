using DG.Tweening;
using UnityEngine;

public abstract class AbstractTransformTweenAsset : BaseScriptableObject
{
	public abstract Tween CreateTween(Transform transform, Vector3 target);
}
