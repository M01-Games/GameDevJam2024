using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestMove : MonoBehaviour
{
    public GameObject forest;
    public float speed = 3f;
    public float wait = 20f;
    public float addwait = 20f;
    public bool canMove = true;
    public bool switchTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Example());
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if(canMove == true)
        {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    void Respawn()
    {
        transform.position = new Vector3(-10f, 0.057f, 114f);
        wait = 0.0f;
    }

    IEnumerator Example()
    {
        if(switchTimer == false)
        {
            yield return new WaitForSeconds(wait);    
        }
        else
        {
            yield return new WaitForSeconds(addwait);
        }
        Respawn();
        switchTimer = true;
        StartCoroutine(Example());
    }
}