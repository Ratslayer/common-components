using UnityEditor.SpeedTree.Importer;
using UnityEngine;

namespace BB
{
    public interface IMaterialEffect
    {
        void Apply(in ApplyMaterialEffectContext context);
    }

    public readonly struct ApplyMaterialEffectContext
    {
        public MaterialPropertyBlock Block { get; init; }
        public Material Material { get; init; }
        public Renderer Renderer { get; init; }

        const string BaseColorPropertyName = "_BaseColor";

        public Color GetBaseColor()
        {
            if (Block.isEmpty)
                return Material.GetColor(BaseColorPropertyName);
            return Block.GetColor(BaseColorPropertyName);
        }

        public void SetBaseColor(Color color)
        {
            Block.SetColor(BaseColorPropertyName, color);
        }

        public void SetAlpha(float alpha)
        {
            var color = GetBaseColor();
            color.a = alpha;
            SetBaseColor(color);
        }
    }
}