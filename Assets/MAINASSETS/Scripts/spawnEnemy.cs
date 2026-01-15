using System.Collections;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public float timeBetweenSpawns;
    public float spawnPercentage;
    public GameObject enemy;
    
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(timeBetweenSpawns);
        if (Random.value <= spawnPercentage)
        {
            Instantiate(enemy, transform.position, Quaternion.Euler(0f, 45f, 0f));
        }
        StartCoroutine(SpawnEnemy());
    }

}
