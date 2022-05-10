/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class BaseSection : UnitDataSection
    {
        #region Fields
        #endregion

        public BaseSection(SerializedObject so, GUISkin skin, Texture2D[] textures) 
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("name");
            properties[1] = serializedObject.FindProperty("tooltip");
            properties[2] = serializedObject.FindProperty("selectionInfoIcon");
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            Rect sectionRect = baseRect;
            GUILayout.BeginArea(sectionRect);
            MyGUI.DrawColorRect(new Rect(0f, 0f, sectionRect.width, sectionRect.height), sectionColor);
            Texture2D iconTexture = iconTextures[0];
            if (properties[2].objectReferenceValue != null)
            {
                Sprite iconSprite = properties[2].objectReferenceValue as Sprite;
                iconTexture = iconSprite.texture;
            }

            sectionRect = new Rect(sectionRect.x + 5f, sectionRect.y + 5f, 150f, 150f);
            EditorGUI.DrawPreviewTexture(sectionRect, iconTexture);

            sectionRect = new Rect(sectionRect.x + 160f, sectionRect.y, 256f, 75f);
            GUILayout.BeginArea(sectionRect);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("UnitName", GUILayout.Height(20f));
            EditorGUI.PropertyField(new Rect(0f, 20f, sectionRect.width, EditorGUIUtility.singleLineHeight * 2), properties[0], GUIContent.none);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            sectionRect = new Rect(sectionRect.x, sectionRect.y + sectionRect.height + 2f, 256f, 75f);
            GUILayout.BeginArea(sectionRect);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Tooltip", GUILayout.Height(20f));
            EditorGUI.PropertyField(new Rect(0f, 20f, sectionRect.width, EditorGUIUtility.singleLineHeight * 2), properties[1], GUIContent.none);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion
    }
}

