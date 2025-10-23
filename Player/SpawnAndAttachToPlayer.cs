using BB.Di;
using System;

namespace BB
{
    public sealed record AttachToPlayerOnEnable(Player Player) : EntitySystem
    {
        [OnEnable, OnEvent(typeof(Player))]
        void OnEnable() => Entity.AttachTo(Player);
    }
    public sealed record SpawnAndAttachToPlayer(
        IEntityInstaller Installer,
        Player Player) : EntitySystem
    {
        [OnSpawn, OnEvent(typeof(Player))]
        void Attach() => Installer.SpawnAndAttachTo(Player);
    }
    public abstract record BaseAttachEventToPlayer<TEvent> : EntitySystem
    {
        [Inject]
        protected Player _player;
        DisposableToken _disposable;
        [OnEnable]
        void OnEnable()
        {
            _disposable = _player.Value.TempSubscribe<TEvent>(OnEvent);
            if (_player.Value.Has(out TEvent e))
                OnEvent(e);
        }
        [OnDisable]
        void OnDisable()
        {
            _disposable.Dispose();
            _disposable = default;
        }
        protected abstract void OnEvent(TEvent e);
    }
}