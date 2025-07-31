using UnityEngine;
using BB.Di;
namespace BB
{
	public sealed class BlackboardBehaviour : EntityBehaviour
	{
		[SerializeField]
		BoardValuesAsset _values;
		public override void Install(IDiContainer container)
		{
			base.Install(container);
			container.BindBlackboard(_values);
		}
	}
}