using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lerptime = 5f;
    private Vector3 position;

    private void Start()
    {
        position = transform.position;
    }

    private void Update()
    {
        MovementHandle();
    }

    private void MovementHandle()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            position += transform.forward * speed;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            position += transform.forward * -speed;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            position += transform.right * -speed;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            position += transform.right * speed;
        }

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * lerptime);
    }
}
