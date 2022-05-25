/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using UnitEditor.UI.Custom;
using UnitEditor.Data;

namespace UnitEditor.UI.Section
{
    public class BuildingSpecificSection : UnitDataSection
    {
        #region Fields

        private ReorderableList producedResourecesList;
        private ReorderableList suppliedStartResources;

        #endregion

        public BuildingSpecificSection(SerializedObject so, GUISkin skin, Texture2D[] textures) 
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void Initialize()
        {
            base.Initialize();
            InitLists();
        }

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("producedResources");
            properties[1] = serializedObject.FindProperty("productionSpeed");
            properties[2] = serializedObject.FindProperty("suppliedResourcesOnStart");
        }

        private void InitLists()
        {
            producedResourecesList = new ReorderableList(properties[0].serializedObject, properties[0], false, false, false, false);
            producedResourecesList.drawElementCallback = DrawProducedResourcesListItems;

            suppliedStartResources = new ReorderableList(properties[2].serializedObject, properties[2], false, false, false, false);
            suppliedStartResources.drawElementCallback = DrawSuppliedStartResourcesListItems;            
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            GUILayout.BeginArea(baseRect);
            {
                MyGUI.DrawColorRect(new Rect(0f, 0f, baseRect.width, baseRect.height), sectionColor);
                Rect sectionRect = new Rect(15f, 10f, 650f, baseRect.height - 15f);

                GUILayout.BeginArea(sectionRect);
                {
                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(properties[1], GUILayout.Width(200f));

                        EditorGUILayout.Space(5f);

                        EditorGUILayout.BeginVertical(GUILayout.Width(450f));

                        EditorGUILayout.LabelField("ProducedResources", mySkin.GetStyle("baseLabelField"));

                        EditorGUILayout.Space(2f);

                        producedResourecesList.elementHeight = 32f;
                        producedResourecesList.DoLayoutList();

                        EditorGUILayout.Space(10f);

                        EditorGUILayout.LabelField("SuppliedResOnStart", mySkin.GetStyle("baseLabelField"));

                        EditorGUILayout.Space(2f);

                        suppliedStartResources.elementHeight = 32f;
                        suppliedStartResources.DoLayoutList();

                    EditorGUILayout.EndVertical();
                }              
                GUILayout.EndArea();
            }               
            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion

        #region Reorderable Lists

        private void DrawProducedResourcesListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = producedResourecesList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty resourceType = element.FindPropertyRelative("resourceType");
            SerializedProperty amount = element.FindPropertyRelative("amount");

            Texture2D tex = new Texture2D(64,64);
            tex = DataHandler.Instance.IconTextures[index + 5];

            EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, 32f, 32f), tex);
            EditorGUI.LabelField(new Rect(rect.x + 70f, rect.y + 4f, 150f, 25f), ((ResourceType)resourceType.enumValueFlag).ToString());
            EditorGUI.LabelField(new Rect(rect.x + 230f, rect.y + 4f, 50f, 25f), "amount:");
            amount.intValue = EditorGUI.IntField(new Rect(rect.x + 300f, rect.y + 4f, 75f, 25f), amount.intValue);
        }

        private void DrawSuppliedStartResourcesListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = suppliedStartResources.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty resourceType = element.FindPropertyRelative("resourceType");
            SerializedProperty amount = element.FindPropertyRelative("amount");

            Texture2D tex = new Texture2D(64, 64);
            tex = DataHandler.Instance.IconTextures[index + 5];

            EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, 32f, 32f), tex);
            EditorGUI.LabelField(new Rect(rect.x + 70f, rect.y + 4f, 150f, 25f), ((ResourceType)resourceType.enumValueFlag).ToString());
            EditorGUI.LabelField(new Rect(rect.x + 230f, rect.y + 4f, 50f, 25f), "amount:");
            amount.intValue = EditorGUI.IntField(new Rect(rect.x + 300f, rect.y + 4f, 75f, 25f), amount.intValue);
        }

        #endregion
    }
}
