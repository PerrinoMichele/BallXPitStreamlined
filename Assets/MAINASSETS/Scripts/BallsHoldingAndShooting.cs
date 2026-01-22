using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        audioSource = FindFirstObjectByType<AudioSource>();
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
            if (!DOTween.IsTweening(transform))
            {
                transform.DOPunchScale(Vector3.one * 0.4f, 0.15f);
            }
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
}
