/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using UnitEditor.Window;
using UnitEditor.UI.Custom;
using UnitEditor.Data;


namespace UnitEditor.UI.Section
{
    public class BaseSection : UnitDataSection
    {
        #region Fields

        private ReorderableList requiredResourecesList;

        #endregion

        public BaseSection(SerializedObject so, GUISkin skin, Texture2D[] textures) 
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void Initialize()
        {
            base.Initialize();
            InitList();
        }

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("name");
            properties[1] = serializedObject.FindProperty("tooltip");
            properties[2] = serializedObject.FindProperty("selectionInfoIcon");
            properties[3] = serializedObject.FindProperty("requiredResources");
        }

        private void InitList()
        {
            requiredResourecesList = new ReorderableList(properties[3].serializedObject, properties[3], false, false, false, false);
            requiredResourecesList.drawElementCallback = DrawRequiredResouresListElements;
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            Rect sectionRect = baseRect;
            GUILayout.BeginArea(baseRect);
            MyGUI.DrawColorRect(new Rect(0f, 0f, sectionRect.width, sectionRect.height), sectionColor);
            Texture2D iconTexture = iconTextures[0];
            if (properties[2].objectReferenceValue != null)
            {
                Sprite iconSprite = properties[2].objectReferenceValue as Sprite;
                iconTexture = iconSprite.texture;
            }

            sectionRect = new Rect(sectionRect.x + 5f, sectionRect.y + 5f, 150f, 150f);
            EditorGUI.DrawPreviewTexture(sectionRect, iconTexture);

            sectionRect = new Rect(baseRect.width - 265f, sectionRect.y, 125f, 135f);
            GUILayout.BeginArea(sectionRect);
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Edit Components", GUILayout.Height(40f)))
            {
                if (ComponentsWindow.IsOpen)
                {
                    ComponentsWindow.CloseWindow();
                }
                ComponentsWindow.OpenWindow();

                var obj = DataHandler.Instance.ActiveObj;                
                ComponentsWindow.SetObject(obj);
            }
            EditorGUILayout.Space(5f);
            if (GUILayout.Button("Load Data from File", GUILayout.Height(40f)))
            {
                if (LoadFromFileWIndow.IsOpen)
                {
                    LoadFromFileWIndow.CloseWindow();
                }
                LoadFromFileWIndow.OpenWindow();
            }
            EditorGUILayout.Space(5f);
            GUILayout.Button("Add To Scene", GUILayout.Height(40f));
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            sectionRect = new Rect(185f, sectionRect.y, 256f, 75f);
            GUILayout.BeginArea(sectionRect);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("UnitName", mySkin.GetStyle("baseLabelField"));
            EditorGUI.PropertyField(new Rect(0f, 20f, sectionRect.width, EditorGUIUtility.singleLineHeight * 2), properties[0], GUIContent.none);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            sectionRect = new Rect(sectionRect.x, sectionRect.y + sectionRect.height + 2f, 256f, 75f);
            GUILayout.BeginArea(sectionRect);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Tooltip", mySkin.GetStyle("baseLabelField"));
            EditorGUI.PropertyField(new Rect(0f, 20f, sectionRect.width, EditorGUIUtility.singleLineHeight * 2), properties[1], GUIContent.none);
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();

            sectionRect = new Rect(baseRect.x + 5f, 200f, 450f, 185f);
            GUILayout.BeginArea(sectionRect);
            GUIStyle labelStyle = new GUIStyle(mySkin.GetStyle("baseLabelField"));
            EditorGUILayout.LabelField("Requrired Resources", labelStyle);
            EditorGUILayout.Space(2f);
            requiredResourecesList.elementHeight = 32f;
            requiredResourecesList.DoLayoutList();
            GUILayout.EndArea();

            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion


        private void DrawRequiredResouresListElements(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = requiredResourecesList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty resourceType = element.FindPropertyRelative("resourceType");
            SerializedProperty amount = element.FindPropertyRelative("amount");

            Texture2D tex = new Texture2D(64, 64);
            tex = DataHandler.Instance.IconTextures[index + 5];

            EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, 32f, 32f), tex);
            EditorGUI.LabelField(new Rect(rect.x + 70f, rect.y + 4f, 150f, 25f), ((ResourceType)resourceType.enumValueFlag).ToString());
            EditorGUI.LabelField(new Rect(rect.x + 230f, rect.y + 4f, 50f, 25f), "amount:");
            amount.intValue = EditorGUI.IntField(new Rect(rect.x + 300f, rect.y + 4f, 75f, 25f), amount.intValue);
        }
    }
}

