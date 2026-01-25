using BB.Di;
using BB.Map;
using UnityEngine;
namespace BB
{
    public sealed class Player : EntityVariable<Player>
    {
        PlayerCamera _camera;
        Root _root;
        CurrentPlayerMapPoint _point;
        public Vector3 Position => _root.Position;
        protected override void OnUpdate()
        {
            base.OnUpdate();
            _camera = Require<PlayerCamera>();
            _root = Require<Root>();
            _point = Require<CurrentPlayerMapPoint>();
        }
        public void WarpToPoint(Vector3 point)
        {
            _root.Position = point;
            _camera.Value.transform.position = point;
        }
        public void SetMapPosition(PlayerMapPosition position)
        {
            _point.Value = position;
            WarpToPoint(position.Point.transform.position);
        }

        //[Inject] PlayerInstaller _playerInstaller;
        //[OnEvent]
        //void InitPlayer(EntityCreatedEvent _)
        //{
        //    Value = Entity.Spawn(new SpawnEntityFromInstaller3DContext
        //    {
        //        Installer = _playerInstaller,
        //    });
        //}
    }
    public abstract class SubscribeToEntityVariableEventSystem<TVariable, TEvent>
        : EntitySystem, IEventHandler<TEvent>
        where TVariable : EntityVariable<TVariable>
    {
        [Inject] TVariable _var;
        [Inject] IEvent<TVariable> _event;
        public TVariable Var => _var;
        readonly EventHandler _handler = new();
        [OnEvent(typeof(EntitySpawnedEvent))]
        void OnSpawn()
        {
            _handler._subscription = this;
            OnEvent(Var.Get<TEvent>());
            _event.Subscribe(_handler);
            _handler.Subscribe(Var);
        }
        [OnEvent(typeof(EntityDespawnedEvent))]
        void OnDespawn()
        {
            _handler.Unsubscribe(Var);
            _event.Unsubscribe(_handler);
        }

        public void OnEvent(TVariable msg)
        {
            if (_var.PreviousValue.Has(out IEvent<TEvent> e))
                e.Unsubscribe(this);
            if (_var.Value.Has(out e))
                e.Subscribe(this);
            //for those events that are self sourced, we trigger OnEvent immediately
            if (_var.Value.Has(out TEvent eventSource))
                OnEvent(eventSource);
        }

        public abstract void OnEvent(TEvent msg);

        sealed class EventHandler
            : IEventHandler<TVariable>
        {
            public SubscribeToEntityVariableEventSystem<TVariable, TEvent> _subscription;

            public void OnEvent(TVariable msg)
            {
                Unsubscribe(_subscription._var.PreviousValue);
                Subscribe(_subscription._var.Value);
                //для тех событий которые являеются своими же источниками
                if (_subscription._var.Value.Has(out TEvent eventSource))
                    _subscription.OnEvent(eventSource);
            }
            public void Subscribe(Entity entity)
            {
                if (entity.Has(out IEvent<TEvent> e))
                    e.Subscribe(_subscription);
            }
            public void Unsubscribe(Entity entity)
            {
                if (entity.Has(out IEvent<TEvent> e))
                    e.Unsubscribe(_subscription);
            }
        }
    }
}
