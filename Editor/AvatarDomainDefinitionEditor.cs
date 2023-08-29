#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using net.rs64.TexTransTool.Build;

namespace net.rs64.TexTransTool.Editor
{

    [CustomEditor(typeof(AvatarDomainDefinition), true)]
    public class AvatarDomainDefinitionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Avatar"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenerateCustomMipMap"));

            var thisTarget = target as AvatarDomainDefinition;

            if (thisTarget.TexTransGroup == null)
            {
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Button("TexTransGroup is null");
                EditorGUI.EndDisabledGroup();

                serializedObject.ApplyModifiedProperties();
                return;
            }

            EditorGUI.BeginDisabledGroup(!thisTarget.TexTransGroup.IsPossibleApply || thisTarget.Avatar == null);
            if (thisTarget.TexTransGroup.IsApply == false)
            {
                if (GUILayout.Button("MaterialDomainUse - Apply"))
                {
                    thisTarget.Apply();
                    EditorUtility.SetDirty(thisTarget);
                    EditorUtility.SetDirty(thisTarget.TexTransGroup);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(!thisTarget.IsSelfCallApply);
                if (GUILayout.Button("Revert"))
                {
                    thisTarget.Revert();
                    EditorUtility.SetDirty(thisTarget);
                    EditorUtility.SetDirty(thisTarget.TexTransGroup);
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
