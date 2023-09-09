#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using net.rs64.TexTransCore.TransTextureCore;
namespace net.rs64.TexTransTool
{
    [AddComponentMenu("TexTransTool/TextureBlender")]
    public class TextureBlender : TextureTransformer
    {
        public Renderer TargetRenderer;
        public int MaterialSelect = 0;
        public Texture2D BlendTexture;
        public Color Color = Color.white;
        public BlendType BlendType = BlendType.Normal;
        public PropertyName TargetPropertyName;

        public TextureBlenderDataContainer Container = new TextureBlenderDataContainer();

        [SerializeField] protected bool _IsApply = false;
        public override bool IsApply => _IsApply;

        public override bool IsPossibleApply => TargetRenderer != null && BlendTexture != null;


        public override void Apply(AvatarDomain avatarMaterialDomain = null)
        {
            if (!IsPossibleApply) return;
            if (_IsApply) return;
            _IsApply = true;

            var DistMaterials = TargetRenderer.sharedMaterials;

            if (DistMaterials.Length <= MaterialSelect) return;
            var DistMat = DistMaterials[MaterialSelect];

            var DistTex = DistMat.GetTexture(TargetPropertyName) as Texture2D;
            var AddTex = TextureLayerUtil.CreateMultipliedRenderTexture(BlendTexture, Color);
            if (DistTex == null) return;

            if (avatarMaterialDomain == null)
            {
                avatarMaterialDomain = new AvatarDomain(TargetRenderer.gameObject);

                var newBlendTex = TextureLayerUtil.BlendBlit(DistTex, BlendTexture, BlendType).CopyTexture2D();
                Container.BlendTextures = newBlendTex;

                avatarMaterialDomain.SetTexture(DistTex, newBlendTex);
            }
            else
            {
                avatarMaterialDomain.AddTextureStack(DistTex, new TextureLayerUtil.BlendTextures(AddTex, BlendType));
            }
        }
    }

    public class TextureBlenderDataContainer : TTDataContainer
    {
        [SerializeField] Texture2D _BlendTextures;

        public Texture2D BlendTextures
        {
            set => _BlendTextures = value;
            get => _BlendTextures;
        }

    }
}
#endif
