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

		public static bool IsOpen;

		#endregion

		#region UnityFunctions

		private void OnEnable()
		{
			bool setupSuccess = Initialize();

			if (!setupSuccess)
			{
				window.Close();
				Debug.LogError("Setup Failed");
			}

			IsOpen = true;
		}

		private void OnGUI()
		{
			editor.OnInspectorGUI();
		}

		private void OnDestroy()
		{
			if (editor != null) DestroyImmediate(editor);
			IsOpen = false;
		}

		#endregion

		#region Initialize

		private bool Initialize()
		{
			var data = DataHandler.Instance.EditorData;
			if(data == null) return false;

			editor = Editor.CreateEditor(data);
			if(editor == null) return false;

			return true;
		}

        #endregion

        #region Destroy
        #endregion

        #region Window

        public static void OpenWindow()
		{
			window = GetWindow<SettingsWindow>("Settings");

			Rect mainWindowRect = UnitEditorWindow.GetWindowRect();

			window.maxSize = new Vector2(400f, 200f);
			window.minSize = new Vector2(400f, 200f);
			window.position = new Rect(mainWindowRect.x + 25f, mainWindowRect.y + (mainWindowRect.height) - 300f, 400f, 200f);
		}

		public static void CloseWindow()
		{
			if (IsOpen && window != null) window.Close();
		}

        #endregion
    }
}

