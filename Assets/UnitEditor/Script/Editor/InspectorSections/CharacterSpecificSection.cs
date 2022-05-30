/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class CharacterSpecificSection : UnitDataSection
    {
        #region Fields
        #endregion

        public CharacterSpecificSection(SerializedObject so, GUISkin skin, Texture2D[] textures)
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("movementType");
            properties[1] = serializedObject.FindProperty("movementSpeed");
            properties[2] = serializedObject.FindProperty("movementAccerlation");
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            GUILayout.BeginArea(baseRect);
            {
                MyGUI.DrawColorRect(new Rect(0f, 0f, baseRect.width, baseRect.height), sectionColor);
                Rect sectionRect = new Rect(15f, 10f, 650f, baseRect.height - 15f);

                GUILayout.BeginArea(sectionRect);

                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(properties[0]);
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(properties[1]);
                EditorGUILayout.Space(5f);
                EditorGUILayout.PropertyField(properties[2]);

                GUILayout.EndArea();
            }
            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion
    }
}
