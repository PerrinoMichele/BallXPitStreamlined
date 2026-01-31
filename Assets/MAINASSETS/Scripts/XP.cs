using System;
using DG.Tweening;
using UnityEngine;

public class XP : MonoBehaviour
{
    private float speed;
    private void Awake()
    {
        speed = Enemy.Speed;
    }

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}
