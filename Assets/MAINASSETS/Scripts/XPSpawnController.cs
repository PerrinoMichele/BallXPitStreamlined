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
    [SerializeField] private ParticleImage xpParticleEffectsPrefab;

    private List<GameObject> availableXPsToCollect;

    private void Awake()
    {
        Instance = this;
        availableXPsToCollect = new List<GameObject>();
    }
    
    public void SpawnXP(Vector3 position)
    {
        var xpModel = Instantiate(xpPrefab, position, Quaternion.identity);
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
                    SpawnParticleEffect(xpToCollect.transform.position);
                    Destroy(xpToCollect);
                });
            }
        }
    }

    private void SpawnParticleEffect(Vector3 collectPosition)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, collectPosition);
        var xpParticleEffect = Instantiate(xpParticleEffectsPrefab,xpParticleEffectsParentContainer);
        xpParticleEffect.attractorTarget = targetAttractorTransform;
        xpParticleEffect.GetComponent<RectTransform>().position = screenPoint;
        xpParticleEffect.Play();
    }

    private void OnDestroyedXP(GameObject xp)
    {
        if (availableXPsToCollect.Contains(xp))
        {
            availableXPsToCollect.Remove(xp);
        }
    }
    
}
