using BB.Di;
using BB.Serialized;
using BB.Serialized.States;
using UnityEngine;
namespace BB
{
    [RequireComponent(typeof(TriggerVolumeBehaviour))]
    public sealed class PlayerTriggerVolumeActions : EntityComponent3D
    {
        public SerializedSceneStateActions _actions = new();
        public override void Install(IDiContainer container)
        {
            base.Install(container);
            container.System<TriggerSystem>();
        }
        sealed class TriggerSystem : EntitySystem
        {
            [Inject] PlayerTriggerVolumeActions _behaviour;
            [Inject] Player _player;
            [OnEvent]
            void OnEnter(TriggerVolumeEnterEvent e)
            {
                if (e._entity != _player)
                    return;
                var context = new SerializedActionContext
                {
                    Entity = Entity
                };
                _behaviour._actions.Enter(context);
            }
            [OnEvent]
            void OnExit(TriggerVolumeExitEvent e)
            {
                if (e._entity != _player)
                    return;
                var context = new SerializedActionContext
                {
                    Entity = Entity
                };
                _behaviour._actions.Exit(context);
            }
        }
    }
}