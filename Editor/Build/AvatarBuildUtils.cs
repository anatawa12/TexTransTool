#if UNITY_EDITOR
using System;
using UnityEngine;

namespace net.rs64.TexTransTool.Build
{
    public static class AvatarBuildUtils
    {

        public static bool ProcessAvatar(GameObject avatarGameObject, UnityEngine.Object OverrideAssetContainer = null, bool UseTemp = false)
        {
            try
            {
                if (OverrideAssetContainer == null && UseTemp) { AssetSaveHelper.IsTemporary = true; }
                var aDDs = avatarGameObject.GetComponentsInChildren<AvatarDomainDefinition>();
                foreach (var aDD in aDDs)
                {
                    aDD.Apply(avatarGameObject, OverrideAssetContainer);
                }
                DestroyITexTransToolTags(avatarGameObject);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private static void DestroyITexTransToolTags(GameObject avatarGameObject)
        {
            foreach (var tf in avatarGameObject.GetComponentsInChildren<ITexTransToolTag>(true))
            {
                if (tf != null && tf is MonoBehaviour mb && mb != null && mb.gameObject != null)
                { MonoBehaviour.DestroyImmediate(mb.gameObject); }
            }
        }
    }
}
#endif
