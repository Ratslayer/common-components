using Sirenix.OdinInspector;
namespace BB
{
	public sealed class BoardConfig : BaseScriptableObject
	{
		[Title("Hit"), Required]
		public Stat _hitChance;
		public Stat _missChance,
			_dodge,
			_parry,
			_targetSize;

		[Title("Damage"), Required]
		public Stat _damage;
		public Stat _armor,
			_damageTaken;

		[Title("Distance"), Required]
		public Stat _targetDistance;

		[Required]
		public Stat
			_softMinReach,
			_hardMinReach,
			_softMaxReach,
			_hardMaxReach;

		[Required]
		public BoardTag 
			_softTooFar,
			_hardTooFar,
			_softTooClose, _hardTooClose;

		[Required]
		public BoardResource _health, _stamina;
		[Required]
		public BoardKey _xp, _level, _perkCount;
	}
}