/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using UnitEditor.Attributes;

namespace UnitEditor.UI.Section
{
    // Test
    [CustomPropertyDrawer(typeof(NoLabelAttribute))]
    public class NoLabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect rect = new Rect(position.x, position.y, position.width, position.height);

            EditorGUI.PropertyField(rect, property, GUIContent.none);
        }
    }

    // Test
    /*[CustomPropertyDrawer(typeof(Ability))]
    public class AbilitiyDrawer : PropertyDrawer
    {
        private List<SerializedProperty> abilityProperties = new List<SerializedProperty>();
        private bool edit = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            return 264;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {        
            if (property.objectReferenceValue != null)
            {
                abilityProperties.Clear();
                SerializedObject so = new SerializedObject(property.objectReferenceValue);
                SetAbilitiyProperties(so);
                //Debug.Log(abilityProperties.Count);
                DrawElements(position, property, label);
                so.ApplyModifiedProperties();
            }     
            else
            {
                position.height = 16f;
                EditorGUI.PropertyField(position, property, GUIContent.none);
            }
        }

        private void SetAbilitiyProperties(SerializedObject so)
        {
            if (so == null) return;

            SerializedProperty name = so.FindProperty("name");
            abilityProperties.Add(name);
            SerializedProperty tooltip = so.FindProperty("tooltip");
            abilityProperties.Add(tooltip);
            SerializedProperty icon = so.FindProperty("icon");
            abilityProperties.Add(icon);
            SerializedProperty iconHighlighted = so.FindProperty("iconHighlighted");
            abilityProperties.Add(iconHighlighted);
            SerializedProperty iconPressed = so.FindProperty("iconPressed");
            abilityProperties.Add(iconPressed);
            SerializedProperty iconDisabled = so.FindProperty("iconDisabled");
            abilityProperties.Add(iconDisabled);
            SerializedProperty unitTemplate = so.FindProperty("unitTemplate");
            abilityProperties.Add(unitTemplate);
            SerializedProperty useTemplateSprites = so.FindProperty("useTemplateSprites");
            abilityProperties.Add(useTemplateSprites);
        }

        private Texture2D IconTexture()
        {
            Sprite iconSprite = abilityProperties[2].objectReferenceValue as Sprite;
            if (iconSprite == null) return new Texture2D(64, 64);
            return iconSprite.texture;
        }

        private void DrawElements(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 16f;
            EditorGUI.PropertyField(position, property, GUIContent.none);
            position.y += EditorGUIUtility.singleLineHeight + 2f;

            //Rect leftRect = new Rect(position.x, position.y, 64f, 64f);
            //Rect rightRect = new Rect(position.x + leftRect.width + 2f, position.y, position.width, 256 - 2f - position.height);
            //MyGUI.DrawColorRect(leftRect, Color.grey);
            //MyGUI.DrawColorRect(rightRect, Color.grey);


            EditorGUIUtility.labelWidth = 128f;       
            Texture2D iconTexture = IconTexture();
            EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y, 64f, 64f), iconTexture);

            position = new Rect(position.x + 72f, position.y + 4f, position.width - 85f, position.height + 2f);

            GUILayout.BeginArea(position);

            edit = GUILayout.Toggle(edit, "Edit", GUILayout.Height(22f), GUILayout.Width(100f));
            GUILayout.EndArea();

            position.y += 26f;

            if (!edit)
            {
                Debug.Log(true);
                EditorGUI.LabelField(position, abilityProperties[0].stringValue);
                return;
            }

                foreach (var abilityProperty in abilityProperties)
                {
                    EditorGUI.PropertyField(position, abilityProperty);
                    position.y += EditorGUIUtility.singleLineHeight;
                }
        }
    }*/
}
