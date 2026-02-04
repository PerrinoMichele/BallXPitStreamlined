using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float Speed = 0.5f;
    private bool isAttacking = false;
    private GameObject player;

    // Transform visivo su cui applicare l'effetto (primo figlio se esiste)
    private Transform visualTransform;

    // Memoria della scala originale del visual (per ripristinarla sempre)
    private Vector3 originalLocalScale;

    private void Start()
    {
        player = FindFirstObjectByType<BallsHoldingAndShooting>().gameObject;

        // Semplificato: usa il primo child se esiste, altrimenti fallback al root
        visualTransform = transform.childCount > 0 ? transform.GetChild(0) : transform;

        // Memorizza la scala originale del visual (fallback a Vector3.one se null)
        originalLocalScale = visualTransform != null ? visualTransform.localScale : Vector3.one;
    }

    void Update()
    {
        transform.position += Vector3.back * Speed * Time.deltaTime;

        if (transform.position.z <= -17 && !player.GetComponent<Health>().isTakingDamage)
        {
            player.gameObject.GetComponent<Health>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!player.GetComponent<Health>().isTakingDamage)
            {
                player.gameObject.GetComponent<Health>().TakeDamage(1);
            }
        }
    }

    public void PunchScale()
    {
        if (visualTransform == null) return;

        // Assicuriamoci di cancellare eventuali tween attivi e ripristinare la scala originale
        DOTween.Kill(visualTransform);
        visualTransform.localScale = originalLocalScale;

        // Applichiamo il punch sul localScale del transform visivo e assicuriamoci che alla fine venga ripristinata
        visualTransform.DOPunchScale(Vector3.one * 0.4f, 0.15f)
            .OnComplete(() =>
            {
                // Forza il ripristino esatto della scala originale al termine del tween
                if (visualTransform != null)
                    visualTransform.localScale = originalLocalScale;
            })
            .SetTarget(visualTransform);
    }
}
