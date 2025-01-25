using BB.Di;

namespace BB
{
	public abstract record BaseBoardValue(IBoardKey Key, IBoard Board) : IBoardValueContainer, IValue<double>
	{
		public double Value { get; set; }
		protected Entity Entity => Board.Entity;
		public virtual void Add(in AddBoardContext context)
			=> Value += context._value;
		public virtual double Get(in GetBoardContext context)
			=> Value;
	}
}