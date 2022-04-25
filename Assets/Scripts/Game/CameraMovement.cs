

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    #region Fields

    [Header("Base")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float movetime = 5f;
    [SerializeField] private float scrolltime = 5f;
    private Vector3 position;
    private float height;

    [Header("Boundaries")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;
    [SerializeField] private float minHeight = 1f;
    [SerializeField] private float maxHeight = 2f;

    #endregion

    #region UnityFunctions

    private void Start()
    {
        position = transform.position;
        height = transform.position.y;
    }

    #endregion

    #region Handling

    private void Update()
    {
        Movement();
        Zoom();
        CheckBoundaries();
    }

    private void Movement()
    {
        if (Keyboard.current.upArrowKey.isPressed | Keyboard.current.wKey.isPressed)
        {
            position += transform.forward * speed;
        }
        if (Keyboard.current.downArrowKey.isPressed | Keyboard.current.sKey.isPressed)
        {
            position += transform.forward * -speed;
        }
        if (Keyboard.current.leftArrowKey.isPressed | Keyboard.current.aKey.isPressed)
        {
            position += transform.right * -speed;
        }
        if (Keyboard.current.rightArrowKey.isPressed | Keyboard.current.dKey.isPressed)
        {
            position += transform.right * speed;
        }

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * movetime);
        
    }

    private void CheckBoundaries()
    {

        if (height < minHeight)
        {
            height = minHeight;
        }

        if (height > maxHeight)
        {
            height = maxHeight;
        }


        if (position.x < minX)
        {
            position.x = minX;
        }

        if (position.x > maxX)
        {
            position.x = maxX;
        }

        if (position.z < minZ)
        {
            position.z = minZ;
        }

        if (position.z > maxZ)
        {
            position.z = maxZ;
        }
    }

    private void Zoom()
    {
        Vector2 mouseScroll = Mouse.current.scroll.ReadValue();

        if (mouseScroll.y > 0)
        {
            height += 0.5f;
        }       
        else if (mouseScroll.y < 0)
        {
            height -= 0.5f;
        }

        position.y = height;
    }

    #endregion

}
