using UnityEngine;
using System.Collections;

public enum BallType { Normal, Ghost, Iron }
public class Ball : MonoBehaviour
{
    public BallType BallType;
    public float speed = 16f;

    Rigidbody rb;

    public float magnetSpeed = 10f;
    bool isMagneted = false;
    Transform player;
    
    private float hitSpeedMultiplier = 1;
    private float bounceRandomness = 0.05f;
    private Vector3 lastVelocity = Vector3.zero;

    private void Start()
    {
        speed = BallType == BallType.Iron ? speed * 1.5f : speed;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var damage = BoostersController.Instance.GetBoosterImplementedValue(BoosterType.BallDamage, 1);
            damage = BallType == BallType.Iron ? damage * 2 : damage;
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);

            if (BallType != BallType.Ghost)
            {
                ChangeDirection(collision);
            }

            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.PunchScale();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ChangeDirection(collision);
        }
        else if (collision.gameObject.tag == "BottomWall")
        {
            StartMagnet();
        }
    }

    private void ChangeDirection(Collision collision)
    {
        if (collision.contactCount > 0)
        {
            hitSpeedMultiplier += 0.15f;
            Vector3 normal = collision.contacts[0].normal;
            Vector3 randomOffset = new Vector3(Random.Range(bounceRandomness, bounceRandomness), 0,
                Random.Range(-bounceRandomness, bounceRandomness)
            );

            Vector3 modifiedNormal = (normal + randomOffset).normalized;
            Vector3 incomingDir = lastVelocity.normalized;
            Vector3 reflectedDir = Vector3.Reflect(incomingDir, modifiedNormal);
            rb.linearVelocity = reflectedDir * speed * hitSpeedMultiplier;
        }
    }

    private void StartMagnet()
    {
        isMagneted = true;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; 
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<BallsHoldingAndShooting>().AddBall(BallType);
            hitSpeedMultiplier = 1.0f;
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (isMagneted)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, 
                magnetSpeed * Time.fixedDeltaTime);
        }

        lastVelocity = rb.linearVelocity;
    }
}
