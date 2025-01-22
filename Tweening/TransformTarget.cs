//using Sirenix.OdinInspector;
//using System;
//using UnityEngine;
//[Serializable]
//public sealed class Vector3Target
//{
//	public enum TargetType
//	{
//		Vector = 0,
//		Transform = 1
//	}
//	[SerializeField]
//	TargetType _type;
//	[SerializeField, ShowIf(nameof(ShowTransform))]
//	Transform _transform;
//	[SerializeField, ShowIf(nameof(ShowVector))]
//	Vector3 _vector;

//	bool ShowTransform => _type == TargetType.Transform;
//	bool ShowVector => _type == TargetType.Vector;
//}
//[Serializable]
//public sealed class TransformTarget
//{
//	public enum TargetType
//	{
//		TargetRoot = 0,
//		SceneTransform = 1,
//		AssetRoot = 2
//	}
//	[SerializeField]
//	TargetType _targetType;
//	[SerializeField, ShowIf(nameof(ShowTransform))]
//	Transform _transform;
//	[SerializeField, ShowIf(nameof(ShowAsset))]
//	EntityVarAsset _entityVarAsset;

//	bool ShowTransform => _targetType == TargetType.SceneTransform;
//	bool ShowAsset => _targetType == TargetType.AssetRoot;
//	public bool HasTransform(Entity entity, out Transform t)
//	{
//		t = _targetType switch
//		{
//			TargetType.TargetRoot => entity.Has(out Root root) ? root.Transform : default,
//			TargetType.AssetRoot => _entityVarAsset.Has(out Root root) ? root.Transform : default,
//			_ => _transform,
//		};
//		return t;
//	}
//}
