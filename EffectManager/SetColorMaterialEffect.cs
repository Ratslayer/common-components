using UnityEngine;

namespace BB
{
	public sealed class SetColorMaterialEffect
		: BasePooledMaterialEffect<SetColorMaterialEffect>
	{
		public string _colorName;
		private Color _color;
		public Color Color
		{
			get => _color;
			set
			{
				if (value.Approximately(_color))
					return;
				_color = value;
				Update();
			}
		}
		public override void Apply(in ApplyMaterialEffectContext context)
		{
			context.Block.SetColor(_colorName, _color);
		}
	}
}
