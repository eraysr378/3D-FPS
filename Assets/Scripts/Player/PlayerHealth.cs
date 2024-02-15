using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health System")]
    [SerializeField] private Image fillImageFront;
    [SerializeField] private Image fillImageBack;
    [SerializeField] private float maxHealth;
    private float chipSpeed = 2f;
    private float lerpTimer;
    private float currentHealth;
    [Header("Damage Overlay")]
    [SerializeField] private Image overlay;
    [SerializeField] private float duration;
    [SerializeField] private float fadeSpeed;
    private float maxAlpha = 100f / 255f;
    private float durationTimer;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBarUI();
        if (Input.GetKeyDown(KeyCode.B))
        {
            TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            RestoreHealth(10f);
        }
        if(overlay.color.a > 0)
        {
            if(currentHealth < 30)
            {
                return;
            }
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b,tempAlpha);
            }
        }
    }
    private void UpdateHealthBarUI()
    {
        float fillF = fillImageFront.fillAmount;
        float fillB = fillImageBack.fillAmount;
        float hFraction = currentHealth / maxHealth;
        if (fillB > hFraction)
        {
            fillImageFront.fillAmount = hFraction;
            fillImageBack.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            fillImageBack.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            fillImageBack.fillAmount = hFraction;
            fillImageBack.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            fillImageFront.fillAmount = Mathf.Lerp(fillF, fillImageBack.fillAmount, percentComplete);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        lerpTimer = 0;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, maxAlpha);

    }
    public void RestoreHealth(float healAmount)
    {
        currentHealth += healAmount;

        lerpTimer = 0f;

    }
}
