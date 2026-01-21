using UnityEngine;

public class WeaponLineRay : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private LineRenderer mainRay;
    [SerializeField] private LineRenderer normalRay;

    private void Update()
    {
        DrawLines();
    }

    private void DrawLines()
    {
        Vector3 direction = transform.forward; 

        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayers))
        {
            mainRay.enabled = true;
            mainRay.SetPosition(0, transform.position);
            mainRay.SetPosition(1, hit.point);

            normalRay.enabled = true;
            normalRay.SetPosition(0, hit.point);
            normalRay.SetPosition(1, hit.point + hit.normal * 5.0f);
        }
        else
        {
            mainRay.enabled = true;
            mainRay.SetPosition(0, transform.position);
            mainRay.SetPosition(1, transform.position + direction * 80.0f);

            normalRay.enabled = false;
        }
    }
}