using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class EnterState : SerializedActionSync
    {
        public StateMachineBehaviour _machine;
        [ValueDropdown(nameof(States))]
        public StateBehaviour _state;
        protected override void InvokeSync(SerializedActionContext context)
        {
            _machine.EnterState(_state);
        }
        protected override void SetupValidator(Validator validator, SerializedActionContext context)
        {
            validator
                .IsAssigned(_machine, nameof(_machine))
                .IsAssigned(_state, nameof(_state));

        }
        StateBehaviour[] States
            => _machine ? _machine._states : null;
    }
}