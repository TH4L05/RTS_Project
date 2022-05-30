/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.UI.Section;

namespace UnitEditor.Inspector
{
    [CustomEditor(typeof(BuildingData))]
    public class BuildingDataEditor : UnitDataEditor
    {
        #region Fields

        private BuildingSpecificSection buildingSpecificSection;

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            base.Initialize();
            buildingSpecificSection = new BuildingSpecificSection(serializedObject, mySkin, iconTextures);
        }

        #endregion

        #region Destroy

        protected override void Destroy()
        {
            base.Destroy();
            buildingSpecificSection = null;
        }

        #endregion

        #region GUI

        protected override void OnGUI()
        {
            base.OnGUI();
            Rect baseSectionRect4 = new Rect(baseRect.x, baseRect.y + 1600f, baseRect.width, 450f);
            buildingSpecificSection.OnGUI(baseSectionRect4);
        }

        #endregion
    }
}

