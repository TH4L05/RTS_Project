/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.Data;
using UnitEditor.UI.Section;
using UnitEditor.UI.Custom;


public class UnitDataEditor : Editor
{
    #region PrivateFields

    protected Color sectionColor = new Color(0.45f, 0.45f, 0.45f);
    protected GUISkin mySkin;
    protected Texture2D[] iconTextures;
    protected BaseSection baseSection;
    protected IconSection iconSection;
    protected StatsBaseSection statsSection;
    protected StatsAdditionalSection statsAdditionalSection;
    protected AbilitiesSection abilitiesSection;
    protected Rect baseRect = new Rect(20f, 15f, 1000f, 200f);

    #endregion

    #region UnityFunctions

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        Destroy();
    }

    #endregion

    #region GUI

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.DrawDefaultInspector();
        OnGUI();
        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    protected virtual void OnGUI()
    {
        Rect baseSectionRect1a = new Rect(baseRect.x, baseRect.y, baseRect.width, 400f);
        Rect baseSectionRect1b = new Rect(baseRect.x, baseRect.y + baseSectionRect1a.y + baseSectionRect1a.height, baseRect.width, 190f);
        Rect baseSectionRect2a = new Rect(baseRect.x, baseRect.y + baseSectionRect1b.y + baseSectionRect1b.height, baseRect.width, baseRect.height);
        Rect baseSectionRect2b = new Rect(baseRect.x, baseRect.y + baseSectionRect2a.y + baseSectionRect2a.height, baseRect.width, 300f);
        Rect baseSectionRect3 = new Rect(baseRect.x, baseRect.y + baseSectionRect2b.y + baseSectionRect2b.height, baseRect.width, 430f);

        baseSection.OnGUI(baseSectionRect1a);
        iconSection.OnGUI(baseSectionRect1b);
        statsSection.OnGUI(baseSectionRect2a);
        statsAdditionalSection.OnGUI(baseSectionRect2b);
        abilitiesSection.OnGUI(baseSectionRect3);
    }

    #region Setup

    protected virtual void Initialize()
    {
        iconTextures = DataHandler.Instance.IconTextures;
        mySkin = DataHandler.Instance.MySkin;

        baseSection = new BaseSection(serializedObject, mySkin, iconTextures);
        iconSection = new IconSection(serializedObject, mySkin, iconTextures);
        statsSection = new StatsBaseSection(serializedObject, mySkin, iconTextures);
        statsAdditionalSection = new StatsAdditionalSection(serializedObject, mySkin, iconTextures);
        abilitiesSection = new AbilitiesSection(serializedObject, mySkin, iconTextures);        
    }

    protected virtual void Destroy()
    {      
        baseSection = null;
        iconSection = null;
        statsSection = null;
        statsAdditionalSection = null;
        abilitiesSection = null;
    }

    #endregion
}
