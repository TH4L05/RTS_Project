/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.UI.Section;

namespace UnitEditor.Inspector
{
    [CustomEditor(typeof(CharacterData))]
    public class CharacterDataEditor : UnitDataEditor
    {
        #region Fields

        private CharacterSpecificSection characterSpecificSection;

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            base.Initialize();
            characterSpecificSection = new CharacterSpecificSection(serializedObject, mySkin, iconTextures);
        }

        #endregion

        #region Destroy

        protected override void Destroy()
        {
            base.Destroy();
            characterSpecificSection = null;
        }

        #endregion

        #region GUI

        protected override void OnGUI()
        {
            base.OnGUI();
            Rect baseSectionRect4 = new Rect(baseRect.x, baseRect.y + 1600f, baseRect.width, 450f);
            characterSpecificSection.OnGUI(baseSectionRect4);
        }

        #endregion
    }
}
