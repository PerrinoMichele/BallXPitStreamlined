using System;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Events;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BallTypePrefab
{
    public BallType BallType;
    public GameObject BallPrefab;
}

public class BallsHoldingAndShooting : MonoBehaviour
{
    [SerializeField] private BallTypePrefab[] ballTypePrefabsList;
    public float timeBetweenShots = .7f;
    public AudioClip shootSFX;

    AudioSource audioSource;
    private bool spawnRoutineRunning = false;

    private List<BallType> ballTypeList;

    void Start()
    {
        ballTypeList = new List<BallType>() { BallType.HorizontalLaser, BallType.VerticalLaser, BallType.HorizontalLaser, BallType.VerticalLaser };
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SpawnBalls());

        EventBus.Subscribe<BallCollectedEvent>(AddBall);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<BallCollectedEvent>(AddBall);

    }

    IEnumerator SpawnBalls()
    {
        spawnRoutineRunning = true;

        while (ballTypeList.Count > 0)
        {
            var prefab = ballTypePrefabsList.FirstOrDefault(ball => ball.BallType == ballTypeList[0]).BallPrefab;
            Instantiate(prefab, transform.position + Vector3.forward, Quaternion.identity);

            audioSource.volume = .15f;
            audioSource.pitch = Random.Range(0.5f, 0.8f);
            audioSource.PlayOneShot(shootSFX);
            StopCoroutine(PunchVFX());
            StartCoroutine(PunchVFX());

            yield return new WaitForSeconds(timeBetweenShots);

            ballTypeList.RemoveAt(0);
        }

        spawnRoutineRunning = false;
    }

    public void AddBall(BallType ballType)
    {
        ballTypeList.Add(ballType);
        if (!spawnRoutineRunning)
            StartCoroutine(SpawnBalls());
    }

    public void AddBall(BallCollectedEvent ballCollectedEvent)
    {
        ballTypeList.Add(ballCollectedEvent.BallType);
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
