using System;
using UnityEngine;

namespace BB
{
	[Serializable]
	public sealed class SerializedBoardValueCost : ICostBuilderModifier
	{
		[SerializeField] SerializedBoardValue[] _values = { };

		public void Modify(ICostBuilder builder)
		{
			foreach (var value in _values)
				if (value)
					builder.SpendResource(value.Key, value.Value);
		}
	}
}