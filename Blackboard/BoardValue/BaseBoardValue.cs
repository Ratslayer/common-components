using BB.Di;

namespace BB
{
	public abstract record BaseBoardValue(IBoardKey Key, IBoard Board) : IBoardValueContainer, IValue<double>
	{
		public double Value { get; set; }
		protected Entity Entity => Board.Entity;
		public virtual void Add(double value, in AddBoardContext context)
			=> Value += value * context._numStacks;
		public virtual double GetValue(in GetBoardContext context)
			=> Value;
	}
}