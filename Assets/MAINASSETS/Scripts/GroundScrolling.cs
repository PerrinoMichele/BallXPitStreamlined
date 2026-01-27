using System;
using UnityEngine;

public class GroundScrolling : MonoBehaviour
{

    public float speed;


    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= -40)
        {
            Vector3 p = transform.position;
            p.z = 40;
            transform.position = p;
        }
    }
}
