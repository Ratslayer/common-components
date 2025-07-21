//namespace BB
//{
//	public abstract record BaseBoardValue(IBoardKey Key, IBoard Board) 
//		: IBoardValueContainer, IValue<double>
//	{
//		double _value;
//		public double Value
//		{
//			get => _value;
//			set
//			{
//				if (!value.IsValid())
//				{
//					Log.Logger.Error($"Attempted to set {Key.Name} value to {value}");
//					return;
//				}

//				_value = value;
//			}
//		}
//		public double PreviousValue { get; private set; }
//		public void FlushPreviousValue()
//		{
//			PreviousValue = Value;
//		}
//		protected Entity Entity => Board.Entity;
//		public virtual void Add(BoardContext context)
//			=> Value += context._value;
//		public virtual double Get(BoardContext context)
//			=> Value;

//		public void Set(double value)
//			=> Value = value;
//	}
//}