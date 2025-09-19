using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BB.Serialized.Actions
{
	[Serializable]
    public sealed class EnterState : ISerializedActionSync
    {
        public StateMachineBehaviour _machine;
        [ValueDropdown(nameof(States))]
        public StateBehaviour _state;
        public void Invoke(in SerializedActionContext context)
        {
            if (!Validator.GetPooled()
                .WithEntity(context.Entity)
                .IsAssigned(_machine, nameof(_machine))
                .IsAssigned(_state, nameof(_state))
                .ValidateAndDispose())
                return;
            _machine.EnterState(_state);
        }
        List<(string, StateBehaviour)> States
            => _machine ? _machine._states
            .Select(s => s ? (s.name, s) : ("", null))
            .ToList()
            : new();

    }
}