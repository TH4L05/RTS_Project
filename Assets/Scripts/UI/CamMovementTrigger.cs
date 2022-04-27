using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamMovementTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool forward;
    public bool backward;
    public bool left;
    public bool right;

    private bool active;

    public void OnPointerEnter(PointerEventData eventData)
    {
       active = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {       
        active = false;
    }

    private void Update()
    {
        if (!active) return;

        if (forward)
        {
            Game.Instance.CameraRig.MoveForward();
        }
        else if (backward)
        {
            Game.Instance.CameraRig.MoveBackward();
        }
        else if (left)
        {
            Game.Instance.CameraRig.MoveLeft();
        }
        else if (right)
        {
            Game.Instance.CameraRig.MoveRight();
        }
    }
}
