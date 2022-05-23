/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using UnitEditor.Data;
using UnitEditor.UI.Section;
using UnitEditor.UI.Custom;

namespace UnitEditor.Inspector
{
    [CustomEditor(typeof(BuildingData))]
    public class BuildingDataEditor : UnitDataEditor
    {
        private BuildingSpecificSection buildingSpecificSection;

        protected override void Initialize()
        {
            base.Initialize();
            buildingSpecificSection = new BuildingSpecificSection(serializedObject, mySkin, iconTextures);
        }

        protected override void Destroy()
        {
            base.Destroy();
            buildingSpecificSection = null;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            Rect baseSectionRect4 = new Rect(baseRect.x, baseRect.y + 1600f, baseRect.width, 450f);
            buildingSpecificSection.OnGUI(baseSectionRect4);
        }
    }
}

