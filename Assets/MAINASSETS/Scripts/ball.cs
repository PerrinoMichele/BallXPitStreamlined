using UnityEngine;
using System.Collections;

public enum BallType
{
    Normal, Ghost, Iron, Poison, Vampire, Lightning,
    EarthQuake, BroodMother, EggSack, Dark, HorizontalLaser, VerticalLaser
}
public class Ball : MonoBehaviour
{
    public BallType BallType;
    public float speed = 16f;

    Rigidbody rb;

    public float magnetSpeed = 10f;
    public float defaultDamage = 1.0f;
    bool isMagneted = false;
    Transform player;

    private float hitSpeedMultiplier = 1;
    private float bounceRandomness = 0.05f;
    private Vector3 lastVelocity = Vector3.zero;

    private LineRenderer laserLeft;
    private LineRenderer laserRight;
    private GameObject laserLeftObj;
    private GameObject laserRightObj;
    private LineRenderer laserFront; // Reusing "Front" for Forward/Up visual
    private LineRenderer laserBack;  // Reusing "Back" for Back/Down visual
    private GameObject laserFrontObj;
    private GameObject laserBackObj;
    private float laserMaxDist = 50f;

    private void Start()
    {
        speed = BallType == BallType.Iron ? speed * 1.5f : speed;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.forward * speed;

        if (BallType == BallType.HorizontalLaser)
        {
            SetupHorizontalLasers();
        }
        else if (BallType == BallType.VerticalLaser)
        {
            SetupVerticalLasers();
        }
    }

    private void SetupHorizontalLasers()
    {
        CreateLaser(out laserLeft, out laserLeftObj, "LaserLeft");
        CreateLaser(out laserRight, out laserRightObj, "LaserRight");
    }

    private void SetupVerticalLasers()
    {
        CreateLaser(out laserFront, out laserFrontObj, "LaserFront");
        CreateLaser(out laserBack, out laserBackObj, "LaserBack");
    }

    private void CreateLaser(out LineRenderer lr, out GameObject obj, string name)
    {
        obj = new GameObject(name);
        lr = obj.AddComponent<LineRenderer>();
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        // Use a simple shader, assuming Sprites/Default is available
        Material mat = new Material(Shader.Find("Sprites/Default"));
        lr.material = mat;

        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.positionCount = 2;
        lr.useWorldSpace = true; // Handle positions manually
    }

    private void Update()
    {
        if (BallType == BallType.HorizontalLaser)
        {
            UpdateLaser(laserLeft, Vector3.left);
            UpdateLaser(laserRight, Vector3.right);
        }
        else if (BallType == BallType.VerticalLaser)
        {
            UpdateLaser(laserFront, Vector3.forward);
            UpdateLaser(laserBack, Vector3.back);
        }
    }

    private void UpdateLaser(LineRenderer lr, Vector3 direction)
    {
        if (lr == null) return;

        Vector3 startPos = transform.position;
        lr.SetPosition(0, startPos);

        RaycastHit hit;
        // Raycast against everything or specific layers? Using default for now.
        // We probably want to hit Enemies and Walls.
        if (Physics.Raycast(startPos, direction, out hit, laserMaxDist))
        {
            lr.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Health>()?.TakeDamage(defaultDamage * Time.deltaTime);
            }
        }
        else
        {
            lr.SetPosition(1, startPos + direction * laserMaxDist);
        }
    }

    private void OnDestroy()
    {
        if (laserLeftObj != null) Destroy(laserLeftObj);
        if (laserRightObj != null) Destroy(laserRightObj);
        if (laserFrontObj != null) Destroy(laserFrontObj);
        if (laserBackObj != null) Destroy(laserBackObj);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(defaultDamage);

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

            // Se la collisione è contro un nemico, rimbalzo deterministico (nessun offset casuale).
            // Altrimenti manteniamo il comportamento precedente con randomness.
            Vector3 randomOffset = Vector3.zero;
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                randomOffset = new Vector3(
                    Random.Range(-bounceRandomness, bounceRandomness),
                    0f,
                    Random.Range(-bounceRandomness, bounceRandomness)
                );
            }

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
