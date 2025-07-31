using System.Collections.Generic;
namespace BB
{
	public sealed class BoardContext : ProtectedPooledObject<BoardContext>
	{
		public IBoardKey Key { get; private set; }
		public IBoard Board { get; private set; }
		public IBoard TargetBoard { get; private set; }
		public List<object> Tags { get; private set; } = new();
		public List<object> TargetTags { get; private set; } = new();
		public List<IBoardKey> ActiveKeys { get; private set; } = new();
		public double Value { get; private set; } = 1;
		public static BoardContext GetPooled(IBoard board, IBoardKey key = null)
		{
			var result = GetPooledInternal();
			result.Key = key;
			result.Board = board;
			return result;
		}
		public override void Dispose()
		{
			Key = null;
			Board = null;
			TargetBoard = null;
			Tags.Clear();
			TargetTags.Clear();
			ActiveKeys.Clear();
			Value = 1;
			base.Dispose();
		}
		public BoardContext GetPooledCopy()
		{
			var copy = GetPooled(Board);
			copy.Key = Key;
			copy.TargetBoard = Board;
			copy.Tags.AddRange(TargetTags);
			copy.TargetTags.AddRange(TargetTags);
			copy.Value = Value;
			return copy;
		}
		public BoardContext WithKey(IBoardKey key)
		{
			Key = key;
			return this;
		}
		public BoardContext WithTarget(IBoard board)
		{
			TargetBoard = board;
			return this;
		}
		public BoardContext WithTag(object tag)
		{
			Tags.Add(tag);
			return this;
		}
		public BoardContext WithTags(IEnumerable<object> tags)
		{
			Tags.AddRange(tags);
			return this;
		}
		public BoardContext WithValue(double multiplier)
		{
			Value = multiplier;
			return this;
		}
		public BoardContext WithSwappedBoards()
		{
			(Board, TargetBoard) = (TargetBoard, Board);
			return this;
		}
		public void AddAndDispose()
		{
			Board.Add(this);
			Dispose();
		}
		public double GetAndDispose()
		{
			var result = Board.Get(this);
			Dispose();
			return result;
		}
		public void AddAndDispose(IBoardValuesProvider provider)
		{
			provider.Add(this);
			Dispose();
		}
		public RemoveBoardValuesOnDispose AddAndDisposeWithInverse(IBoardValuesProvider provider)
		{
			provider.Add(this);
			var result = RemoveBoardValuesOnDispose.GetInversePooledFromContext(this, provider);
			Dispose();
			return result;
		}
	}
}