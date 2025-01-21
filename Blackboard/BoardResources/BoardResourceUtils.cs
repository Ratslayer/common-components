//namespace BB
//{
//	public static class BoardResourceUtils
//	{
//		public static void SetValueToMax(this BoardResource resource, IBlackboardNew board)
//		{
//			if (resource.Has(board, out var value))
//				value.Value = value.MaxValue;
//		}
//		public static void SetValue(this BoardResource resource, IBlackboardNew board, double value)
//		{
//			if (resource.Has(board, out var bv))
//				bv.Value = value;
//		}
//		public static void GetValues(
//			this BoardResource resource, IBlackboardNew board,
//			out double value, out double maxValue)
//		{
//			if (!resource.Has(board, out var bv))
//			{
//				value = maxValue = 0;
//				return;
//			}

//			value = bv.Value;
//			maxValue = bv.MaxValue;
//		}
//		public static bool TryAddValue(
//			this BoardResource resource, IBlackboardNew board,
//			double value, out double oldValue, out double newValue)
//		{
//			resource.GetValues(board, out oldValue, out var maxValue);
//			newValue = oldValue + value;
//			if (newValue < 0 || newValue > maxValue)
//				return false;
//			resource.SetValue(board, newValue);
//			return true;
//		}
//	}
//}