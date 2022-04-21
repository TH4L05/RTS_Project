
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private TextMeshProUGUI barText;
    [SerializeField] private bool showBarText = false;
    [SerializeField] private bool billboardBar = true;
    private Camera cam;

    private void Awake()
    {
        if (billboardBar)
        {
            cam = Camera.main;
        }
    }

    private void OnDestroy()
    {
        
    }

    private void LateUpdate()
    {
        if (cam != null && billboardBar)
        {
            LookAtCamera();
        }
    }

    private void LookAtCamera()
    {
        transform.LookAt(transform.position + cam.transform.forward);
    }

    public void UpdateValue(float currentValue, float maxValue)
    {
        if(barImage != null) barImage.fillAmount = currentValue / maxValue;
        if (barText != null && showBarText) barText.text = $"{currentValue} / {maxValue} ";
    }
}
