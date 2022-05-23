/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.Data;

namespace UnitEditor.Window
{
	public class SettingsWindow : EditorWindow
	{
		#region Fields

		private static SettingsWindow window;
		private Editor editor;

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

			editor = Editor.CreateEditor(DataHandler.Instance.EditorData);

		}

		private void OnGUI()
		{
			editor.DrawDefaultInspector();
		}

		private void OnDestroy()
		{
			if (editor != null) DestroyImmediate(editor);
		}

		#endregion

		#region Setup

		private bool Setup()
		{
			return true;
		}

		#endregion

		#region Destroy
		#endregion

		public static void OpenWindow()
		{
			window = GetWindow<SettingsWindow>("Settings");
		}

	}
}

