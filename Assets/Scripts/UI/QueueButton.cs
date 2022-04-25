using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image backImage;
    [SerializeField] protected Image fillImage;
    private GameObject selectedObject;
    private float fillamount;
    private int index;

    public void ChangeVisibility(bool visible)
    {
        button.gameObject.SetActive(visible);

        if (visible)
        {
            StartFill(5f);
        }

    }

    public void StartFill(float time)
    {
        fillamount = 0;
        index = (int)(time * 10);
        InvokeRepeating("UpdateIconFillAmount", 0, 0.05f);
    }

    private void UpdateIconFillAmount()
    {
        fillImage.fillAmount = fillamount;
        fillamount += 0.01f;
        Debug.Log(fillImage.fillAmount);

        if (fillImage.fillAmount == 1)
        {
            CancelInvoke("UpdateIconFillAmount");
            button.gameObject.SetActive(false);
        }
    }
}
