using System.Collections.Generic;
using System.Linq;
using BB.Blackboard;
using BB.Di;
namespace BB
{

    public static class BlackboardUtils
    {
        public static void BindBlackboard(this IDiContainer container, in BlackboardInstallContext context)
        {
            container.Service<IBoard, BlackboardV1>();
            container.Event<IBoard>();

            if (context.InitialValues is not null)
                container.SystemWithArgs<InitBlackboard, InitBlackboard>(
                    (typeof(IBoardValue[]), context.InitialValues.ToArray()));
            if (context.FillResources)
                container.System<InitResources>();
        }
    }
    public readonly struct BlackboardInstallContext
    {
        public IEnumerable<IBoardValue> InitialValues { get; init; }
        public bool FillResources { get; init; }
    }
}