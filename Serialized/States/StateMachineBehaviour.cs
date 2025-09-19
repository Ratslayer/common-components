using BB.Di;
using Sirenix.OdinInspector;
namespace BB
{
    public sealed class StateMachineBehaviour : EntityBehaviour
    {
        public StateBehaviour _startingState;
        public StateBehaviour[] _states;
        StateBehaviour _currentState;
        [OnSpawn]
        void OnSpawn() => EnterState(_startingState);
        [OnDespawn]
        void OnDespawn() => EnterState(null);
        public void EnterState(StateBehaviour state)
        {
            if (_currentState)
                _currentState.Exit(new()
                {
                    Machine = this,
                    Entity = Entity
                });
            _currentState = state;
            if (_currentState)
                _currentState.Enter(new()
                {
                    Machine = this,
                    Entity = Entity
                });
        }

        [Button]
        void SetStatesToChildren()
            => _states = GetComponentsInChildren<StateBehaviour>();
    }
}