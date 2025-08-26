using System.Collections.Generic;
namespace BB
{
	public sealed class BoardContext : ProtectedPooledObject<BoardContext>
	{
		public IBoardKey Key { get; private set; }
		public IBoard StartingBoard { get; private set; }
		public IBoard Board { get; private set; }
		public IBoard TargetBoard { get; private set; }
		public List<IBoardKey> ActiveKeys { get; private set; } = new();
		public double Value { get; private set; } = 1;
		public string DebugName { get; private set; }
		public static BoardContext GetPooled(IBoard board, IBoardKey key = null)
		{
			var result = GetPooledInternal();
			result.Key = key;
			result.Board = board;
			result.StartingBoard = board;
			return result;
		}
		public override void Dispose()
		{
			if (!string.IsNullOrWhiteSpace(DebugName))
				Log.Info($"Disposing BoardContext {DebugName}");
			DebugName = null;
			ActiveKeys.Clear();
			Key = null;
			Board = null;
			TargetBoard = null;
			Value = 1;
			base.Dispose();
		}
		public BoardContext WithDebugName(string name)
		{
			DebugName = name;
			return this;
		}
		public BoardContext GetPooledCopy(IBoard board = null)
		{
			board ??= Board;
			var copy = GetPooled(board);
			copy.Key = Key;
			copy.StartingBoard = StartingBoard;
			copy.TargetBoard = TargetBoard;
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
		public BoardContext WithValue(double multiplier)
		{
			Value = multiplier;
			return this;
		}
		public BoardContext TimesValue(double multiplier)
		{
			Value = Value * multiplier;
			return this;
		}
		public BoardContext WithSwappedBoards()
		{
			(Board, TargetBoard) = (TargetBoard, Board);
			return this;
		}
		public void AddAndDispose()
		{
			Add();
			Dispose();
		}
		public void Add() => Board.Add(this);
		public double GetAndDispose()
		{
			var result = Board.Get(this);
			Dispose();
			return result;
		}
		public void AddAndDispose(IBoardValuesProvider provider)
		{
			provider?.AddBoardValues(this);
			Dispose();
		}
		public RemoveBoardValuesOnDispose AddAndDisposeWithInverse(IBoardValuesProvider provider)
		{
			provider.AddBoardValues(this);
			var result = RemoveBoardValuesOnDispose.GetInversePooledFromContext(this, provider);
			Dispose();
			return result;
		}
		public BoardContext GetInverseCopy()
			=> GetPooledCopy().WithValue(-Value);
	}
}