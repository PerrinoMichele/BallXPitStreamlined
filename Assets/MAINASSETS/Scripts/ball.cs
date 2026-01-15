using UnityEngine;

public class ball : MonoBehaviour
{
    public float speed = 16f;

    Rigidbody rb;

    public float magnetSpeed = 10f;
    bool isMagneted = false;
    Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(1);

        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 v = rb.linearVelocity.normalized;

            v.z = -Mathf.Max(Mathf.Abs(v.z), .3f);

            rb.linearVelocity = v.normalized * speed;
        }

        if (collision.gameObject.tag == "BottomWall")
        {
            {
                StartMagnet();
                return;
            }
        }
    }

    void StartMagnet()
    {
        isMagneted = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; // stop physics completely

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<playerInteraction>().AddBall();
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (isMagneted)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                magnetSpeed * Time.fixedDeltaTime
            );
            return;
        }

        speed += .03f;
        // Keep speed constant (classic arcade feel)
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
