using System;
using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedBoardValues : IBoardValuesProvider
	{
		[SerializeField]
		SerializedStatValue[] _stats = { };

		[SerializeField]
		SerializedResourceBoardValueWithStats[] _resources = { };

		[SerializeField]
		SerializedBoardValue[] _values = { };

		[SerializeField]
		BoardTag[] _tags = { };

		[SerializeField]
		BoardValuesAsset[] _assets = { };

		public void Add(in AddBoardContext context)
		{
			using var _ = context._board.FlushOnDispose();

			Add(_stats, context);
			Add(_resources, context);
			Add(_values, context);

			foreach (var tag in _tags)
				context
					.WithKey(tag)
					.WithValue(1)
					.Add();

			foreach (var asset in _assets)
				if (asset)
					asset.Add(context);

			static void Add(
				IEnumerable<ISerializedBoardValue> values,
				in AddBoardContext context)
			{
				foreach (var value in values)
					value.Add(context);
			}
		}
	}
}