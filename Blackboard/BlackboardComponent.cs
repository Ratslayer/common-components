using UnityEngine;
using BB.Di;
namespace BB
{
    public sealed class BlackboardComponent : EntityComponent3D
    {
        [SerializeField]
        BoardValuesAsset _values;
        public override void Install(IDiContainer container)
        {
            base.Install(container);
            container.BindBlackboard(new()
            {
                InitialValues = _values,
                FillResources = true
            });
        }
    }
}