/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using UnitEditor.Data;
using UnitEditor.UI.Section;
using UnitEditor.UI.Custom;

namespace UnitEditor.Inspector
{
    [CustomEditor(typeof(CharacterData))]
    public class CharacterDataEditor : UnitDataEditor
    {
        private CharacterSpecificSection characterSpecificSection;

        protected override void Initialize()
        {
            base.Initialize();
            characterSpecificSection = new CharacterSpecificSection(serializedObject, mySkin, iconTextures);
        }

        protected override void Destroy()
        {
            base.Destroy();
            characterSpecificSection = null;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            Rect baseSectionRect4 = new Rect(baseRect.x, baseRect.y + 1600f, baseRect.width, 450f);
            characterSpecificSection.OnGUI(baseSectionRect4);
        }
    }
}
