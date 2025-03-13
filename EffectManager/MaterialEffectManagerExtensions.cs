namespace BB
{
	public static class MaterialEffectManagerExtensions
	{
		const string ColorPropertyName = "_BaseColor";
		public static SetColorMaterialEffect GetDiffuseColor(
			this IMaterialEffectManager manager)
		{
			var effect = SetColorMaterialEffect.GetPooled();
			effect._colorName = ColorPropertyName;
			return effect.WithManager(manager);
		}
	}
}
