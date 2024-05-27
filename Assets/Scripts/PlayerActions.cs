using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

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

    public void OnUse()
    {
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxUseDistance, useLayers))
        {
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
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
        if (Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, maxUseDistance, useLayers)
            && hit.collider.TryGetComponent<Door>(out Door door))
        {
            if (door.isOpen)
                useText.SetText("Close \"E\"");
            else
                useText.SetText("Open \"E\"");

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
            useText.gameObject.SetActive(false);
    }
}
