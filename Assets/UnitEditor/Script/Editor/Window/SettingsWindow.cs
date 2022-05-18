/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

namespace UnitEditor.Window
{
	public class SettingsWindow : EditorWindow
	{
		#region Fields

		private static SettingsWindow window;

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

