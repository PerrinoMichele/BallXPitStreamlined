using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public bool isTakingDamage = false;
    public int healthPoints;
    public AudioClip getHitSfx;


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
        else 
        {
            if (gameObject.tag == "Player") { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
            else { Destroy(gameObject); }
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
            rend.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.08f);
        rend.material.color = baseColor;
        yield return new WaitForSeconds(.5f);
        isTakingDamage = false;
    }

}
