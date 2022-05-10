/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class BuildingSpecificSection : UnitDataSection
    {
        #region Fields
        private ReorderableList producedResourecesList;
        private ReorderableList suppliedStartResources;

        #endregion

        public BuildingSpecificSection(SerializedObject so, GUISkin skin, Texture2D[] textures) : base(so, skin, textures)
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
            producedResourecesList = new ReorderableList(properties[0].serializedObject, properties[0], true, false, true, true);
            //producedResourecesList.drawHeaderCallback = DrawProducedResourcesListHeader;
            producedResourecesList.drawElementCallback = DrawProducedResourcesListItems;

            suppliedStartResources = new ReorderableList(properties[2].serializedObject, properties[2], false, false, true, true);
            //suppliedStartResources.drawHeaderCallback = DrawProducedResourcesListHeader;
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
                GUIStyle labelStyle = new GUIStyle(mySkin.GetStyle("baseLabelField"));

                GUILayout.BeginArea(sectionRect);
                {
                    EditorGUILayout.Space(5f);
                    EditorGUILayout.PropertyField(properties[1]);
                    EditorGUILayout.Space(5f);
                    EditorGUILayout.LabelField("ProducedResources", labelStyle);
                    EditorGUILayout.Space(2f);
                    producedResourecesList.elementHeight = 32f;
                    producedResourecesList.DoLayoutList();
                    EditorGUILayout.Space(10f);
                    EditorGUILayout.LabelField("SuppliedResOnStart", labelStyle);
                    EditorGUILayout.Space(2f);
                    suppliedStartResources.elementHeight = 32f;
                    suppliedStartResources.DoLayoutList();
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

            SerializedProperty resourceData = element.FindPropertyRelative("ResoureData");
            SerializedProperty amount = element.FindPropertyRelative("amount");
            SerializedProperty resourceIcon;
            SerializedProperty resourceType;
            string name = string.Empty;

            Texture2D tex = new Texture2D(64,64);

            if (resourceData.objectReferenceValue != null)
            {
                SerializedObject so = new SerializedObject(resourceData.objectReferenceValue);

                resourceIcon = so.FindProperty("icon");
                resourceType = so.FindProperty("type");
                name = ((ResourceType)resourceType.enumValueIndex - 1).ToString();

                if (resourceIcon.objectReferenceValue != null)
                {
                    Sprite sprite = resourceIcon.objectReferenceValue as Sprite;
                    tex = sprite.texture;
                }

                EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, 32f, 32f), tex);
                EditorGUI.LabelField(new Rect(rect.x + 45f, rect.y + 4f, 125f, 25f), name);

            }
                      
            EditorGUI.PropertyField(new Rect(rect.x + 180f, rect.y + 4f, 225f, 25f), resourceData, GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x + 420f, rect.y + 4f, 50f, 25f), "amount:");
            amount.intValue = EditorGUI.IntField(new Rect(rect.x + 475f, rect.y + 4f, 75f, 25f), amount.intValue);
        }

        private void DrawSuppliedStartResourcesListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = suppliedStartResources.serializedProperty.GetArrayElementAtIndex(index);

            SerializedProperty resourceData = element.FindPropertyRelative("ResoureData");
            SerializedProperty amount = element.FindPropertyRelative("amount");
            SerializedProperty resourceIcon;
            SerializedProperty resourceType;
            string name = string.Empty;

            Texture2D tex = new Texture2D(64, 64);

            if (resourceData.objectReferenceValue != null)
            {
                SerializedObject so = new SerializedObject(resourceData.objectReferenceValue);

                resourceIcon = so.FindProperty("icon");
                resourceType = so.FindProperty("type");
                name = ((ResourceType)resourceType.enumValueIndex - 1).ToString();

                if (resourceIcon.objectReferenceValue != null)
                {
                    Sprite sprite = resourceIcon.objectReferenceValue as Sprite;
                    tex = sprite.texture;
                }

                EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, 32f, 32f), tex);
                EditorGUI.LabelField(new Rect(rect.x + 45f, rect.y + 4f, 125f, 25f), name);

            }

            EditorGUI.PropertyField(new Rect(rect.x + 180f, rect.y + 4f, 225f, 25f), resourceData, GUIContent.none);
            EditorGUI.LabelField(new Rect(rect.x + 420f, rect.y + 4f, 50f, 25f), "amount:");
            amount.intValue = EditorGUI.IntField(new Rect(rect.x + 475f, rect.y + 4f, 75f, 25f), amount.intValue);
        }


        private void DrawProducedResourcesListHeader(Rect rect)
        {
            string name = "ProducedResources";
            EditorGUI.LabelField(rect, name);
        }

        #endregion
    }
}
