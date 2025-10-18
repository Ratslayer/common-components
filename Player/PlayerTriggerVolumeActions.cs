using BB.Di;
using BB.Serialized;
using BB.Serialized.States;
using UnityEngine;
using static BB.PlayerTriggerVolumeActions;
namespace BB
{
    [RequireComponent(typeof(TriggerVolumeBehaviour))]
    public sealed class PlayerTriggerVolumeActions : EntityBehaviour<TriggerSystem>
    {
        public SerializedSceneStateActions _actions = new();
        public sealed record TriggerSystem(
            PlayerTriggerVolumeActions Behaviour,
            Player Player) : EntitySystem
        {
            [OnEvent]
            void OnEnter(TriggerVolumeEnterEvent e)
            {
                if (e._entity != Player)
                    return;
                var context = new SerializedActionContext
                {
                    Entity = Entity
                };
                Behaviour._actions.Enter(context);
            }
            [OnEvent]
            void OnExit(TriggerVolumeExitEvent e)
            {
                if (e._entity != Player)
                    return;
                var context = new SerializedActionContext
                {
                    Entity = Entity
                };
                Behaviour._actions.Exit(context);
            }
        }
    }
}