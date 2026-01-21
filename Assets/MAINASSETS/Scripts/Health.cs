using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoints;
    public AudioClip getHitSfx;

    AudioSource audioSource;
    Renderer rend;
    MeshRenderer mesh;
    Vector3 originalScale;
    Color baseColor;

    void Awake()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
        rend = GetComponentInChildren<Renderer>();
        mesh = GetComponentInChildren<MeshRenderer>();
        originalScale = mesh.transform.localScale;
        baseColor = rend.material.color;
    }

    public void TakeDamage(int damage)
    {
        audioSource.volume = .15f;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(getHitSfx);

        if (healthPoints > 1)
        {
            StopCoroutine(PunchFlashVFX());
            StartCoroutine(PunchFlashVFX());
            healthPoints -= damage;
        }
        else { Destroy(gameObject); }
    }

    private IEnumerator PunchFlashVFX()
    {
        mesh.transform.localScale = originalScale * 1.15f;

        if (gameObject.tag == "Enemy")
        {
            rend.material.color = Color.white;
        }

        else if (gameObject.tag == "Player")
        {
            rend.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.08f);
        rend.material.color = baseColor;
        mesh.transform.localScale = originalScale;
    }

}
