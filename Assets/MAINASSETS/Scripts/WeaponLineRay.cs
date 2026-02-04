using UnityEngine;

public class WeaponLineRay : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private LineRenderer mainRay;
    [SerializeField] private LineRenderer normalRay;

    [Header("Detection tweak")]
    [Tooltip("Diametro della palla — la detection userà metà di questo valore come raggio del SphereCast")]
    [SerializeField] private float projectileDiameter = 0.5f;
    [SerializeField] private float maxDistance = 80f;

    [Header("Bounce visual")]
    [Tooltip("Lunghezza della linea che mostra il rimbalzo (breve)")]
    [SerializeField] private float bounceLength = 2.0f;

    private void Update()
    {
        DrawLines();
    }

    private void DrawLines()
    {
        Vector3 direction = transform.forward.normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        bool hitSomething;
        float radius = Mathf.Max(0f, projectileDiameter * 0.5f);

        // Usa SphereCast con raggio pari a metà del diametro della palla per rilevare colpi "al bordo"
        if (radius > 0f)
            hitSomething = Physics.SphereCast(transform.position, radius, direction, out hit, maxDistance, hitLayers);
        else
            hitSomething = Physics.Raycast(ray, out hit, maxDistance, hitLayers);

        mainRay.enabled = true;
        mainRay.positionCount = 2;
        mainRay.SetPosition(0, transform.position);

        if (hitSomething)
        {
            // Calcola la distanza lungo forward proiettando il punto d'impatto sulla retta forward
            float distAlong = Mathf.Clamp(Vector3.Dot(hit.point - transform.position, direction), 0f, maxDistance);
            Vector3 visualHitPoint = transform.position + direction * distAlong;
            mainRay.SetPosition(1, visualHitPoint);

            // Se è stata assegnata la normalRay, disegna un breve rimbalzo collegato
            if (normalRay != null)
            {
                Vector3 reflected = Vector3.Reflect(direction, hit.normal).normalized;
                normalRay.enabled = true;
                normalRay.positionCount = 2;
                normalRay.SetPosition(0, visualHitPoint); // parte dalla fine della main line (visiva)
                normalRay.SetPosition(1, visualHitPoint + reflected * bounceLength);
            }
        }
        else
        {
            // Nessun hit: linea lunga in avanti
            mainRay.SetPosition(1, transform.position + direction * maxDistance);

            if (normalRay != null) normalRay.enabled = false;
        }
    }
}