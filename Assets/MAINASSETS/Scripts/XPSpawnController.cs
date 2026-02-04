using System;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Core.Events;
using DG.Tweening;
using UnityEngine;

public class XPSpawnController : MonoBehaviour
{
    public static XPSpawnController Instance;

    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform xpModelsParentContainer;
    [SerializeField] private GameObject xpPrefab;
    
    [Header("Particle Effects")]
    [SerializeField] private RectTransform xpParticleEffectsParentContainer;
    [SerializeField] private RectTransform targetAttractorTransform;
    //[SerializeField] private ParticleImage xpParticleEffectsPrefab;

    [Header("Spawn Randomization")]
    [SerializeField] private bool randomizeSpawnPosition = true;
    [SerializeField] private float spawnOffsetRadius = 0.5f;
    [SerializeField] private bool randomizeSpawnRotation = true;

    private List<GameObject> availableXPsToCollect;

    private void Awake()
    {
        Instance = this;
        availableXPsToCollect = new List<GameObject>();
    }
    
    public void SpawnXP(Vector3 position)
    {
        // Applica un offset casuale sul piano XZ attorno alla posizione fornita
        Vector3 spawnPos = position;
        if (randomizeSpawnPosition && spawnOffsetRadius > 0f)
        {
            Vector2 rnd = UnityEngine.Random.insideUnitCircle * spawnOffsetRadius;
            spawnPos += new Vector3(rnd.x, 0f, rnd.y);
        }

        // Applica una rotazione casuale attorno all'asse Y se abilitato
        Quaternion spawnRot = Quaternion.identity;
        if (randomizeSpawnRotation)
        {
            spawnRot = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
        }

        var xpModel = Instantiate(xpPrefab, spawnPos, spawnRot);
        xpModel.transform.parent = xpModelsParentContainer;
        availableXPsToCollect.Add(xpModel);
    }

    private void Update()
    {
        CheckXPsToCollect(characterTransform.position);
    }

    private void CheckXPsToCollect(Vector3 characterPosition)
    {
        foreach (var xpToCollect in availableXPsToCollect)
        {
            if ((xpToCollect.transform.position - characterPosition).sqrMagnitude <= 1.0f)
            {
                if (DOTween.IsTweening(xpToCollect.transform)) return;
                xpToCollect.transform.DOMove(characterPosition, 0.05f).OnComplete(() =>
                {
                    OnDestroyedXP(xpToCollect);

                    // Pubblica direttamente l'evento di raccolta XP perché
                    // l'FX che prima lo pubblicava è stato rimosso.
                    EventBus.Publish(new XPCollectEvent(1));

                    // se vuoi riabilitare il VFX, ripristina qui l'instanziazione:
                    // SpawnParticleEffect(xpToCollect.transform.position);

                    Destroy(xpToCollect);
                });
            }
        }
    }

    //private void SpawnParticleEffect(Vector3 collectPosition)
    //{
    //    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, collectPosition);
    //    var xpParticleEffect = Instantiate(xpParticleEffectsPrefab,xpParticleEffectsParentContainer);
    //    xpParticleEffect.attractorTarget = targetAttractorTransform;
    //    xpParticleEffect.GetComponent<RectTransform>().position = screenPoint;
    //    xpParticleEffect.Play();
    //}

    private void OnDestroyedXP(GameObject xp)
    {
        if (availableXPsToCollect.Contains(xp))
        {
            availableXPsToCollect.Remove(xp);
        }
    }
    
}
