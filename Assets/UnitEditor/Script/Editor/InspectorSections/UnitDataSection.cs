/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;

using UnitEditor.UI.Custom;

namespace UnitEditor.UI.Section
{
    public class UnitDataSection
    {
        #region Fields

        protected SerializedObject serializedObject;
        protected GUISkin mySkin;
        protected Texture2D[] iconTextures;
        protected Color sectionColor = new Color(0.45f, 0.45f, 0.45f);
        protected SerializedProperty[] properties;

        #endregion

        public UnitDataSection(SerializedObject so, GUISkin skin, Texture2D[] textures)
        {
            serializedObject = so;
            mySkin = skin;
            iconTextures = textures;

            Initialize();
        }

        #region Initialize

        protected virtual void Initialize()
        {
            properties = new SerializedProperty[10];
            SetProperties();
        }

        protected virtual void SetProperties()
        {
        }

        #endregion
        
        #region Destroy

        protected virtual void Destroy()
        {
            if (serializedObject == null) return;
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region OnGUI

        public virtual void OnGUI(Rect baseRect)
        {
            if (serializedObject == null) return;
            SectionGUI(baseRect);
        }

        protected virtual void SectionGUI(Rect baseRect)
        {           
        }

        #endregion
    }
}
