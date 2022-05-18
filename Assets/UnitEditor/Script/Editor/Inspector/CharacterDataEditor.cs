/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

using UnitEditor.Data;
using UnitEditor.UI.Section;
using UnitEditor.UI.Custom;

namespace UnitEditor.Inspector
{
    [CustomEditor(typeof(CharacterData))]
    public class CharacterDataEditor : UnitDataEditor
    {
        
    }
}
