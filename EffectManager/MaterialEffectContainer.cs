using System.Collections.Generic;
using UnityEngine;

namespace BB
{
    public sealed class MaterialEffectContainer
        : PooledObject<MaterialEffectContainer>,
            IMaterialEffectContainer
    {
        public Renderer _renderer;
        public IMaterialEffectManager _manager;
        readonly MaterialPropertyBlock _block = new();
        readonly List<IMaterialEffect> _effects = new();

        public override void Dispose()
        {
            _renderer.SetPropertyBlock(null);
            _effects.DisposeElementsAndClear();
            base.Dispose();
        }

        public void AddEffect(IMaterialEffect effect)
        {
            _effects.Add(effect);
            _manager.SetDirty(this);
        }

        public void RemoveEffect(IMaterialEffect effect)
        {
            _effects.Remove(effect);
            _manager.SetDirty(this);
        }

        public void Update()
        {
            foreach (var i in _renderer.sharedMaterials.Length)
            {
                _renderer.GetPropertyBlock(_block, i);
                foreach (var effect in _effects)
                    effect.Apply(new ApplyMaterialEffectContext
                    {
                        Block = _block,
                        Renderer = _renderer,
                        Material = _renderer.sharedMaterials[i]
                    });
                _renderer.SetPropertyBlock(_block,i);
            }
        }
    }
}