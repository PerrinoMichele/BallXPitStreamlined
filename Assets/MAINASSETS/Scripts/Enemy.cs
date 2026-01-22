using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    private bool isAttacking = false;
    private GameObject player;

    private void Start()
    {
        player = FindFirstObjectByType<BallsHoldingAndShooting>().gameObject;
    }
    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        //If enemy reaches bottom of the screen damage player, then destroy itself
        if (transform.position.z <= -17 && !player.GetComponent<Health>().isTakingDamage)
        {
            player.gameObject.GetComponent<Health>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(!player.GetComponent<Health>().isTakingDamage)
            {
                player.gameObject.GetComponent<Health>().TakeDamage(1);
            }
        }
    }


    public void PunchScale()
    {
        if (DOTween.IsTweening(transform)) return;

        transform.DOPunchScale(Vector3.one * 0.2f, 0.15f);
    }
}
