/// <author> Thomas Krahl </author>

using System;
using UnityEngine;
using UnityEditor;
using UnitEditor.Data;

namespace UnitEditor
{
	public class NewUnitWindow : EditorWindow
	{
        #region Events

        public static Action NewUnitCreated;

        #endregion

        #region Fields

        private static NewUnitWindow window;
		private static DataHandler dataHandler;
		private UnitType unitType;
		private string unitName;

		public static bool IsActive;

		#endregion

		#region UnityFunctions

		private void OnEnable()
		{
			bool setupSuccess = Initialize();

			if (!setupSuccess)
			{
				window.Close();
				Debug.LogError("New Unit Window Setup = Failed");
			}

			IsActive = true;

		}

		private void OnGUI()
		{
			Rect rect = new Rect(15f, 15f, 300f, 175f);
			GUILayout.BeginArea(rect);

			EditorGUILayout.LabelField("UnitType");
			EditorGUILayout.Space(1f);
			unitType = (UnitType)EditorGUILayout.EnumFlagsField(unitType);
			EditorGUILayout.Space(6f);
			EditorGUILayout.LabelField("Name");
			EditorGUILayout.Space(1f);
			unitName = EditorGUILayout.TextField(unitName);
			EditorGUILayout.Space(15f);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Cancel", GUILayout.Width(100f), GUILayout.Height(40f)))
			{
				Close();
			}
			if (GUILayout.Button("Create", GUILayout.Width(100f), GUILayout.Height(40f)))
			{
				dataHandler.CreateNewUnit(unitType, unitName);
				NewUnitCreated?.Invoke();
				Close();
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.EndArea();

		}

		private void OnDestroy()
		{
			IsActive = false;
		}

		#endregion

		#region Initialize

		public static void OpenWindow(UnitEditorWindow rootWindow)
		{
			window = GetWindow<NewUnitWindow>("Craete New Unit");

			window.maxSize = new Vector2(400f, 200f);
			window.minSize = new Vector2(400f, 200f);
			window.position = new Rect(rootWindow.position.x + 50f, rootWindow.position.y + (rootWindow.position.height / 2), 400f, 200f);

			dataHandler = rootWindow.dataHandler;
		}

		private bool Initialize()
		{
			return true;
		}

		#endregion

		#region Destroy
		#endregion

	}
}
