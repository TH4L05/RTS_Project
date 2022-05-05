using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEditorInternal;
using UnitEditor.CustomGUI;

[CustomEditor(typeof(BuildingData))]
public class BuildingDataEditor : Editor
{
    private new SerializedProperty name;
    private SerializedProperty type;
    private SerializedProperty tooltip;
    private SerializedProperty buildTime;
    private SerializedProperty deathTime;
    private SerializedProperty requiredResources;

    private SerializedProperty healthMax;
    private SerializedProperty healthRegen;
    private SerializedProperty healthRegenRate;
    private SerializedProperty manaMax;
    private SerializedProperty manaRegen;
    private SerializedProperty manaRegenRate;
    private SerializedProperty armor;

    private SerializedProperty attackRange;
    private SerializedProperty actionRange;
    private SerializedProperty attackSpeed;
    private SerializedProperty baseDamage;
    private SerializedProperty weapon;
    private SerializedProperty weaponAttackOffsetTime;

    private SerializedProperty test;
    private SerializedProperty abilities;

    bool[] checks = new bool[12];

    private void OnEnable()
    {
        SetProperties();
    }

    private void SetProperties()
    {
        name = serializedObject.FindProperty("name");
        type = serializedObject.FindProperty("type");
        tooltip = serializedObject.FindProperty("tooltip");
        buildTime = serializedObject.FindProperty("buildTime");
        deathTime = serializedObject.FindProperty("deathTime");
        requiredResources = serializedObject.FindProperty("requiredResources");

        test = serializedObject.FindProperty("test");
        abilities = serializedObject.FindProperty("abilities");    
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.DrawDefaultInspector();

        GUIStyle toplabelStyle = new GUIStyle();
        toplabelStyle.alignment = TextAnchor.MiddleLeft;
        toplabelStyle.normal.textColor = Color.red;
        toplabelStyle.fontSize = 20;

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = new Color(0.15f, 0.15f, 0.15f);
        labelStyle.fontSize = 10;


        GUILayout.Label("Abilities:", toplabelStyle);

        Rect rect = new Rect(5f, 35f, 128f, 128f);
        int index = 0;
        var enumerator = abilities.GetEnumerator();      
        while (enumerator.MoveNext())
        {
            MyGUI.DrawColorRect(rect, Color.gray);
            var prop = enumerator.Current as SerializedProperty;


            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 128f, 25f), prop, GUIContent.none);
            
            

            if (prop.objectReferenceValue != null)
            {
                SerializedObject so = new SerializedObject(prop.objectReferenceValue);

                SerializedProperty name = so.FindProperty("name");
                SerializedProperty icon = so.FindProperty("icon");

                Texture2D iconTexture = new Texture2D(64, 64);

                if (icon.objectReferenceValue != null)
                {
                    Sprite iconSprite = icon.objectReferenceValue as Sprite;
                    iconTexture = iconSprite.texture;
                }

                EditorGUI.LabelField(new Rect(rect.x + 3f, rect.y + 26f, rect.width - 3f, EditorGUIUtility.singleLineHeight), name.stringValue, labelStyle);
                EditorGUI.DrawPreviewTexture(new Rect(rect.x +32f , rect.y + 50f, 64f, 64f), iconTexture);
            }



            if (index == 3 || index == 7)
            {
                rect.y += rect.height + 2f;
                rect.x = 5f;
            }
            else
            {
                rect.x += rect.width + 2f;

            }

            index++;
        }
       
        serializedObject.ApplyModifiedProperties();
    }

    
}
