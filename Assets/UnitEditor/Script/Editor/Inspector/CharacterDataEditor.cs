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
    public class CharacterDataEditor : Editor
    {
        #region PrivateFields

        private Color sectionColor = new Color(0.45f, 0.45f, 0.45f);
        private GUISkin mySkin;
        private Texture2D[] iconTextures;
        private BaseSection baseSection;
        private UnitDataSectionIcon iconSection;
        private UnitDataSectionStatsBase statsSection;
        private UnitDataSectionStatsAdditional statsAdditionalSection;
        private UnitDataAbilitiesSection abilitiesSection;

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            var path = DataHandler.GetEditorDataPath();
            LoadTextures(path);
            LoadSkin(path);

            baseSection = new BaseSection(serializedObject, mySkin, iconTextures);
            iconSection = new UnitDataSectionIcon(serializedObject, mySkin, iconTextures);
            statsSection = new UnitDataSectionStatsBase(serializedObject, mySkin, iconTextures);
            statsAdditionalSection = new UnitDataSectionStatsAdditional(serializedObject, mySkin, iconTextures);
            abilitiesSection = new UnitDataAbilitiesSection(serializedObject, mySkin, iconTextures);

        }

        private void OnDestroy()
        {
            if (baseSection != null) DestroyImmediate(baseSection);
            if (iconSection != null) DestroyImmediate(iconSection);
            if (statsSection != null) DestroyImmediate(statsSection);
            if (statsAdditionalSection != null) DestroyImmediate(statsAdditionalSection);
            if (abilitiesSection != null) DestroyImmediate(abilitiesSection);
        }

        #endregion

        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //base.DrawDefaultInspector();

            float baseWidth = 1000f;
            float baseHeight = 200f;
            float baseX = 20f;
            float baseY = 15f;

            Rect baseSectionRect1a = new Rect(baseX, baseY, baseWidth, baseHeight);
            Rect baseSectionRect1b = new Rect(baseX, baseY + baseSectionRect1a.y + baseSectionRect1a.height, baseWidth, 190f);
            Rect baseSectionRect2a = new Rect(baseX, baseY + baseSectionRect1b.y + baseSectionRect1b.height, baseWidth, baseHeight);
            Rect baseSectionRect2b = new Rect(baseX, baseY + baseSectionRect2a.y + baseSectionRect2a.height, baseWidth, 300f);
            Rect baseSectionRect3 = new Rect(baseX, baseY + baseSectionRect2b.y + baseSectionRect2b.height, baseWidth, 410f);
            Rect baseSectionRect4 = new Rect(baseX, baseY + baseSectionRect3.y + baseSectionRect3.height, baseWidth, baseHeight);

            baseSection.OnGUI(baseSectionRect1a);
            iconSection.OnGUI(baseSectionRect1b);
            statsSection.OnGUI(baseSectionRect2a);
            statsAdditionalSection.OnGUI(baseSectionRect2b);
            abilitiesSection.OnGUI(baseSectionRect3);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Setup

        private void LoadTextures(string path)
        {
            iconTextures = new Texture2D[10];

            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/Data/Texture/IconNoIcon.png");
            if (tex == null) tex = new Texture2D(128, 128);
            iconTextures[0] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/Data/Texture/iconHealth.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[1] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/Data/Texture/iconMana.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[2] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/Data/Texture/IconArmor.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[3] = tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path + "/Data/Texture/IconWeapon1.png");
            if (tex == null) tex = iconTextures[0];
            iconTextures[4] = tex;
        }

        private void LoadSkin(string path)
        {
            mySkin = AssetDatabase.LoadAssetAtPath<GUISkin>(path + "/Data/UnitDataSkin.guiskin");
        }

        #endregion
    }
}
