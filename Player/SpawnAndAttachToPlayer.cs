using BB.Di;
namespace BB
{
    //public sealed class AttachToPlayerOnEnable : EntitySystem
    //{
    //    [Inject] Player _player;
    //    [OnEvent(typeof(EntityEnabledEvent), typeof(Player))]
    //    void OnEnable() => Entity.AttachTo(_player);
    //}
    //public sealed class SpawnAndAttachToPlayer : EntitySystem
    //{
    //    [Inject] IEntityInstaller _installer;
    //    [Inject] Player _player;
    //    [OnEvent(typeof(EntitySpawnedEvent), typeof(Player))]
    //    void Attach() => _installer.SpawnAndAttachTo(_player);
    //}
    public abstract class BaseAttachEventToPlayer<TEvent> : EntitySystem
    {
        [Inject]
        protected Player _player;
        DisposableToken _disposable;
        [OnEvent(typeof(EntityEnabledEvent))]
        void OnEnable()
        {
            _disposable = _player.Value.Subscribe<TEvent>(OnEvent);
            if (_player.Value.Has(out TEvent e))
                OnEvent(e);
        }
        [OnEvent(typeof(EntityDisabledEvent))]
        void OnDisable()
        {
            _disposable.Dispose();
            _disposable = default;
        }
        protected abstract void OnEvent(TEvent e);
    }
}