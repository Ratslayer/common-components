using BB.Di;
using System.Collections.Generic;
using System.Linq;

namespace BB
{
	public interface IState { }
	public interface IStateOnEnter : IState
	{
		void EnterState(IStateMachine machine);
	}
	public interface IStateOnExit : IState
	{
		void ExitState(IStateMachine machine);
	}
	public interface IStateMachine
	{
		IState CurrentState { get; }
		Entity Entity { get; }
		void EnterState(IState state);
		void ExitState(IState state);
		void AddStateEntity(Entity entity);
	}
	internal record StackStateMachine : EntitySystem, IStateMachine
	{
		public IState CurrentState => _states.LastOrDefault();
		readonly List<IState> _states = new();
		readonly List<Entity> _stateEntities = new();
		public void EnterState(IState state)
		{
			ExitCurrentState();
			_states.Add(state);
			EnterCurrentState();
		}

		public void ExitState(IState state)
		{
			if (CurrentState == state)
			{
				ExitCurrentState();
				_states.RemoveAt(_states.Count - 1);
				EnterCurrentState();
				return;
			}

			foreach (var i in -_states.Count)
				if (_states[i] == state)
				{
					_states.RemoveAt(i);
					return;
				}
		}

		public void AddStateEntity(Entity entity)
		{
			_stateEntities.Add(entity);
		}
		void ExitCurrentState()
		{
			if (CurrentState is not IStateOnExit exit)
				return;
			exit.ExitState(this);
			_stateEntities.DespawnAndClear();
		}
		void EnterCurrentState()
		{
			if (CurrentState is IStateOnEnter enter)
				enter.EnterState(this);
		}
	}
	public static class StateMachineExtensions
	{
		public static void BindStateMachine(this IDiContainer container)
		{
			container
				.Construct<IStateMachine, StackStateMachine>()
				.Inject()
				.Lazy();
		}
		public static void EnterState(this Entity entity, IState state)
		{
			if (state is null || state is UnityEngine.Object obj && !obj)
				return;
			if (entity.Has(out IStateMachine machine))
				machine.EnterState(state);
		}
	}

	public static class CompositionUtils
	{
		public static void GetFlatListRecursionSafe<T>(
			IEnumerable<T> componentSource,
			IList<T> components)
		{
			var set = new HashSet<T>();
			GetAllComponents(componentSource, components, set);
		}
		public static void GetAllComponents<T>(
			IEnumerable<T> componentSource,
			IList<T> components,
			HashSet<T> addedComponents)
		{
			foreach (var c in componentSource)
				if (c is null)
					continue;
				else if (addedComponents.Add(c))
				{
					if (c is IEnumerable<T> subComponents)
						GetAllComponents(subComponents, components, addedComponents);
					else components.Add(c);
				}
				else Log.Logger.Error(
					$"{c} has been added more than once. " +
					$"This usually indicates cyclic referencing.");

		}
	}
}