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
    public sealed class StateMachineBehaviour : EntityComponent3D, IStateMachine, IStateProvider
    {
        [ValueDropdown(nameof(GetStateNames))]
        public string _startingStateName;
        public SerializedState[] _states = { };
        public IState CurrentState { get; private set; }
        readonly List<IState> _runtimeStates = new();
        [OnEvent(typeof(EntitySpawnedEvent))]
        void OnSpawn()
        {
            _runtimeStates.SetRange(GetAllStates());
            EnterState(_startingStateName);
        }
        [OnEvent(typeof(EntityDespawnedEvent))]
        void OnDespawn() => EnterState(default(IState));
        public void EnterState(IState state)
        {
            if (CurrentState is IStateExit exit)
                exit.Exit(new()
                {
                    Machine = this,
                    Entity = Entity
                });
            CurrentState = state;
            if (CurrentState is IStateEnter enter)
                enter.Enter(new()
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