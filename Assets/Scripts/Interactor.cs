using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactRange;
    private PlayerActions playerActions;

    void Start()
    {
        playerActions = GetComponentInParent<PlayerActions>();
        if (playerActions == null)
        {
            Debug.LogError("PlayerActions component not found in parent.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                    if (hitInfo.collider.gameObject.TryGetComponent<PickupObject>(out PickupObject pickupObject) && pickupObject.isKey)
                    {
                        playerActions.SetHeldKey(pickupObject);
                    }
                }
            }
        }
    }
}
