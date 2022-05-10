/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class StatsBaseSection : UnitDataSection
    {
        #region Fields
        #endregion

        public StatsBaseSection(SerializedObject so, GUISkin skin, Texture2D[] textures)
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("healthMax");
            properties[1] = serializedObject.FindProperty("healthRegen");
            properties[2] = serializedObject.FindProperty("healthRegenRate");
            properties[3] = serializedObject.FindProperty("manaMax");
            properties[4] = serializedObject.FindProperty("manaRegen");
            properties[5] = serializedObject.FindProperty("manaRegenRate");
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            GUILayout.BeginArea(baseRect);
            MyGUI.DrawColorRect(new Rect(0f, 0f, baseRect.width, baseRect.height), sectionColor);

            Rect sectionRect = new Rect(15f, 10f, 420f, 175f);
            Rect imageRect = new Rect(15f, (sectionRect.height - 128f) / 2, 128f, 128f);
            Rect subSectionRect = new Rect(imageRect.x + imageRect.width, imageRect.y, 315f, 45f);
            Rect labelRect = new Rect(15f, 0f, 180f, 15f);
            Rect floatRect = new Rect(labelRect.x + labelRect.width, 0f, 50f, 15f);
            Rect sliderRect = new Rect(labelRect.x, labelRect.y + labelRect.height + 4f, 235f, 25f);
            float[] maxValues = { 9999f, 999f, 999f };
            int count = 0;

            for (int i = 0; i < 2; i++)
            {
                GUILayout.BeginArea(sectionRect);
                MyGUI.DrawColorRect(new Rect(0f, 0f, sectionRect.width, sectionRect.height), new Color(0.33f, 0.33f, 0.33f, 0.5f));
                EditorGUI.DrawPreviewTexture(imageRect, iconTextures[i + 1]);

                for (int s = 0; s < 3; s++)
                {
                    GUILayout.BeginArea(subSectionRect);
                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.LabelField(labelRect, properties[count].name, mySkin.customStyles[i]);
                    properties[count].floatValue = EditorGUI.IntField(floatRect, (int)properties[count].floatValue, mySkin.customStyles[6]);
                    EditorGUILayout.EndHorizontal();

                    properties[count].floatValue = GUI.HorizontalSlider(sliderRect, properties[count].floatValue, 0f, maxValues[s], mySkin.customStyles[i + 2], mySkin.customStyles[i + 4]);
                    EditorGUILayout.EndVertical();
                    GUILayout.EndArea();

                    subSectionRect.y += subSectionRect.height + 2f;
                    count++;
                }

                subSectionRect.y = imageRect.y;
                sectionRect.x += sectionRect.width + 15f;
                GUILayout.EndArea();
            }
            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion
    }
}
