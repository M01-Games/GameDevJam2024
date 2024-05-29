using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isLocked = false; // New property to determine if the door is locked
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1.0f;
    [Header("Rotation Configs")]
    private float rotationAmount = 90f;
    [SerializeField]
    private float forwardDirection = 0.0f;

    private Vector3 startRotation;
    private Vector3 Forward;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        Forward = transform.right;
    }

    public void Open(Vector3 userPosition)
    {
        if (!isOpen)
        {
            if (isLocked)
            {
                Debug.Log("Door is locked.");
                return;
            }

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            if (isRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (userPosition - transform.position).normalized);
                animationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion start = transform.rotation;
        Quaternion end;

        if (forwardAmount >= forwardDirection)
        {
            end = Quaternion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }
        else
        {
            end = Quaternion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        }

        isOpen = true;

        float time = 0.0f;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(start, end, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            if (isRotatingDoor)
            {
                animationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.Euler(startRotation);

        isOpen = false;

        float time = 0.0f;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(start, end, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log("Door unlocked.");
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
