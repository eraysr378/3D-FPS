using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;

    private InputManager inputManager;
    private PlayerUI playerUI;
    private Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }


    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance,Color.green);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance, mask))
        {
            if(hit.collider.GetComponent<Interactable>() != null) 
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.GetPromptMessage());
                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.Interact();
                }
            }
        }
    }
}
