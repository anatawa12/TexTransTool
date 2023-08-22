#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Rs64.TexTransTool.Decal
{
    public abstract class AbstractDecal : TextureTransformer
    {
        public List<Renderer> TargetRenderers = new List<Renderer> { null };
        public BlendType BlendType = BlendType.Normal;
        public Color Color = Color.white;
        public string TargetPropatyName = "_MainTex";
        public bool MultiRendereMode = false;
        public float DefaultPading = 0.5f;
        public bool FastMode = true;
        public bool IsSeparateMaterial;

        public virtual Vector2? GetOutRengeTexture { get => Vector2.zero; }

        [SerializeField] protected bool _IsApply = false;
        public override bool IsApply => _IsApply;
        public override bool IsPossibleCompile => IsPossibleApply;


        public override void Apply(AvatarDomain avatarDomain)
        {
            if (!IsPossibleApply) return;
            if (_IsApply) return;
            Dictionary<Texture2D, Texture> DecalCompiledTextures = CompileDecal();

            if (avatarDomain != null)
            {
                if (!IsSeparateMaterial)
                {
                    foreach (var trp in DecalCompiledTextures)
                    {
                        avatarDomain.AddTextureStack(trp.Key, new TextureLayerUtil.BlendTextures(trp.Value, BlendType));
                    }
                }
                else
                {
                    var DecaleBlendTexteres = DecalBlend(DecalCompiledTextures, BlendType);
                    var Materials = Utils.GetMaterials(TargetRenderers).Distinct();
                    CopyTexdiscription(DecaleBlendTexteres);

                    var DictMat = TextureSwapdMaterial(DecaleBlendTexteres, Materials, TargetPropatyName);
                    Utils.ChangeMaterials(TargetRenderers, DictMat);
                }
            }
            else
            {
                var DecaleBlendTexteres = DecalBlend(DecalCompiledTextures, BlendType);
                var Materials = Utils.GetMaterials(TargetRenderers).Distinct();
                var DictMat = TextureSwapdMaterial(DecaleBlendTexteres, Materials, TargetPropatyName);

                Utils.ChangeMaterials(TargetRenderers, DictMat);
                var ListMatpe = MatPea.GeneratMatPeaList(DictMat);
                LocalSave = new DecalDataContainer();
                LocalSave.GenereatMaterials = ListMatpe;
                LocalSave.DecaleBlendTexteres = DecaleBlendTexteres.Values.ToList();
            }
            _IsApply = true;
        }

        private static void CopyTexdiscription(Dictionary<Texture2D, Texture2D> DecaleBlendTexteres)
        {
            foreach (var Dist in DecaleBlendTexteres.Keys.ToArray())
            {
                DecaleBlendTexteres[Dist] = DecaleBlendTexteres[Dist].CopySetting(DecaleBlendTexteres[Dist]);
            }
        }

        public static Dictionary<Material, Material> TextureSwapdMaterial(Dictionary<Texture2D, Texture2D> DecaleBlendTexteres, IEnumerable<Material> Materials, string TargetPropatyName)
        {
            var DictMat = new Dictionary<Material, Material>();
            foreach (var Material in Materials)
            {
                var OldTex = Material.GetTexture(TargetPropatyName) as Texture2D;

                if (OldTex == null) continue;
                if (!DecaleBlendTexteres.ContainsKey(OldTex)) continue;

                var NewMat = UnityEngine.Object.Instantiate(Material);

                var NewTex = DecaleBlendTexteres[OldTex];
                NewMat.SetTexture(TargetPropatyName, NewTex);
                DictMat.Add(Material, NewMat);
            }

            return DictMat;
        }

        public static Dictionary<Texture2D, Texture2D> DecalBlend(Dictionary<Texture2D, Texture> DecalCompiledTextures, BlendType BlendType)
        {
            var DecaleBlendTexteres = new Dictionary<Texture2D, Texture2D>();
            foreach (var Texture in DecalCompiledTextures)
            {
                var BlendTexture = TextureLayerUtil.BlendBlit(Texture.Key, Texture.Value, BlendType).CopyTexture2D();
                BlendTexture.Apply();
                DecaleBlendTexteres.Add(Texture.Key, BlendTexture);
            }

            return DecaleBlendTexteres;
        }

        public abstract Dictionary<Texture2D, Texture> CompileDecal();

        DecalDataContainer LocalSave;

        public override void Revart(AvatarDomain avatarMaterialDomain = null)
        {
            if (!_IsApply) return;
            _IsApply = false;

            if (avatarMaterialDomain != null)
            {
                //何もすることはない。
            }
            else
            {
                var RevarList = MatPea.GeneratMatDict(MatPea.SwitchingdList(LocalSave.GenereatMaterials));
                Utils.ChangeMaterials(TargetRenderers, RevarList);
                LocalSave = null;
            }
            IsSelfCallApply = false;

        }

    }
}



#endif