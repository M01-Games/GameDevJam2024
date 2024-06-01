using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainRot : MonoBehaviour
{
    public float speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime, Space.Self);
    }
}
