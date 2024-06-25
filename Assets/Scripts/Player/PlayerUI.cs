using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI promptText;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        promptText = GameObject.Find("/Canvas/PromptText").GetComponent<TextMeshProUGUI>();
    }
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
