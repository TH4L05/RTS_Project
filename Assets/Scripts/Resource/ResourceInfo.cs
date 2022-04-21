using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetSprite(Sprite sp)
    {
        icon.sprite = sp;
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
