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
    private bool spawnRoutineRunning = false;
    private int ballCount = 4;

    //private List<GameObject> balls = new List<GameObject>();

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        spawnRoutineRunning = true;

        while (ballCount > 0)
        {
            ballCount--;
            yield return new WaitForSeconds(1f);
            Instantiate(ball, transform.position + Vector3.forward, Quaternion.identity);
        }

        spawnRoutineRunning = false;
    }

    public void AddBall()
    {
        ballCount++;
        if (!spawnRoutineRunning)
            StartCoroutine(SpawnBalls());
    }
}
