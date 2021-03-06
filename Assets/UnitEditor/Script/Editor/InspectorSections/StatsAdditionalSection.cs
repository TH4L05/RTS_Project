/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class StatsAdditionalSection : UnitDataSection
    {
        #region Fields
        #endregion

        public StatsAdditionalSection(SerializedObject so, GUISkin skin, Texture2D[] textures)
            : base(so, skin, textures)
        {
        }

        #region Initialize

        protected override void SetProperties()
        {
            properties[0] = serializedObject.FindProperty("armor");
            properties[1] = serializedObject.FindProperty("attackRange");
            properties[2] = serializedObject.FindProperty("actionRange");
            properties[3] = serializedObject.FindProperty("weapon");
        }

        #endregion

        #region OnGUI

        protected override void SectionGUI(Rect baseRect)
        {
            Rect sectionRect = new Rect(15f, 10f, 420f, 175f);
            Rect imageRect = new Rect(15f, (sectionRect.height - 128f) / 2, 128f, 128f);
            Rect subSectionRect = new Rect(imageRect.x + imageRect.width, imageRect.y, 275f, 45f);
            Rect labelRect = new Rect(15f, 0f, 180f, 15f);
            Rect floatRect = new Rect(labelRect.x + labelRect.width, 0f, 50f, 15f);
            Rect sliderRect = new Rect(labelRect.x, labelRect.y + labelRect.height + 4f, 235f, 25f);

            GUILayout.BeginArea(baseRect);
            MyGUI.DrawColorRect(new Rect(0f, 0f, baseRect.width, baseRect.height), sectionColor);
        
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < 2; i++)
            {
                GUILayout.BeginArea(sectionRect);
                MyGUI.DrawColorRect(new Rect(0f, 0f, sectionRect.width, sectionRect.height), new Color(0.33f, 0.33f, 0.33f, 0.5f));
                EditorGUI.DrawPreviewTexture(imageRect, iconTextures[i + 3]);

                if (i == 0)
                {
                    GUILayout.BeginArea(subSectionRect);
                    EditorGUILayout.BeginVertical();
                    EditorGUI.LabelField(labelRect, properties[0].name);
                    properties[0].floatValue = EditorGUI.Slider(sliderRect, properties[0].floatValue, 0f, 999f);
                    EditorGUILayout.EndVertical();
                    GUILayout.EndArea();
                }
                else
                {
                    subSectionRect = new Rect(subSectionRect.x, subSectionRect.y, subSectionRect.width, 200f);
                    GUILayout.BeginArea(subSectionRect);

                    EditorGUI.LabelField(labelRect, "Weapon", mySkin.FindStyle("sliderLabelGrey"));
                    labelRect.y += labelRect.height + 1f;
                    EditorGUI.PropertyField(labelRect, properties[3], GUIContent.none);

                    if (properties[3].objectReferenceValue != null)
                    {
                        //EditorGUILayout.BeginVertical();
                        SerializedObject so = new SerializedObject(properties[3].objectReferenceValue);
                        GameObject go = so.targetObject as GameObject;
                        var weaponData = go.GetComponent<Weapon>().Data;

                        var attackType = weaponData.AttackType;
                        var damageType = weaponData.DamageType;
                        var baseDamage = weaponData.BaseDamage;

                        labelRect.y += labelRect.height + 10f;
                        EditorGUI.LabelField(labelRect, "AttackType   : " + attackType.ToString());
                        labelRect.y += labelRect.height + 2f;
                        EditorGUI.LabelField(labelRect, "DamageType : " + damageType.ToString());
                        labelRect.y += labelRect.height + 2f;
                        EditorGUI.LabelField(labelRect, "BaseDamage : " + baseDamage.ToString());

                        //EditorGUILayout.EndVertical();
                    }

                    GUILayout.EndArea();
                }

                GUILayout.EndArea();
                sectionRect.x += sectionRect.width + 15f;
            }
        
            sectionRect = new Rect(15f, sectionRect.y + sectionRect.height + 10f, sectionRect.width * 2 + 15f, 90f);
            GUILayout.BeginArea(sectionRect);
            {
                MyGUI.DrawColorRect(new Rect(0f, 0f, sectionRect.width, sectionRect.height), new Color(0.33f, 0.33f, 0.33f, 0.5f));

                subSectionRect = new Rect(0f, subSectionRect.y, subSectionRect.width, 200f);

                for (int i = 0; i < 2; i++)
                {
                    GUILayout.BeginArea(subSectionRect);
                    {
                        EditorGUILayout.BeginVertical();
                        labelRect = new Rect(15f, 0f, 180f, 15f);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.LabelField(labelRect, properties[1 + i].name, mySkin.FindStyle("sliderLabelGrey"));
                        properties[1 + i].floatValue = EditorGUI.IntField(floatRect, (int)properties[1 + i].floatValue, mySkin.customStyles[6]);
                        EditorGUILayout.EndHorizontal();

                        properties[1 + i].floatValue = GUI.HorizontalSlider(sliderRect, properties[1 + i].floatValue, 0f, 99f, mySkin.FindStyle("sliderGrey"), mySkin.FindStyle("sliderThumbGrey"));
                        EditorGUILayout.EndVertical();
                    }
                    GUILayout.EndArea();

                    subSectionRect.x += subSectionRect.width + 10f;
                }                             
            }               
            GUILayout.EndArea();

            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }

        #endregion

        #region Destroy
        #endregion  
    }
}
