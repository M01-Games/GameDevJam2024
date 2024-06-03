using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro useText;
    [SerializeField]
    private Transform camera;
    [SerializeField]
    private float maxUseDistance = 5f;
    [SerializeField]
    private LayerMask useLayers;

    private PickupObject heldKey = null; // Store the currently held key

    private void OnEnable()
    {
        PickupObject.OnObjectDropped += HandleObjectDropped;
    }

    private void OnDisable()
    {
        PickupObject.OnObjectDropped -= HandleObjectDropped;
    }

    private void HandleObjectDropped(PickupObject obj)
    {
        if (obj == heldKey)
        {
            heldKey = null;
        }
    }

    public void OnUse()
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxUseDistance, useLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.isLocked)
                {
                    if (heldKey != null && heldKey.isKey)
                    {
                        door.Unlock();
                        Destroy(heldKey.gameObject);
                        heldKey = null;
                    }
                    else
                    {
                        Debug.Log("Door is locked and you don't have a key.");
                        return;
                    }
                }

                if (door.isOpen)
                {
                    Debug.Log("Door closing");
                    door.Close();
                    Debug.Log("Door closed");
                }
                else
                {
                    Debug.Log("Door opening");
                    door.Open(transform.position);
                    Debug.Log("Door opened");
                }
            }
        }
    }

    private void Update()
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxUseDistance, useLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                useText.SetText(door.isLocked ? "Door is locked" : door.isOpen ? "Close \"E\"" : "Open \"E\"");
                useText.gameObject.SetActive(true);
                Vector3 directionToCamera = (camera.position - hit.point).normalized;
                Vector3 textPosition = hit.point + directionToCamera * 1f;
                useText.transform.position = textPosition;
                useText.transform.rotation = Quaternion.LookRotation(-directionToCamera);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OnUse();
                }
            }
            else
            {
                useText.gameObject.SetActive(false);
            }
        }
        else
        {
            useText.gameObject.SetActive(false);
        }
    }

    public void SetHeldKey(PickupObject key)
    {
        heldKey = key;
    }
}
