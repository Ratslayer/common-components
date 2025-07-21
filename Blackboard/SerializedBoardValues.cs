using System;
using System.Collections.Generic;
using UnityEngine;
namespace BB
{
	[Serializable]
	public sealed class SerializedBoardValues : IBoardValuesProvider
	{
		[SerializeField]
		SerializedBoardValueWithConditions[] _stats = { };

		[SerializeField]
		SerializedResourceBoardValueWithStats[] _resources = { };

		[SerializeField]
		SerializedBoardValue[] _values = { };

		[SerializeField]
		BoardTag[] _tags = { };

		[SerializeField]
		BoardValuesAsset[] _assets = { };
		public RemoveBoardValuesOnDispose Add(IBoard board)
		{
			var context = BoardContext.GetPooled(board);
			var result = Add(context);
			context.Dispose();
			return result;
		}
		public RemoveBoardValuesOnDispose Add(BoardContext context)
		{
			using var _ = context.Board.FlushOnDispose();

			Add(_stats, context);
			Add(_resources, context);
			Add(_values, context);

			foreach (var tag in _tags)
				context
					.GetPooledCopy()
					.WithKey(tag)
					.WithValue(1)
					.AddAndDispose();

			foreach (var asset in _assets)
				if (asset)
					asset.Add(context);

			return RemoveBoardValuesOnDispose.GetInversePooledFromContext(context, this);

			static void Add(
				IEnumerable<ISerializedBoardValue> values,
				BoardContext context)
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