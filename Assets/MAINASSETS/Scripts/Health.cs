using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float healthPoints;
    public bool isTakingDamage = false;
    public AudioClip getHitSfx;
    public Image fill;


    public AudioSource audioSource;
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

    public void TakeDamage(float damage)
    {
        audioSource.volume = .15f;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(getHitSfx);

        if (healthPoints > 0)
        {
            StopCoroutine(PunchFlashVFX());
            StartCoroutine(PunchFlashVFX());
            healthPoints -= damage;
        }
        else
        {
            // Se è un nemico: spawn XP e distruggi
            if (gameObject.CompareTag("Enemy"))
            {
                XPSpawnController.Instance.SpawnXP(transform.position);
                Destroy(gameObject);
            }
            // Se è il player: riavvia la scena (minima modifica)
            else if (gameObject.CompareTag("Player"))
            {
                // Disabilita input/ulteriori effetti se necessario (minimo cambiamento)
                StartCoroutine(RestartSceneCoroutine());
            }
            else
            {
                // fallback generico
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator PunchFlashVFX()
    {
        isTakingDamage = true;
        if (gameObject.tag == "Enemy")
        {
            rend.material.color = Color.white;
        }

        else if (gameObject.tag == "Player")
        {
            fill.fillAmount = (float)healthPoints / 10;
            print(fill.fillAmount);
            rend.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.08f);
        rend.material.color = baseColor;
        yield return new WaitForSeconds(.5f);
        isTakingDamage = false;
    }

    private IEnumerator RestartSceneCoroutine()
    {
        // breve ritardo reale per permettere SFX/feedback; usa realtime per essere indipendente da timeScale
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
