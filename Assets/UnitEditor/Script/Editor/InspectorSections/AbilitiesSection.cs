/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class AbilitiesSection : UnitDataSection
    {
        #region Fields
        #endregion

        public AbilitiesSection(SerializedObject so, GUISkin skin, Texture2D[] textures)
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("abilities");
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {          
            GUILayout.BeginArea(baseRect);
            MyGUI.DrawColorRect(new Rect(0f, 0f, baseRect.width, baseRect.height), sectionColor);

            Rect topLabelRect = new Rect(15f, 5f, 200f, 20f);
            EditorGUI.LabelField(topLabelRect, "Abilities", mySkin.GetStyle("baseLabelField"));

            Rect sectionRect = new Rect(0f, topLabelRect.y + topLabelRect.height, baseRect.width, baseRect.height - topLabelRect.height);
            GUILayout.BeginArea(sectionRect);

            //TODO: add to Skin
            GUIStyle abilitiesLabelStyle = new GUIStyle();
            abilitiesLabelStyle.alignment = TextAnchor.MiddleCenter;
            abilitiesLabelStyle.normal.textColor = new Color(0.15f, 0.15f, 0.15f);
            abilitiesLabelStyle.fontSize = 10;

            Rect abilityRect = new Rect(15f, 10f, 128f, 128f);
            int index = 0;
            var enumerator = properties[0].GetEnumerator();
            while (enumerator.MoveNext())
            {
                MyGUI.DrawColorRect(abilityRect, new Color(0.33f, 0.33f, 0.33f, 0.5f));
                var prop = enumerator.Current as SerializedProperty;

                EditorGUI.PropertyField(new Rect(abilityRect.x, abilityRect.y, abilityRect.width, 25f), prop, GUIContent.none);

                if (prop.objectReferenceValue != null)
                {
                    SerializedObject so = new SerializedObject(prop.objectReferenceValue);
                    SerializedProperty name = so.FindProperty("name");
                    SerializedProperty icon = so.FindProperty("icon");

                    Texture2D iconTexture = new Texture2D(64, 64);
                    if (icon.objectReferenceValue != null)
                    {
                        Sprite iconSprite = icon.objectReferenceValue as Sprite;
                        iconTexture = iconSprite.texture;
                    }

                    EditorGUI.LabelField(new Rect(abilityRect.x + 3f, abilityRect.y + 26f, abilityRect.width - 3f, EditorGUIUtility.singleLineHeight), name.stringValue, abilitiesLabelStyle);
                    EditorGUI.DrawPreviewTexture(new Rect(abilityRect.x + 32f, abilityRect.y + 50f, 64f, 64f), iconTexture);
                }

                if (index == 3 || index == 7)
                {
                    abilityRect.y += abilityRect.height + 2f;
                    abilityRect.x = 15f;
                }
                else
                {
                    abilityRect.x += abilityRect.width + 2f;
                }

                index++;
            }
            GUILayout.EndArea();

            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion
    }
}
