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



            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion
    }
}
