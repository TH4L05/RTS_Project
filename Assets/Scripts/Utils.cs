/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEngine.InputSystem;

public class Utils : MonoBehaviour
{
    public static Vector2 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        //origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        //corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        Rect rect = Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);

        return rect;
    }

    public static void DrawScreenRect(Rect rect, Color color, Texture texture)
    {
        

        GUI.color = color;
        GUI.DrawTexture(rect, texture);
        GUI.color = Color.white;
    }

    public static UnitData GetUnitData(Unit unit)
    {
        UnitType type = unit.UnitData.Type;

        switch (type)
        {
            default:
                return null;


            case UnitType.Building:
                return unit.UnitData as BuildingData;


            case UnitType.Character:
                return unit.UnitData as CharacterData;

        }
    }

    public static float GetDistance(Vector3 vec1, Vector3 vec2)
    {
        float distance = Vector3.Distance(vec1, vec2);
        return distance;
    }
}
