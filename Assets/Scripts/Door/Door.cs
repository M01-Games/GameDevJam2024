using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isLocked = false;
    [SerializeField]
    private bool isRotatingDoor = true;
    [SerializeField]
    private float speed = 1.0f;
    [Header("Rotation Configs")]
    private float rotationAmount = 90f;
    [SerializeField]
    private float forwardDirection = 0.0f;
    [Header("Sliding Configs")]
    [SerializeField]
    private float slideAmount = 1.25f;

    private Vector3 startRotation;
    private Vector3 startPosition;
    private Vector3 Forward;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        startPosition = transform.position;
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
            else
            {
                animationCoroutine = StartCoroutine(DoSlideOpen());
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

    private IEnumerator DoSlideOpen()
    {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(-slideAmount, 0, 0); // Slide to the left

        isOpen = true;

        float time = 0.0f;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(start, end, time);
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
            else
            {
                animationCoroutine = StartCoroutine(DoSlideClose());
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

    private IEnumerator DoSlideClose()
    {
        Vector3 start = transform.position;
        Vector3 end = startPosition; // Slide back to the original position

        isOpen = false;

        float time = 0.0f;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(start, end, time);
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
