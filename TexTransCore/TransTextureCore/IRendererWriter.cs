using System.Collections.Generic;
using UnityEngine;

namespace net.rs64.TexTransCore.TransTextureCore
{
    /// <summary>
    /// Instance of this interface may provides revertible material access
    /// </summary>
    public interface IRendererWriter
    {
        void SetMesh(Renderer renderer, Mesh mesh);
        void SetMaterials(Renderer renderer, Material[] materials);
    }

    public static class RendererWriterExtensions
    {
        public static void ChangeMaterialForRenderers(this IRendererWriter writer, 
            IEnumerable<Renderer> renderers, Material target, Material replacement)
        {
            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials;
                var modified = false;

                for (var index = 0; index < materials.Length; index++)
                {
                    var distMat = materials[index];
                    if (target == distMat)
                    {
                        materials[index] = replacement;
                        modified = true;
                    }
                }

                if (modified)
                {
                    writer.SetMaterials(renderer, materials);
                }
            }
        }
        
        public static void ChangeMaterialForRenderers(this IRendererWriter writer, 
            IEnumerable<Renderer> renderers, Dictionary<Material, Material> mapping)
        {
            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials;
                var modified = false;
                for (var index = 0; index < materials.Length; index++)
                {
                    if (mapping.TryGetValue(materials[index], out var replacement))
                    {
                        materials[index] = replacement;
                        modified = true;
                    }
                }
                if (modified)
                {
                    writer.SetMaterials(renderer, materials);
                }
            }
        }
    }

    public class DefaultRendererWriter : IRendererWriter
    {
        public static IRendererWriter Instance = new DefaultRendererWriter();

        private DefaultRendererWriter()
        {
        }

        public void SetMesh(Renderer renderer, Mesh mesh)
        {
            switch (renderer)
            {
                case SkinnedMeshRenderer skinnedMeshRenderer:
                {
                    skinnedMeshRenderer.sharedMesh = mesh;
                    break;
                }
                case MeshRenderer meshRenderer:
                {
                    meshRenderer.GetComponent<MeshFilter>().sharedMesh = mesh;
                    break;
                }
                default:
                    // TODO: throw exception instead?
                    break;
            }
        }

        public void SetMaterials(Renderer renderer, Material[] materials) => renderer.sharedMaterials = materials;
    }
}