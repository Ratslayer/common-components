using BB.Di;

namespace BB
{
	public abstract record BaseBoardValue(IBoardKey Key, IBoard Board) : IBoardValueContainer, IValue<double>
	{
		double _value;
		public double BaseValue
		{
			get => _value;
			set
			{
				if (!value.IsValid())
				{
					Log.Logger.Error($"Attempted to set {Key.Name} value to {value}");
					return;
				}

				_value = value;
			}
		}
		public double PreviousValue { get; private set; }
		public void FlushPreviousValue()
		{
			PreviousValue = BaseValue;
		}
		protected Entity Entity => Board.Entity;
		public virtual void Add(in AddBoardContext context)
			=> BaseValue += context._value;
		public virtual double Get(in GetBoardContext context)
			=> BaseValue;
	}
}