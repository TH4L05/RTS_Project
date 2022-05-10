/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.Toolbar;
using UnitEditor.Data;
using UnitEditor.UI;
using UnitEditor.UI.Custom;

namespace UnitEditor
{
    public class UnitEditorWindow : EditorWindow
    {
        #region Events



        #endregion

        #region SerializedFields



        #endregion

        #region PrivateFields

        private static UnitEditorWindow window;
        private UnitEditorToolbar toolbar;
        private ButtonList buttonlist;
        private PropertiesArea propertiesArea;
        private bool setupDone = false;

        private Vector2 scrollPosition1 = Vector2.zero;
        private Vector2 scrollPosition2 = Vector2.zero;

        private Rect leftRect;
        private Rect rightRect;

        #endregion

        #region PublicFields

        public DataHandler dataHandler;
        public static bool NeedRepaint { get; set; }


        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            Initialize();
        }

        private void OnGUI()
        {
            if (!setupDone) return;

            toolbar.OnGUI();

            leftRect = new Rect(20f, 50f, 300f, window.position.size.y - 60f);
            rightRect = new Rect(leftRect.position.x + leftRect.size.x + 5f, 50f, window.position.size.x - leftRect.width - leftRect.x - 10f, window.position.size.y - 60f);
            
            LeftArea(leftRect);            
            RightArea(rightRect);

            Event e = Event.current;

            switch (e.type)
            {
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.Escape)
                    {
                        window.Close();                     
                    }
                    break;

                default:
                    break;
            }

            //if (Event.current.type == EventType.MouseMove) Repaint();
            //Repaint();          
        }

        private void OnDestroy()
        {
            if (toolbar != null)
            {
                toolbar.Destroy();
                DestroyImmediate(toolbar);
            }
            if (buttonlist != null)
            {
                buttonlist.Destroy();
                DestroyImmediate(buttonlist);
            }

            if (propertiesArea != null)
            {
                propertiesArea.Destroy();
                DestroyImmediate(propertiesArea);
            }
        }

        #endregion

        #region Initialize

        [MenuItem("UnitEditor/UnitEditorWindow")]
        public static void ShowWindow()
        {
            window = GetWindow<UnitEditorWindow>("UnitEditor");
            window.minSize = new Vector2(650f, 350f);
        }

        private void Initialize()
        {
            dataHandler = new DataHandler();

            bool setupSuccess = dataHandler.Setup();

            if (!setupSuccess)
            {
                window.Close();
                Debug.LogError("Setup Failed");
                return;
            }

            toolbar = new UnitEditorToolbar(this);
            buttonlist = new ButtonList(this, dataHandler);
            propertiesArea = new PropertiesArea(this, dataHandler);
            setupDone = true;
        }

        #endregion

        #region Destroy
        #endregion

        #region Areas

        private void LeftArea(Rect rect)
        {
            Rect viewRect = new Rect(0f, 0f, rect.width - 20f, 2000f);         
            scrollPosition1 = GUI.BeginScrollView(rect, scrollPosition1, viewRect);
            MyGUI.DrawColorRect(viewRect, new Color(0.35f, 0.35f, 0.35f));
            buttonlist.OnGUI();
            GUI.EndScrollView();
        }

        private void RightArea(Rect rect)
        {
            Rect viewRect = new Rect(0f, 0f, 900f, 2000f);
            scrollPosition2 = GUI.BeginScrollView(rect, scrollPosition2, viewRect);
            MyGUI.DrawColorRect(new Rect(viewRect.x, viewRect.y, rect.width, viewRect.height), new Color(0.35f, 0.35f, 0.35f));
            GUILayout.BeginArea(viewRect);
            propertiesArea.OnGUI();
            GUILayout.EndArea();
            GUI.EndScrollView();
        }

        #endregion
    }
}

