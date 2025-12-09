using BB.Di;
namespace BB
{
    public sealed class DisableOnBoardChangeBehaviour : EntityBehaviour
    {
        public BaseBoardKey _key;
        LogicalOperation _operation;
        public double _value;
		public override void Install(IDiContainer container)
		{
			base.Install(container);
            container.System<DisableSystem>();
		}
        sealed record DisableSystem()
        {
            [Inject]
            IBoard Board;
            [OnEvent]
            void OnEvent(IBoard board)
            {

            }
        }
    }
    public enum LogicalOperation
    {
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEquals,
        GreaterThan,
        GreaterThanOrEquals,
    }
    public static class LogicalOperationExtensions
    {
        public static bool Compare(this LogicalOperation op, double l, double r)
            => op switch
            {
                LogicalOperation.Equals => l.Approximately(r),
                LogicalOperation.NotEquals => !l.Approximately(r),
                LogicalOperation.LessThanOrEquals => l.LessThanOrEquals(r),
                LogicalOperation.GreaterThanOrEquals => l.GreaterThanOrEquals(r),
                LogicalOperation.LessThan => l.LessThan(r),
                LogicalOperation.GreaterThan => l.GreaterThan(r),
                _ => false
            };
    }
}