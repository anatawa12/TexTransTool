#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace net.rs64.TexTransTool.Utils
{
    public static class RendererUtility
    {
        public static List<Material> GetMaterials(IEnumerable<Renderer> Renderers)
        {
            List<Material> matList = new List<Material>();
            foreach (var renderer in Renderers)
            {
                matList.AddRange(renderer.sharedMaterials);
            }
            return matList;
        }
        public static void SetMaterials(IEnumerable<Renderer> Renderers, List<Material> Mat)
        {
            int startOffset = 0;
            foreach (var renderer in Renderers)
            {
                int takeLength = renderer.sharedMaterials.Length;
                renderer.sharedMaterials = Mat.Skip(startOffset).Take(takeLength).ToArray();
                startOffset += takeLength;
            }
        }

        public static void ChangeMaterialForSerializedProperty(Dictionary<Material, Material> MatMapping, GameObject targetRoot, Type[] IgnoreTypes = null)
        {
            var allComponent = targetRoot.GetComponentsInChildren<Component>();
            IEnumerable<Component> components;
            if (IgnoreTypes.Any())
            {
                var filteredComponents = new List<Component>(allComponent.Length);
                foreach (var component in allComponent)
                {
                    var type = component.GetType();
                    if (!IgnoreTypes.Any(J => J.IsAssignableFrom(type))) { filteredComponents.Add(component); }
                }
                components = filteredComponents;
            }
            else
            {
                components = allComponent;
            }

            foreach (var component in components)
            {
                var type = component.GetType();

                var serializeObj = new SerializedObject(component);
                var iter = serializeObj.GetIterator();
                while (iter.Next(true))
                {
                    var s_Obj = iter;
                    if (s_Obj.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (s_Obj.objectReferenceValue is Material mat && MatMapping.ContainsKey(mat))
                        {
                            s_Obj.objectReferenceValue = MatMapping[mat];
                            serializeObj.ApplyModifiedProperties();
                        }
                    }
                }
            }
        }



        public static List<Mesh> GetMeshes(IEnumerable<Renderer> renderers, bool NullInsertion = false)
        {
            List<Mesh> meshes = new List<Mesh>();
            foreach (var renderer in renderers)
            {
                var mesh = renderer.GetMesh();
                if (mesh != null || NullInsertion) meshes.Add(mesh);
            }
            return meshes;
        }

        public static Mesh GetMesh(this Renderer Target)
        {
            Mesh mesh = null;
            switch (Target)
            {
                case SkinnedMeshRenderer SMR:
                    {
                        mesh = SMR.sharedMesh;
                        break;
                    }
                case MeshRenderer MR:
                    {
                        mesh = MR.GetComponent<MeshFilter>().sharedMesh;
                        break;
                    }
                default:
                    break;
            }
            return mesh;
        }

        // Remove once revert logic is removed
        public static void SetMesh(this Renderer Target, Mesh SetTarget)
        {
            switch (Target)
            {
                case SkinnedMeshRenderer SMR:
                    {
                        SMR.sharedMesh = SetTarget;
                        break;
                    }
                case MeshRenderer MR:
                    {
                        MR.GetComponent<MeshFilter>().sharedMesh = SetTarget;
                        break;
                    }
                default:
                    break;
            }
        }

        public static void SetMeshes(IEnumerable<Renderer> renderers, List<Mesh> DistMesh, List<Mesh> SetMesh)
        {
            foreach (var renderer in renderers)
            {
                switch (renderer)
                {
                    case SkinnedMeshRenderer SMR:
                        {
                            if (DistMesh.Contains(SMR.sharedMesh))
                            {
                                SMR.sharedMesh = SetMesh[DistMesh.IndexOf(SMR.sharedMesh)];
                            }
                            break;
                        }
                    case MeshRenderer MR:
                        {
                            var MF = MR.GetComponent<MeshFilter>();
                            if (DistMesh.Contains(MF.sharedMesh))
                            {
                                MF.sharedMesh = SetMesh[DistMesh.IndexOf(MF.sharedMesh)];
                            }
                            break;
                        }
                    default:
                        continue;
                }
            }
        }



    }
}
#endif
