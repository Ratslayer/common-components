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
	[Serializable]
	public sealed class SerializedDouble : SerializedValue<double>
	{
		protected override double Convert(double value)
			=> value;

		protected override double GetRandom(double min, double max)
			=> RandomUtils.Range(min, max);

		public static implicit operator SerializedDouble(double value)
			=> new()
			{
				_minValue = value,
				_type = SerializedValueType.Const
			};
	}
}