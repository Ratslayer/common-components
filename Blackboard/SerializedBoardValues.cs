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
		public BuffDisposable Add(IBoard board)
			=> Add(new AddBoardContext(board, null, 1));
		public BuffDisposable Add(in AddBoardContext context)
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

			return BuffDisposable.GetPooled(context._board, this);

			static void Add(
				IEnumerable<ISerializedBoardValue> values,
				in AddBoardContext context)
			{
				foreach (var value in values)
					value.Add(context);
			}
		}
		public IEnumerable<(BaseBoardKey, double)> GetAllValues()
		{
			foreach (var value in _values)
				if (value)
					yield return (value.Key, value.Value);
			foreach (var value in _resources)
				if (value)
					yield return (value.Key, value.Value);
			foreach (var value in _stats)
				if (value)
					yield return (value.Key, value.Value);
		}
	}
}