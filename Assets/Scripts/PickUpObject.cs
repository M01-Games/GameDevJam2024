using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour, IInteractable
{
    public bool isPickedUp = false;
    public bool isInspecting = false;
    private Transform playerTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public Vector3 holdOffset = new Vector3(0.5f, -0.5f, 1f);
    public Vector3 holdRotation = new Vector3(0f, 0f, 0f);
    public Vector3 inspectPosition = new Vector3(0f, 0f, 1.5f);

    private Transform holdParent;
    private Rigidbody rb;
    private FPSController fpsController;

    public float rotationSpeed = 500f;
    public bool isKey = false; // Indicates whether this object is a key

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fpsController = FindObjectOfType<FPSController>();
    }

    public void Interact()
    {
        if (isPickedUp)
        {
            Drop();
        }
        else
        {
            PickUp();
        }
    }

    void PickUp()
    {
        playerTransform = Camera.main.transform;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        isPickedUp = true;
        rb.isKinematic = true;

        holdParent = new GameObject("HoldPoint").transform;
        holdParent.SetParent(playerTransform);
        holdParent.localPosition = holdOffset;
        holdParent.localRotation = Quaternion.Euler(holdRotation);

        transform.SetParent(holdParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isPickedUp = false;
        isInspecting = false;

        rb.isKinematic = false;
        transform.SetParent(null);
        transform.position = playerTransform.position + playerTransform.forward * holdOffset.z;
        transform.rotation = originalRotation;

        Destroy(holdParent.gameObject);

        if (fpsController != null)
        {
            fpsController.canMove = true;
        }
    }

    void Update()
    {
        if (isPickedUp)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Drop();
            }

            if (Input.GetMouseButtonDown(0))
            {
                ToggleInspect();
            }

            if (isInspecting)
            {
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                float rotationY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
                transform.Rotate(playerTransform.up, rotationX, Space.World);
                transform.Rotate(playerTransform.right, rotationY, Space.World);
            }
        }
    }

    void ToggleInspect()
    {
        isInspecting = !isInspecting;

        if (isInspecting)
        {
            transform.SetParent(playerTransform);
            transform.localPosition = inspectPosition;
            transform.localRotation = Quaternion.identity;

            // Disable the FPS controller
            if (fpsController != null)
            {
                fpsController.canMove = false;
            }
        }
        else
        {
            transform.SetParent(holdParent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            // Re-enable the FPS controller
            if (fpsController != null)
            {
                fpsController.canMove = true;
            }
        }
    }
}
