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
		BoardTag[] _tags = { };

		[SerializeField]
		BoardValuesAsset[] _inline = { };

		public void Add(in AddBoardContext context)
		{
			Add(_stats, context);
			Add(_resources, context);
			foreach (var tag in _tags)
				context
					.WithKey(tag)
					.WithValue(1)
					.Add();

			foreach (var asset in _inline)
				if (asset)
					asset.Add(context);

			void Add(IEnumerable<ISerializedBoardValue> values, in AddBoardContext context)
			{
				foreach (var value in values)
					value.Add(context);
			}
		}
	}
}