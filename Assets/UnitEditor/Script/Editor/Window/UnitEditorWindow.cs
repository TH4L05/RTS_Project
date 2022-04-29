
using UnityEngine;
using UnityEditor;

public class UnitEditorWindow : EditorWindow
{
    #region Actions



    #endregion

    #region SerializedFields

    private static UnitEditorWindow window;
    private static string dataPath;

    #endregion

    #region PrivateFields
    #endregion

    #region PublicFields



    #endregion

    #region UnityFunctions

    private void OnEnable()
    {       
        bool setupSuccess = Setup();

        if (!setupSuccess)
        {
            Debug.LogError("Setup Failed");
        }

    }

    private void OnGUI()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    #endregion

    #region Setup

    private bool Setup()
    {
        bool result = CheckEditorPath();

        if (!result)
        {
            return false;
        }
        return true;
    }

    private bool CheckEditorPath()
    {
        string[] path = AssetDatabase.FindAssets("UnitEditor");
        dataPath = AssetDatabase.GUIDToAssetPath(path[0]);

        if (string.IsNullOrEmpty(dataPath))
        {
            Debug.LogError("Could not find DataPath");
            return false;
        }
        return true;
    }

    #endregion

    #region Destroy
    #endregion

    [MenuItem("Tools/UnitEditor/Window")]
    public static void OpenWindow()
    {
        window = GetWindow<UnitEditorWindow>("UnitEditor");
    }
}
