using System;
using DG.Tweening;
using UnityEngine;

public class XPCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("XP"))
        {
            other.transform.DOMove(transform.position, 0.1f);
        }
    }
}
