using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerMotor playerMotor;
    [SerializeField] Color defaultColor;
    // Start is called before the first frame update
    void Start()
    {
        defaultColor = fillImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        fillImage.fillAmount = playerMotor.GetSlideTimeFraction();
        if(fillImage.fillAmount == 1)
        {
            fillImage.color = Color.green;
        }
        else
        {
            fillImage.color = defaultColor;
        }
    }
}
