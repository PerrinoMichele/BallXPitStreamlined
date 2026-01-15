using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteraction : MonoBehaviour
{
    public float moveSpeed;
    public GameObject ball;

    Rigidbody rigidbody;
    public FloatingJoystick joystick;
    Quaternion rotation;
    private float joystickX;
    private float joystickY;
    private Vector3 lookDir;
    private bool isShooting = false;
    private int ballCount = 4;

    //private List<GameObject> balls = new List<GameObject>();

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rotation = Quaternion.Euler(0, 0, 0);

    }

    void Update()
    {
        joystickX = joystick.Horizontal;
        joystickY = joystick.Vertical;
        lookDir = rotation * new Vector3(joystickX, 0f, joystickY);
    }

    void FixedUpdate()
    {
        if (!isShooting)
        {
            StartCoroutine(SpawnBalls());
        }
        if (lookDir != Vector3.zero)
        {
            Move();
        }
        else if (lookDir == Vector3.zero)
        {
            Stop();
        }
        //Move();
        //if (!isShooting)
        //    StartCoroutine(SpawnBalls());
    }

    void Move()
    {
        Vector3 input = new Vector3(joystickX, 0f, joystickY);

        rigidbody.linearVelocity =
            rotation * new Vector3(
                input.x * moveSpeed,
                rigidbody.linearVelocity.y,
                input.z * moveSpeed
            );
    }

    void Stop()
    {
        transform.rotation = Quaternion.identity;
        rigidbody.linearVelocity = new Vector3(0, 0, 0);
    }

    IEnumerator SpawnBalls()
    {
        {
            isShooting = true;

            while (ballCount > 0)
            {
                Instantiate(ball, transform.position + Vector3.forward, Quaternion.identity);
                ballCount--;
                yield return new WaitForSeconds(0.5f);
            }

            isShooting = false;
        }
    }

    public void AddBall()
    {
        ballCount++;
    }
}
