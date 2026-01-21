using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
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
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
            ChangeDirection(collision);

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
            other.GetComponent<BallsHoldingAndShooting>().AddBall();
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
