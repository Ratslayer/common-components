using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace BB.Serialized.Actions
{
	[Serializable]
	public sealed class SerializedActions
	{
		[SerializeField]
		SerializedTriggers _triggers = new();
		[SerializeReference]
		ISerializedAction[] _actions = { };
		[SerializeReference]
		ISerializedActionCondition[] _conditions = { };
		[SerializeReference]
		ISerializedAction[] _failedActions = { };

		public IDisposable Subscribe(Entity entity)
		{
			var subscription = _triggers.CreateSubscription(new()
			{
				Entity = entity,
				Action = () => Invoke().Forget()
			});

			subscription.Subscribe();

			return subscription;

			async UniTaskVoid Invoke()
			{
				var success = true;
				foreach (var condition in _conditions)
					if (condition?.CanInvoke(new() { Entity = entity }) is false)
					{
						success = false;
						break;
					}
				var context = new SerializedSceneActionContext
				{
					Entity = entity
				};
				var actions = success ? _actions : _failedActions;
				foreach (var action in actions)
					if (action is ISerializedActionSync sync)
						sync.Invoke(context);
					else if (action is ISerializedActionAsync async)
					{
						var task = async.Invoke(context);
						if (async.WaitForExecution)
							await task;
						else task.Forget();
					}
			}
		}
	}
}
