using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace BB.States
{
    public interface IStateProvider
    {
        IEnumerable<IState> GetStates();
    }

    public sealed class StateMachineComponent : EntityComponent3D, IStateMachine, IStateProvider, ISerializableComponent
    {
        [ValueDropdown(nameof(GetStateNames))] public string _startingStateName;
        public SerializedState[] _states = { };
        public IState CurrentState { get; private set; }
        readonly List<IState> _runtimeStates = new();

        [OnEvent]
        void OnSpawn(EntitySpawnedEvent _)
        {
            _runtimeStates.SetRange(GetAllStates());
            EnterState(_startingStateName);
        }

        [OnEvent]
        void OnDespawn(EntityDespawnedEvent _) => EnterState(default(IState));

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

        public IEntityComponentSerializer[] GetSerializers()
            => new[]
            {
                StateMachineComponentSerializerV1.Default
            };
    }

    public sealed class StateMachineComponentSerializerV1 : BaseSerializer<
        StateMachineComponentSerializerV1,
        StateMachineComponent,
        StateMachineComponentSerializerV1.Data>
    {
        public sealed class Data
        {
            public string _stateName;
        }

        protected override Data Serialize(StateMachineComponent target)
        {
            return new Data
            {
                _stateName = target.CurrentState.Name
            };
        }

        protected override void ApplySpawn(StateMachineComponent target, Data data)
        {
            var state = target._states.FirstOrDefault(x => x?.Name == data._stateName);
            if (state is null)
            {
                target.Entity
                    .GetLogger()
                    .WithClass(GetType())
                    .Error($"No state found with name {data._stateName}");
                return;
            }
            
            target.EnterState(state);
        }
    }
}