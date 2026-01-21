using UnityEngine;

public class Ball45 : MonoBehaviour
{
    public float speed = 12f;
    public LayerMask hitMask;          // set to Walls + Enemies layers in inspector
    public float skin = 0.01f;         // small offset to prevent sticking

    Vector3 dir = Vector3.forward;
    float radius;

    void Awake()
    {
        radius = GetComponent<SphereCollider>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z);
        dir = Snap45(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Backwall")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float dist = speed * Time.deltaTime;

        // SphereCast prevents tunneling and gives a clean hit normal
        if (Physics.SphereCast(transform.position, radius, dir, out RaycastHit hit, dist + skin, hitMask))
        {
            // Move to just before impact
            transform.position = hit.point - dir * skin;

            // Damage
            if (hit.collider.CompareTag("Enemy"))
                hit.collider.GetComponent<Health>()?.TakeDamage(1);

            // Reflect + snap to 45°
            dir = Snap45(Vector3.Reflect(dir, hit.normal));

            // Push out a bit along the normal so we don't immediately re-hit
            transform.position += hit.normal * (radius + skin);

            // Use remaining distance after the hit (optional but helps)
            float remaining = dist - hit.distance;
            if (remaining > 0f)
                transform.position += dir * remaining;
        }
        else
        {
            transform.position += dir * dist;
        }
    }

    static Vector3 Snap45(Vector3 v)
    {
        v.y = 0f;
        if (v.sqrMagnitude < 1e-6f) return Vector3.forward;

        float angle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;     // 0 = +Z
        float snapped = Mathf.Round(angle / 45f) * 45f;
        float rad = snapped * Mathf.Deg2Rad;

        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)).normalized;
    }

    void FixedUpdate()
    {
        speed += .03f;
    }
}