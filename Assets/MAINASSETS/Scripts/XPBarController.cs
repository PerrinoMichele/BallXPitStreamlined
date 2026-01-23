using System;
using Core.Events;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class XPBarController : MonoBehaviour
{
    [SerializeField] private Image barImage;

    private int currentXPAmount = 0;
    private int maxXPAmountToCollect = 10;
    private void Start()
    {
        EventBus.Subscribe<XPCollectEvent>(OnXpCollected);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<XPCollectEvent>(OnXpCollected);
    }

    private void OnXpCollected(XPCollectEvent xpCollectEvent)
    {
        currentXPAmount += xpCollectEvent.XP;
        float targetProgress = (float) currentXPAmount / maxXPAmountToCollect;

        DOTween.Kill(barImage, true);
        barImage.DOFillAmount(targetProgress, 0.1f).OnComplete(() =>
        {
            if (currentXPAmount >= maxXPAmountToCollect)
            {
                Debug.Log("Need to Show Booster Screen");
                currentXPAmount = 0;
                barImage.fillAmount = 0.0f;
            }
        });
        
    }
}
