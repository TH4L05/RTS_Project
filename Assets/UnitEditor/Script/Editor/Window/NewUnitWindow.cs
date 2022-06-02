/// <author> Thomas Krahl </author>

using System;

using UnityEngine;
using UnityEditor;

using UnitEditor.UI.Toolbar;
using UnitEditor.Data;

namespace UnitEditor.Window
{
	public class NewUnitWindow : EditorWindow
	{
        #region Events

        public static Action NewUnitCreated;

        #endregion

        #region Fields

        private static NewUnitWindow window;
		public static bool IsOpen;

		private UnitType unitType;
		private MessageType messageType;
		private string unitName;
		private string message;
		private int messageID;

		private GUIStyle label1;
		private GUIStyle label2;

		#endregion

		#region UnityFunctions

		private void OnEnable()
		{
			bool setupSuccess = Initialize();

			if (!setupSuccess)
			{
				Close();
				Debug.LogError("NewUnitWindow Initialize = Failed");
			}

			unitType = (UnitType)UnitEditorToolbar.ToolbarIndex;
			IsOpen = true;
		}

		private void OnGUI()
		{
			GUI();
		}
	
		private void OnDestroy()
		{
			IsOpen = false;
		}

        #endregion

        #region GUI

        private void GUI()
        {
			Rect areaRect = new Rect(15f, 15f, 300f, 175f);
			GUILayout.BeginArea(areaRect);
			{
				Rect rect = new Rect(25f, 2f, 150f, 25f);
				EditorGUI.LabelField(rect, $"Create new Unit of Type: ", label1);

				rect = new Rect(rect.x + rect.width + 2f, 0f, 125f, 25f);
				EditorGUI.LabelField(rect, $"{unitType}", label2);

				rect = new Rect(25f, rect.y + rect.height + 5f, 150f, 25f);
				EditorGUI.LabelField(rect, "Name");

				rect = new Rect(rect.x, rect.y + rect.height + 2f, 200f, 25f);
				unitName = EditorGUI.TextField(rect, unitName);
				if (string.IsNullOrEmpty(unitName))
				{
					messageID = 0;
				}
				else
				{
					if (messageID < 1) messageID = -1;
				}

				SetInfoMessage();

				rect = new Rect(25f, rect.y + rect.height + 8f, 250f, 40f);
				GUILayout.BeginArea(rect);
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Cancel", GUILayout.Width(100f), GUILayout.Height(35f)))
				{
					Close();
				}
				if (GUILayout.Button("Create", GUILayout.Width(100f), GUILayout.Height(35f)))
				{
					CreateNewUnit();
				}
				EditorGUILayout.EndHorizontal();
				GUILayout.EndArea();

				rect = new Rect(25f, rect.y + rect.height + 6f, 250f, 40f);
				GUILayout.BeginArea(rect);
				EditorGUILayout.HelpBox(message, messageType);
				GUILayout.EndArea();
			}
			GUILayout.EndArea();
		}

        #endregion


        #region Initialize

        private bool Initialize()
		{
			//TODO: add to skin file
			label1 = new GUIStyle();
			label1.normal.textColor = Color.white;

			//TODO: add to skin file
			label2 = new GUIStyle();
			label2.fontStyle = FontStyle.Bold;
			label2.normal.textColor = new Color(0.85f, 0.55f, 0f, 0.85f);
			label2.fontSize = 15;

			return true;
		}

        #endregion

        #region Destroy
        #endregion

        #region Window

        public static void OpenWindow()
		{
			window = GetWindow<NewUnitWindow>("Craete New Unit");

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

        private void SetInfoMessage()
		{
			switch (messageID)
			{
				case 0:
					message = "Can not create new Unit with emtpy name";
					messageType = MessageType.Warning;
					break;

				case 1:
					message = "Unit with name " + unitName + " already exists - Cannot create new Unit";
					messageType = MessageType.Error;
					break;

				case -1:
				default:
					message = string.Empty;
					messageType = MessageType.None;
					break;
			}
		}

		private void CreateNewUnit()
        {
			if (string.IsNullOrEmpty(unitName)) return;

			bool unitExist = DataHandler.Instance.UnitNameExistanceCheck(unitName, unitType);

			if (unitExist)
            {
				messageID = 1;
				SetInfoMessage();
				Debug.LogError(message);
				return;
            }

			bool success = DataHandler.Instance.CreateNewUnit(unitType, unitName);

			if (success) NewUnitCreated?.Invoke();
			Close();
		}
	}
}
