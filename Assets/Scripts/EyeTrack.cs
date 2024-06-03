using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrack : MonoBehaviour
{
    public GameObject player; // Public variable to assign the player GameObject

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;

            // Create a rotation that looks at the player
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // Apply the rotation with an additional 180 degrees adjustment around the Y-axis to correct the orientation
            transform.rotation = lookRotation * Quaternion.Euler(0, 90, 0);
        }
    }
}
