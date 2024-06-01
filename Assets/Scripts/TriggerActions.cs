using UnityEngine;
using System.Collections;

public class TriggerActions : MonoBehaviour
{
    public GameObject firstObject;
    public GameObject secondObject;
    public Transform player;
    public Collider triggerArea;
    public Crawler crawler;

    private int entryExitCount = 0;
    private bool isPlayerInArea = false;
    public bool roomChange = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            if (roomChange)
            {
                if (!isPlayerInArea)
                {
                    entryExitCount++;
                    isPlayerInArea = true;
                }

                if (entryExitCount >= 2)
                {
                    StartCoroutine(ToggleVisibilityAfterDelay(1.0f));
                }
            }
            else
            {
                crawler.canChasePlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            if (roomChange)
            {
                if (isPlayerInArea)
                {
                    isPlayerInArea = false;
                }
            }
                
        }
    }

    private IEnumerator ToggleVisibilityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        firstObject.SetActive(false);
        secondObject.SetActive(true);
    }
}
