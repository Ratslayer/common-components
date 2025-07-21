//using System.Collections.Generic;

//namespace BB
//{
//	public sealed record StatBoardValue(
//		IBlackboardStatKey Stat,
//		IBoard Board) : BaseBoardValue(Stat, Board), IBoardStatValue
//	{
//		readonly List<ConditionalBoardValue> _values = new();
//		public override double Get(in GetBoardContext context)
//		{
//			var value = Value;
//			foreach (var bv in _values)
//				value += bv.GetValue(context);
//			return value;
//		}
//	}
//}