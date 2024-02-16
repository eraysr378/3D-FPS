using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitDamageTextSpawner : MonoBehaviour
{
    public static HitDamageTextSpawner Instance;
    [SerializeField] private Transform parent;
    [SerializeField] private TextMeshProUGUI headshotText;
    [SerializeField] private TextMeshProUGUI bodyshotText;
    [SerializeField] private TextMeshProUGUI legshotText;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void SpawnHeadshotText(string text)
    {
        TextMeshProUGUI hitDamageText = Instantiate(headshotText, parent);
        hitDamageText.text = text;
        hitDamageText.gameObject.SetActive(true);
    }
    public void SpawnBodyshotText(string text)
    {
        TextMeshProUGUI  hitDamageText = Instantiate(bodyshotText, parent);
        hitDamageText.text = text;
        hitDamageText.gameObject.SetActive(true);
    }
    public void SpawnLegshotText(string text)
    {
        TextMeshProUGUI hitDamageText = Instantiate(legshotText, parent);
        hitDamageText.text = text;
        hitDamageText.gameObject.SetActive(true);
    }
}
