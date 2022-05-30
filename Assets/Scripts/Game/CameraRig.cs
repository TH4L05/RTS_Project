/// <author> Thomas Krahl </author>

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRig : MonoBehaviour
{
    #region Fields

    [Header("Base")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private float movetime = 5f;
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
            MoveForward();
        }
        if (Keyboard.current.downArrowKey.isPressed | Keyboard.current.sKey.isPressed)
        {
            MoveBackward();
        }
        if (Keyboard.current.leftArrowKey.isPressed | Keyboard.current.aKey.isPressed)
        {
            MoveLeft();
        }
        if (Keyboard.current.rightArrowKey.isPressed | Keyboard.current.dKey.isPressed)
        {
            MoveRight();
        }

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * movetime);
        
    }

    public void MoveForward()
    {
        position += transform.forward * speed;
    }

    public void MoveBackward()
    {
        position += transform.forward * -speed;
    }

    public void MoveLeft()
    {
        position += transform.right * -speed;
    }

    public void MoveRight()
    {
        position += transform.right * speed;
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
            height += scrollSpeed;
        }       
        else if (mouseScroll.y < 0)
        {
            height -= scrollSpeed;
        }

        position.y = height;
    }

    #endregion

}
