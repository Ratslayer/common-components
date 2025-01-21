using System;
using System.Collections.Generic;

namespace BB
{
	public sealed class CostBuilderContext : IDisposable
	{
		public readonly List<string> _errors = new();
		public IBoard _board;
		public Entity _entity, _target;
		public IAttack _attack;
		public AttackDetails _combatDetails;
		public void Init()
		{
			_board = _entity.Get<IBoard>();
			if (_entity && _target && _attack is not null)
			{
				var attackContext = new AttackContext(_attack, _entity, _target);
				attackContext.TryCalculateAttackDetails(out _combatDetails);
			}
		}
		public void Dispose()
		{
			_board = null;
			_target = default;
			_combatDetails = null;
			_attack = null;
			_errors.Clear();
		}
	}
}