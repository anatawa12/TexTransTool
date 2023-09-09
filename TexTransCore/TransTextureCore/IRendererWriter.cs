using System.Collections.Generic;
using UnityEngine;

namespace net.rs64.TexTransCore.TransTextureCore
{
    /// <summary>
    /// Instance of this interface may provides revertible material access
    /// </summary>
    public interface IRendererWriter
    {
        // TODO: make ChangeMaterialForRenderers extension method and provide lower level API for in interface
        void ChangeMaterialForRenderers(IEnumerable<Renderer> renderers, Material target, Material replacement);
        void ChangeMaterialForRenderers(IEnumerable<Renderer> renderers, Dictionary<Material, Material> mapping);
        void SetMesh(Renderer renderer, Mesh mesh);
    }

    public class DefaultRendererWriter : IRendererWriter
    {
        public static IRendererWriter Instance = new DefaultRendererWriter();

        private DefaultRendererWriter()
        {
        }

        public void ChangeMaterialForRenderers(IEnumerable<Renderer> renderers, Material target, Material replacement)
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
                    renderer.sharedMaterials = materials;
                }
            }
        }

        public void ChangeMaterialForRenderers(IEnumerable<Renderer> Renderer, Dictionary<Material, Material> MatPairs)
        {
            foreach (var renderer in Renderer)
            {
                var materials = renderer.sharedMaterials;
                var modified = false;
                for (var index = 0; index < materials.Length; index++)
                {
                    if (MatPairs.TryGetValue(materials[index], out var replacement))
                    {
                        materials[index] = replacement;
                        modified = true;
                    }
                }
                if (modified)
                {
                    renderer.sharedMaterials = materials;
                }
            }
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
    }
}