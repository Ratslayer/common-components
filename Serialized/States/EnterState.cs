using BB.States;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
namespace BB.Serialized.Actions
{
    [Serializable]
    public sealed class EnterState : SerializedActionSync
    {
        public StateMachineBehaviour _machine;
        [ValueDropdown(nameof(States))]
        public string _state;
        protected override void InvokeSync(SerializedActionContext context)
        {
            _machine.EnterState(_state);
        }
        protected override void SetupValidator(Validator validator, SerializedActionContext context)
        {
            validator
                .IsAssigned(_machine, nameof(_machine))
                .NotEmpty(_state, nameof(_state));

        }
        List<string> States => _machine ? _machine.GetStateNames() : null;
    }
}