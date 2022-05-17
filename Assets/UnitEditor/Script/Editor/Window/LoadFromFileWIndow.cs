/// <author> Thomas Krahl </author>

using System;
using UnityEngine;
using UnityEditor;
using UnitEditor.Data;
using UnitEditor.UI;

namespace UnitEditor
{
	public class LoadFromFileWIndow : EditorWindow
	{

		#region Events

		public static Action NewDataLoaded;

        #endregion

        #region Fields

        private static LoadFromFileWIndow window;
		private static DataHandler dataHandler;
		private static GameObject obj;

		private string filePath;
		private string fileName;

		public static bool IsOpen;

		#endregion

		#region UnityFunctions

		private void OnEnable()
		{
			bool setupSuccess = Initialize();

			if (!setupSuccess)
			{
				Close();
				Debug.LogError("LoadFromFileWIndow Setup = Failed");
			}

			IsOpen = true;
		}

		private void OnGUI()
		{

			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("FilePath");
			filePath = EditorGUILayout.TextField(filePath);
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			/*EditorGUILayout.Space(5);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("CSV FileName");
			fileName = EditorGUILayout.TextField(fileName);
			EditorGUILayout.EndHorizontal();*/

			EditorGUILayout.Space(10f);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Cancel", GUILayout.Width(100f), GUILayout.Height(35f)))
			{
				Close();
			}
			if (GUILayout.Button("Load", GUILayout.Width(100f), GUILayout.Height(35f)))
			{
				LoadDataFromCSV();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		private void OnDestroy()
		{
			IsOpen = false;
		}

		#endregion

		#region Initialize

		public static void OpenWindow()
		{
			window = GetWindow<LoadFromFileWIndow>("Load Data From File");
			dataHandler = UnitEditorWindow.DataHandler;

			window.maxSize = new Vector2(400f, 200f);
			window.minSize = new Vector2(400f, 200f);
			window.position = new Rect(Screen.width / 2 - 100f, Screen.height / 2 - 100f, 400f, 200f);
		}

		public static void CloseWindow()
		{
			if (IsOpen && window != null) window.Close();
		}

		private bool Initialize()
		{
			obj = null;
			obj = PropertiesArea.GetObj();

			return true;
		}

		#endregion

		#region Destroy
		#endregion

		private void LoadDataFromCSV()
        {
			string[] fileLines = dataHandler.LoadLinesFromCSV(filePath);

			int idx = 0;
            foreach (var line in fileLines)
            {
				//Ignore headings
				if (idx == 0)
                {
					idx++;
					continue;
                }

				string[] lineData = line.Split(';', StringSplitOptions.None);
				Debug.Log("name: " + lineData[0]);

				if (lineData[0] == obj.name)
                {
					Debug.Log(lineData[0] + " / " + lineData[1] + " ...");
					SetUnitData(lineData);
					return;
                }
				idx++;
			}

			Debug.LogError("Name not found !! - Could not set data");
		}

		private void SetUnitData(string[] data)
        {
			var unit = obj.GetComponent<Unit>();
			var type = unit.UnitData.Type;
			var unitData = unit.UnitData;

			unitData.SetDataFromStrings(data);

			UnitEditorWindow.NeedRepaint();
			Close();
        }
	}
}

