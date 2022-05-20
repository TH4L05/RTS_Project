/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class IconSection : UnitDataSection
    {
        #region Fields
        #endregion

        public IconSection(SerializedObject so, GUISkin skin, Texture2D[] textures)
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("selectionInfoIcon");
            properties[1] = serializedObject.FindProperty("actionButtonIcon");
            properties[2] = serializedObject.FindProperty("actionButtonIconHighlighted");
            properties[3] = serializedObject.FindProperty("actionButtonIconPressed");
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            GUILayout.BeginArea(baseRect);
            MyGUI.DrawColorRect(new Rect(0f, 0f, baseRect.width, baseRect.height), sectionColor);

            Rect sectionRect = new Rect(15f, 10f, 176f, 132f + (EditorGUIUtility.singleLineHeight * 2));
            Rect labelRect = new Rect(2f, 0f, sectionRect.width - 2f, EditorGUIUtility.singleLineHeight);
            Rect iconRect = new Rect(labelRect.x + 22f, labelRect.y + labelRect.height, 128f, 128f);
            Rect propertyRect = new Rect(labelRect.x, labelRect.y + labelRect.height + iconRect.height, sectionRect.width - 2f, EditorGUIUtility.singleLineHeight);
            float rectOffsetX = sectionRect.width + 10f;

            for (int i = 0; i < 4; i++)
            {
                MyGUI.DrawColorRect(sectionRect, new Color(0.33f, 0.33f, 0.33f, 0.5f));
                GUILayout.BeginArea(sectionRect);

                EditorGUI.LabelField(labelRect, properties[i].name, mySkin.customStyles[7]);
                Texture2D iconTexture = iconTextures[0];
                if (properties[i].objectReferenceValue != null)
                {
                    Sprite iconSprite = properties[i].objectReferenceValue as Sprite;
                    iconTexture = iconSprite.texture;
                }
                EditorGUI.DrawPreviewTexture(iconRect, iconTexture);
                EditorGUI.PropertyField(propertyRect, properties[i], GUIContent.none);
                GUILayout.EndArea();

                sectionRect.x += rectOffsetX;
            }

           GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion  
    }
}

