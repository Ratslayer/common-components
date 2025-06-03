using BB.Di;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BB
{
	public interface IState { }
	public interface IStateData : IDisposable
	{
		Entity Entity { get; }
		void OnEnter();
		void OnExit();
		void AddEntity(Entity entity);
	}
	public interface IStateOnEnter : IState
	{
		void EnterState(IStateData data);
	}
	public interface IStateOnExit : IState
	{
		void ExitState(IStateData data);
	}
	public interface IStateMachine
	{
		Entity Entity { get; }
		CountedPooledDisposable EnterState(IState state);
		CountedPooledDisposable AppendState(IState state);
		void ExitState(IState state);
	}
	internal record StackStateMachine : EntitySystem, IStateMachine
	{
		readonly List<CountedPooledDisposable<StateData>> _states = new();
		public CountedPooledDisposable EnterState(IState state)
			=> EnterState(state, false);
		public CountedPooledDisposable AppendState(IState state)
			=> EnterState(state, true);
		private CountedPooledDisposable<StateData> EnterState(IState state, bool append)
		{
			state = state.NullIfDestroyedUnityEngineObject();
			if (state is null)
				return new();

			if (!IsInActiveState(out var current))
				return AddState();

			if (append)
				return AppendState();

			if (state != current?._state)
				return AddState();

			return new();

			CountedPooledDisposable<StateData> AddState()
			{
				current?.OnExit();
				var newState = StateData.GetPooled(this, state);
				var result = newState.GetTypedToken();
				_states.Add(result);
				newState.OnEnter();
				return result;
			}
			CountedPooledDisposable<StateData> AppendState()
			{
				var newState = StateData.GetPooled(this, state);
				var result = newState.GetTypedToken();
				current._appendedStates.Add(result);
				newState._parentState = current;
				newState.OnEnter();
				return result;
			}
		}

		public void ExitState(IState state)
		{
			if (!IsInActiveState(out var current))
				return;

			state = state.NullIfDestroyedUnityEngineObject();
			if (state is null || state == current._state)
			{
				current.OnExit();
				_states.RemoveAt(_states.Count - 1);
				if (IsInActiveState(out var newCurrent))
					newCurrent.OnEnter();
				return;
			}

			foreach (var i in -_states.Count)
				if (_states[i].Value._state == state)
				{
					_states.RemoveAt(i);
					return;
				}
		}
		bool IsInActiveState(out StateData state)
		{
			_states.RemoveDeadElements();
			state = _states.LastOrDefault().Value;
			return state is not null;
		}
	}
	public sealed class StateData : CountedProtectedPooledObject<StateData>, IStateData
	{
		readonly List<Entity> _spawnedEntities = new();
		public readonly List<CountedPooledDisposable<StateData>> _appendedStates = new();
		public StateData _parentState;
		public IState _state;
		public IStateMachine _stateMachine;
		bool _entered;
		public Entity Entity => _stateMachine?.Entity ?? default;
		public void AddEntity(Entity entity)
			=> _spawnedEntities.Add(entity);

		public override void Dispose()
		{
			base.Dispose();
			OnExit();
			_appendedStates.DisposeAndClear();
			_parentState?._appendedStates.Remove(GetTypedToken());
			_parentState = null;
			_stateMachine = null;
			_state = null;
		}

		public void OnEnter()
		{
			if (_entered)
				return;
			_entered = true;
			if (_state is IStateOnEnter e)
				e.EnterState(this);

			_appendedStates.RemoveDeadElements();
			foreach (var data in _appendedStates)
				data.Value.OnEnter();
		}

		public void OnExit()
		{
			if (!_entered)
				return;
			_entered = false;
			_appendedStates.RemoveDeadElements();
			foreach (var data in _appendedStates.Inverse())
				data.Value.OnExit();
			if (_state is IStateOnExit e)
				e.ExitState(this);
			_spawnedEntities.DespawnAndClear();
		}
		public static StateData GetPooled(IStateMachine machine, IState state)
		{
			var data = GetCountedPooledInternal();
			data._stateMachine = machine;
			data._state = state;
			return data;
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
		public static CountedPooledDisposable EnterState(this Entity entity, IState state)
		{
			if (state is null
				|| state is UnityEngine.Object obj && !obj
				|| !entity.Has(out IStateMachine machine))
				return default;

			return machine.EnterState(state);
		}
	}
	public readonly struct EnteredStateDisposable : IDisposable
	{
		readonly IStateMachine _machine;
		readonly IState _state;
		public EnteredStateDisposable(IStateMachine machine, IState state)
		{
			_machine = machine;
			_state = state;
		}

		public void Dispose()
		{
			_machine?.ExitState(_state);
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