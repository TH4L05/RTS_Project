using System.Collections;
using System.Collections.Generic;
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
}
