using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    public Door door;
    public Transform player;
    public int requiredTriggerCount = 2;
    private int triggerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            triggerCount++;
            Debug.Log("Trigger activated " + triggerCount + " times.");

            if (triggerCount == requiredTriggerCount)
            {
                door.Close();
            }
        }
    }
}
