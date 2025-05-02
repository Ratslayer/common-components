using System;

namespace BB
{
	[Serializable]
	public sealed class SerializedInt : SerializedValue<int>
	{
		protected override int Convert(double value)
			=> (int)Math.Floor(value);

		protected override int GetRandom(int min, int max)
			=> RandomUtils.Range(min, max);

		public static implicit operator SerializedInt(int value)
			=> new()
			{
				_minValue = value,
				_type = SerializedValueType.Const
			};
	}
}