using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsHoldingAndShooting : MonoBehaviour
{
    public GameObject ball;
    public float timeBetweenShots = .7f;
    public AudioClip shootSFX;

    AudioSource audioSource;
    private bool spawnRoutineRunning = false;
    private int ballCount = 4;

    //NOTE: We will need held balls to be part of a list so we can alternate baby balls with special balls, first in first out like in Ball X Pit
    //private List<GameObject> balls = new List<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        spawnRoutineRunning = true;

        while (ballCount > 0)
        {
            ballCount--;
            Instantiate(ball, transform.position + Vector3.forward, Quaternion.identity);

            audioSource.volume = .15f;
            audioSource.pitch = Random.Range(0.5f, 0.8f);
            audioSource.PlayOneShot(shootSFX);
            StopCoroutine(PunchVFX());
            StartCoroutine(PunchVFX());

            yield return new WaitForSeconds(timeBetweenShots);
        }

        spawnRoutineRunning = false;
    }

    public void AddBall()
    {
        ballCount++;

        if (!spawnRoutineRunning)
            StartCoroutine(SpawnBalls());
    }

    private IEnumerator PunchVFX()
    {
        GetComponent<MeshRenderer>().transform.localScale = Vector3.one * 1.15f;
        yield return new WaitForSeconds(0.08f);
        GetComponent<MeshRenderer>().transform.localScale = Vector3.one;
    }
}
