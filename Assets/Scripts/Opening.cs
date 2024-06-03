using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject Sleep;
    public float wait;

    public void opening()
    {
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(wait);
        Sleep.SetActive(true);
        yield return new WaitForSeconds(3);
        player1.SetActive(false);
        player2.SetActive(true);
        Sleep.SetActive(false);
    }
}
