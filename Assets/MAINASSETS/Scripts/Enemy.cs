using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }

    public void PunchScale()
    {
        if (DOTween.IsTweening(transform)) return;

        transform.DOPunchScale(Vector3.one * 0.2f, 0.15f);
    }
}
