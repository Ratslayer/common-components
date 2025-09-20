using BB.Di;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
namespace BB.States
{
    public interface IStateProvider
    {
        IEnumerable<IState> GetStates();
    }
    public sealed class StateMachineBehaviour : EntityBehaviour, IStateMachine, IStateProvider
    {
        [ValueDropdown(nameof(GetStateNames))]
        public string _startingStateName;
        public SerializedState[] _states = { };
        IState _currentState;
        readonly List<IState> _runtimeStates = new();
        [OnSpawn]
        void OnSpawn()
        {
            _runtimeStates.SetRange(GetAllStates());
            EnterState(_startingStateName);
        }
        [OnDespawn]
        void OnDespawn() => EnterState(default(IState));
        public void EnterState(IState state)
        {
            _currentState?.Exit(new()
            {
                Machine = this,
                Entity = Entity
            });
            _currentState = state;
            _currentState?.Enter(new()
            {
                Machine = this,
                Entity = Entity
            });
        }
        public void EnterState(string name)
        {
            if (_runtimeStates.TryGetValue(s => s.Name == name, out var state))
                EnterState(state);
        }
        public List<string> GetStateNames()
            => GetAllStates()
            .Select(x => x.Name)
            .ToList();
        List<IState> GetAllStates()
            => GetComponentsInChildren<IStateProvider>()
            .SelectMany(x => x.GetStates())
            .Where(x => !string.IsNullOrWhiteSpace(x.Name))
            .ToList();

        public IEnumerable<IState> GetStates() => _states;
    }
}