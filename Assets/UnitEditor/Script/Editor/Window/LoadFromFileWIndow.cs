/// <author> Thomas Krahl </author>

using System;

using UnityEngine;
using UnityEditor;

using UnitEditor.Data;

namespace UnitEditor.Window
{
	public class LoadFromFileWIndow : EditorWindow
	{
		#region Events

		public static Action NewDataLoaded;

        #endregion

        #region Fields

        private static LoadFromFileWIndow window;
		private GameObject obj;
		private string filePath;

		public static bool IsOpen;

		#endregion

		#region UnityFunctions

		private void OnEnable()
		{
			bool setupSuccess = Initialize();

			if (!setupSuccess)
			{
				Close();
				Debug.LogError("LoadFromFileWindow Initialize = Failed");
			}

			IsOpen = true;
		}

		private void OnGUI()
		{
			EditorGUILayout.Space(5f);
			EditorGUILayout.BeginHorizontal(GUILayout.Width(390f));

			EditorGUILayout.Space(10f);
			EditorGUILayout.BeginVertical(GUILayout.Height(190f));

			EditorGUILayout.LabelField("FilePath");
			filePath = EditorGUILayout.TextField(filePath, GUILayout.Width(300f));
					
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

			EditorGUILayout.EndHorizontal();
		}

		private void OnDestroy()
		{
			IsOpen = false;
		}

		#endregion

		#region Initialize

		private bool Initialize()
		{
			obj = DataHandler.Instance.ActiveObj;
			if (obj == null) return false;
			return true;
		}

        #endregion

        #region Destroy
        #endregion

        #region Window

        public static void OpenWindow()
		{
			window = GetWindow<LoadFromFileWIndow>("Load Data From File");
			window.maxSize = new Vector2(400f, 200f);
			window.minSize = new Vector2(400f, 200f);
			window.position = new Rect(Screen.width / 2 - 100f, Screen.height / 2 - 100f, 400f, 200f);
		}

		public static void CloseWindow()
		{
			if (IsOpen && window != null) window.Close();
		}
        
		#endregion

        private void LoadDataFromCSV()
        {
			string[] fileLines = DataHandler.Instance.LoadLinesFromCSV(filePath);
			if(fileLines == null) return;

			int idx = 0;
            foreach (var line in fileLines)
            {
				//Ignore file headings
				if (idx == 0)
                {
					idx++;
					continue;
                }

				string[] lineData = line.Split(';', StringSplitOptions.None);				

				if (string.IsNullOrEmpty(lineData[0]))
                {
					idx++;
					continue;
				}
				else if (lineData[0] == obj.name)
                {
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

