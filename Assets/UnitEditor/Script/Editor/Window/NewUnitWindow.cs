using UnityEngine;
using UnityEditor;

public class NewUnitWindow : EditorWindow
{
	#region Actions



	#endregion

	#region SerializedFields



	#endregion

	#region PrivateFields

	private static NewUnitWindow window;

    #endregion

    #region PublicFields



    #endregion

    #region UnityFunctions

    private void OnEnable()
    {
		bool setupSuccess = Setup();

		if (!setupSuccess)
        {
			window.Close();
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
		return false;
	}

	#endregion

	#region Destroy
	#endregion

	public static void OpenWindow()
	{
		window = GetWindow<NewUnitWindow>("New Unit");
	}
}
