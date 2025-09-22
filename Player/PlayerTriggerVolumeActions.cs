using BB.Di;
using BB.Serialized.Actions;
using BB.Serialized.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static BB.PlayerTriggerVolumeActions;
namespace BB
{
	[RequireComponent(typeof(TriggerVolumeBehaviour))]
    public sealed class PlayerTriggerVolumeActions : EntityBehaviour<TriggerSystem>
    {
        [SerializeReference] ISerializedStateAction[] _onEnterExit = { };
        [SerializeReference] ISerializedAction[] _onEnter = { }, _onExit = { };
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
                Behaviour._onEnterExit.Invoke(context).Forget();
                Behaviour._onEnter.Invoke(context).Forget();
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
                Behaviour._onEnterExit.Exit(context).Forget();
                Behaviour._onExit.Invoke(context).Forget();
            }
        }
    }
}