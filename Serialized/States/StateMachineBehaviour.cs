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
        void OnSpawn()
        {
            if (_startingState)
                _startingState.Enter(new()
                {
                    Machine = this,
                    Entity = Entity
                });
        }
        [OnDespawn]
        void OnDespawn()
        {
            if (!_currentState)
                return;
            _currentState.Exit(new()
            {
                Machine = this,
                Entity = Entity
            });
            _currentState = null;
        }
        public void EnterState(StateBehaviour state)
        {
            if (_currentState)
                _currentState.Exit(new()
                {
                    Machine = this,
                    Entity = Entity
                });
            _currentState = state;
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