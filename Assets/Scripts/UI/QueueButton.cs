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
    [SerializeField] protected Sprite defaultSprite;

    private void Awake()
    {
        backImage.sprite = defaultSprite;
    }

    public void SetIcon(Sprite sprite)
    {
        backImage.sprite = sprite;
    }

    public void ShowButton()
    {
        gameObject.SetActive(true);
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
        fillImage.fillAmount = 0;
        backImage.sprite = defaultSprite;
    }

    public void SetFillAmount(float amount)
    {
        fillImage.fillAmount = amount;
    }


    /*public void RestartFill()
    {
        fillamount = lastfillamount;
        StartCoroutine(UpdateFill());
    }

    public void StartFill(float time)
    {
        if (fillstartet)
        {
            RestartFill();
            return;
        }

        fillstartet = true;
        fillamount = 0;
        //Debug.Log(updateAmount);
        //InvokeRepeating("UpdateIconFillAmount", 0, 0.05f);
        StartCoroutine(UpdateFill());
    }

    IEnumerator UpdateFill()
    {
        while (fillamount < 1)
        {
            fillImage.fillAmount = fillamount;
            fillamount += updateAmount;
            lastfillamount = fillamount;
            yield return null;
        }

        fillstartet=false;
        fillamount = 0f;
        lastfillamount = 0f;
        button.gameObject.SetActive(false);
        backImage.sprite = defaultSprite;

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
            backImage.sprite = defaultSprite;
        }
    }*/
}
