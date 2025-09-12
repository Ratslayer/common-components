using System;

namespace BB
{
	[Serializable]
	public sealed class SerializedFloat : SerializedValue<float>
	{
		protected override float Convert(double value)
			=> (float)value;

		protected override float GetRandom(float min, float max)
			=> RandomUtils.Range(min, max);

		public static implicit operator SerializedFloat(float value)
			=> new()
			{
				_minValue = value,
				_type = SerializedValueType.Const
			};
	}
}