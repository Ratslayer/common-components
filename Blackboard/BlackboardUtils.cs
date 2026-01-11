using BB.Di;
namespace BB
{

    public static class BlackboardUtils
    {
        public static void BindBlackboard(this IDiContainer container, in BlackboardInstallContext context)
        {
            container.Service<IBoard, Blackboard>();
            container.Event<IBoard>();

            if (context.InitialValues is not null)
                container.SystemWithArgs<InitBlackboard, InitBlackboard>(
                    (typeof(IBoardValuesProvider), context.InitialValues));
            if (context.FillResources)
                container.System<InitResources>();
        }
    }
    public readonly struct BlackboardInstallContext
    {
        public IBoardValuesProvider InitialValues { get; init; }
        public bool FillResources { get; init; }
    }
}