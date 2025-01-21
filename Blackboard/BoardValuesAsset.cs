using Sirenix.OdinInspector;
using UnityEngine;
namespace BB
{
	public sealed class BoardValuesAsset : BaseScriptableObject, IBoardValuesProvider
	{
		[SerializeField, InlineProperty, HideLabel]
		SerializedBoardValues _values = new();

		public void Add(in AddBoardContext context)
			=> _values.Add(context);
	}
}